using Domain.Enums;

namespace Domain;

public class Tarea
{
    private static int _contadorId = 0;
    private int _id;
    private string _titulo;
    private string _descripcion;
    private DateTime _fechaInicio;
    private TimeSpan _duracion;
    private bool _esCritica;
    public Proyecto? Proyecto { get; set; }
    private Estado _estadoActual = new Estado(TipoEstadoTarea.Pendiente);
    private List<Tarea> _tareasDependencia = new List<Tarea>();
    private List<Tarea> _tareasSucesoras = new List<Tarea>();
    private List<RecursoNecesario> _recursosNecesarios =  new List<RecursoNecesario>();
    private List<Usuario> _usuariosAsignados  = new List<Usuario>();

    private TimeSpan _holgura;
    public DateTime EarlyStart { get; set; }
    public DateTime LateStart { get; set; }
    public DateTime EarlyFinish { get; set; }
    public DateTime LateFinish { get; set; }
    private static readonly TimeSpan DuracionMinimaTarea = TimeSpan.FromHours(1);
    
    public Tarea(string titulo, string descripcion, DateTime fechaInicio, TimeSpan duracion, bool esCritica)
    {
        Titulo = titulo;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
        Duracion = duracion;
        EsCritica = esCritica;
        Id = _contadorId++;
    }
    public int Id
    {
        get => _id;
        set => _id = value;
    }
    public string Titulo
    {
        get => _titulo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), "El titulo es requerido y no puede estar vacío.");
            _titulo = value;
        }
    }
    
    public string Descripcion
    {
        get => _descripcion;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), "La descripción es requerida y no puede estar vacía.");
            _descripcion = value;
        }
    }
    
    public DateTime FechaInicio
    {
        get => _fechaInicio;
        set => _fechaInicio = value;
    }

    public TimeSpan Duracion
    {
        get => _duracion;
        set
        {
            if (value < DuracionMinimaTarea )
                throw new ArgumentOutOfRangeException(nameof(value), "La duración mínima es de 1 hora.");
            _duracion = value;
        }
    }
    public bool EsCritica
    {
        get => _esCritica;
        set => _esCritica = value;
    }
    public Estado EstadoActual
    {
        get => _estadoActual;
        set => _estadoActual = value;
    }
    public TimeSpan Holgura
    {
        get => _holgura;
        set => _holgura = value;
    }

    public List<Usuario> UsuariosAsignados
    {
        get => _usuariosAsignados;
    }
    
    public List<RecursoNecesario> RecursosNecesarios => _recursosNecesarios!;
    public List<Tarea> TareasDependencia => _tareasDependencia!;
    public List<Tarea> TareasSucesoras => _tareasSucesoras!;
    
    private void ModificarEstado(TipoEstadoTarea nuevoEstado, DateTime fecha)
    {   
            EstadoActual.Valor = nuevoEstado;
            EstadoActual.Fecha = fecha;
    }
    
    public void AgregarDependencia(Tarea tarea)
    {
        if (tarea == null)
            throw new ArgumentNullException(nameof(tarea));

        _tareasDependencia.Add(tarea);
        tarea._tareasSucesoras.Add(this);
        ActualizarEstado();
        Proyecto.CalcularRutaCritica();
    }
    public void ActualizarEstado()
    {
        if (this.EstadoActual.Valor == TipoEstadoTarea.Efectuada 
            || this.EstadoActual.Valor == TipoEstadoTarea.Ejecutandose)
        {
            return;
        }
        if (VerificarDependenciasCompletadas() && VerificarRecursosDisponibles())
        {
            ModificarEstado(TipoEstadoTarea.Pendiente, DateTime.Now);
            Notificacion notificacion = new Notificacion("La tarea " + Titulo + " ha pasado a pendiente.");
            notificacion.AgregarUsuarios(UsuariosAsignados);
            notificacion.AgregarUsuario(Proyecto.Admin);
            return;
        }
        ModificarEstado(TipoEstadoTarea.Bloqueada, DateTime.Now);
    }

    public void AgregarRecurso(Recurso recurso, int cantidadNecesaria)
    {
        if (cantidadNecesaria <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cantidadNecesaria));    
        }
        
        foreach (RecursoNecesario recursoNecesario in RecursosNecesarios)
        {
            if (recursoNecesario.Recurso == recurso)
            {
                recursoNecesario.CantidadNecesaria += cantidadNecesaria;
                ActualizarEstado();
                return;
            }
        }
        _recursosNecesarios.Add(new RecursoNecesario(recurso, cantidadNecesaria));
        recurso.AgregarRecursoATarea(this);
        ActualizarEstado();
    }

    public void EliminarRecurso(RecursoNecesario recurso)
    {
        RecursosNecesarios.Remove(recurso);
        ActualizarEstado();
    }

    public bool VerificarRecursosDisponibles()
    {
        foreach (RecursoNecesario recursoNecesario in RecursosNecesarios)
        {
            if (!recursoNecesario.Recurso.EstaDisponible(recursoNecesario.CantidadNecesaria))
            {
                return false;
            }
        }
        return true;
    }

    public void ConsumirRecursos()
    {
        foreach (RecursoNecesario recursoNecesario in RecursosNecesarios)
        {
            recursoNecesario.Recurso.ConsumirRecurso(recursoNecesario.CantidadNecesaria);
        }
    }

    public void LiberarRecursos()
    {
        foreach (RecursoNecesario recursoNecesario in RecursosNecesarios)
        {
            recursoNecesario.Recurso.LiberarRecurso(recursoNecesario.CantidadNecesaria);
        }
    }
    
    private bool VerificarDependenciasCompletadas()
    {
        foreach (Tarea tarea in TareasDependencia)
        {
            if (tarea.EstadoActual.Valor != TipoEstadoTarea.Efectuada)
                return false;
        }
        return true;
    }

    public void MarcarTareaComoCompletada()
    {
        ModificarEstado(TipoEstadoTarea.Efectuada, DateTime.Now);
        LiberarRecursos();
        if (DateTime.Now > LateFinish)
        {
            Notificacion notificacion = new Notificacion("La tarea " + Titulo + " ha pasado a sido completada con demora.");
            notificacion.AgregarUsuarios(UsuariosAsignados);
            notificacion.AgregarUsuario(Proyecto.Admin);
        }
        else
        {
            Notificacion notificacion = new Notificacion("La tarea " + Titulo + " ha sido completada en fecha.");
            notificacion.AgregarUsuarios(UsuariosAsignados);
            notificacion.AgregarUsuario(Proyecto.Admin);
        }

        ReevaluarTareasPosteriores();
    }

    public void MarcarTareaComoEjecutandose()
    {
        if (VerificarDependenciasCompletadas() && VerificarRecursosDisponibles())
        {
            ModificarEstado(TipoEstadoTarea.Ejecutandose, DateTime.Now);
            ConsumirRecursos();
            Notificacion notificacion = new Notificacion("La tarea " + Titulo + " ha pasado a ejecución.");
            notificacion.AgregarUsuarios(UsuariosAsignados);
            notificacion.AgregarUsuario(Proyecto.Admin);
        }
    }

    private void ReevaluarTareasPosteriores()
    {
        foreach (Tarea tarea in TareasSucesoras)
        {
            tarea.ActualizarEstado();
        }
    }
    
    public void AgregarUsuario(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));
        Notificacion notificacion = new Notificacion("El usuario " + Titulo + " ha sido agregado a la tarea " + Titulo);
        notificacion.AgregarUsuarios(UsuariosAsignados);
        if (Proyecto.Admin != null)
        {
            notificacion.AgregarUsuario(Proyecto.Admin);
        }
        _usuariosAsignados.Add(usuario);
    }

    public void Modificar(string titulo, string descripcion, DateTime fechaInicio, TimeSpan duracion)
    {
        Titulo = titulo;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
        Duracion = duracion;
        
        Notificacion notificacion = new Notificacion("La tarea " + Titulo + " ha sido modificada.");
        notificacion.AgregarUsuarios(UsuariosAsignados);
        notificacion.AgregarUsuario(Proyecto.Admin);
        Proyecto.CalcularRutaCritica();
    }
}

