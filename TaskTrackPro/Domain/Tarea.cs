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
    private Estado _estadoActual = new Estado(TipoEstadoTarea.Pendiente);
    private List<Tarea> _tareasDependencia = new List<Tarea>();
    private List<Tarea> _tareasSucesoras = new List<Tarea>();
    private List<Usuario> _usuariosAsignados  = new List<Usuario>();
    private bool recursosForzados = false;
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
        RecursosForzados = false;
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
    
    public List<Tarea> TareasDependencia => _tareasDependencia!;
    public List<Tarea> TareasSucesoras => _tareasSucesoras!;
    
    public bool RecursosForzados { get; set; }
    
    private void ModificarEstado(TipoEstadoTarea nuevoEstado, DateTime fecha)
    {   
        EstadoActual.Valor = nuevoEstado;
        EstadoActual.Fecha = fecha;
    }
    
    public void AgregarDependencia(Tarea tarea)
    {
        if (tarea == null)
            throw new ArgumentNullException(nameof(tarea));
        if (tarea.FechaInicio > this.FechaInicio)
            throw new ArgumentException("La dependencia no puede iniciar despues de esta tarea");

        _tareasDependencia.Add(tarea);
        tarea._tareasSucesoras.Add(this);
        ActualizarEstado();
    }
    public void ActualizarEstado()
    {
        if (this.EstadoActual.Valor == TipoEstadoTarea.Efectuada 
            || this.EstadoActual.Valor == TipoEstadoTarea.Ejecutandose)
        {
            return;
        }
        if (VerificarDependenciasCompletadas())
        {
            ModificarEstado(TipoEstadoTarea.Pendiente, DateTime.Now);
            return;
        }
        ModificarEstado(TipoEstadoTarea.Bloqueada, DateTime.Now);
    }

    public void ActualizarEstado(bool recursosDisponibles)
    {
        if (this.EstadoActual.Valor == TipoEstadoTarea.Efectuada 
            || this.EstadoActual.Valor == TipoEstadoTarea.Ejecutandose)
        {
            return;
        }
        if (VerificarDependenciasCompletadas() && recursosDisponibles)
        {
            ModificarEstado(TipoEstadoTarea.Pendiente, DateTime.Now);
            return;
        }
        ModificarEstado(TipoEstadoTarea.Bloqueada, DateTime.Now);
    }
    
    public bool VerificarDependenciasCompletadas()
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
        ReevaluarTareasPosteriores();
    }

    public void MarcarTareaComoEjecutandose()
    {
        if (VerificarDependenciasCompletadas())
        {
            ModificarEstado(TipoEstadoTarea.Ejecutandose, DateTime.Now);
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
        _usuariosAsignados.Add(usuario);
    }

    public void Modificar(string titulo, string descripcion, DateTime fechaInicio, TimeSpan duracion)
    {
        Titulo = titulo;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
        Duracion = duracion;
    }

    public void EliminarDependencia(Tarea tarea)
    {
        TareasDependencia.Remove(tarea);
    }

    public void EliminarSucesora(Tarea tarea)
    {
        TareasSucesoras.Remove(tarea);
    }

    public bool EstaCompletaoEjecutandose()
    {
        return EstadoActual.Valor == TipoEstadoTarea.Efectuada || EstadoActual.Valor == TipoEstadoTarea.Ejecutandose;    
    }

    public bool EstaBloqueada()
    {
        return EstadoActual.Valor == TipoEstadoTarea.Bloqueada;
    }

    public bool DependenciasEfectuadas()
    {
        return TareasDependencia.All(tarea => tarea.EstadoActual.Valor == TipoEstadoTarea.Efectuada);
    }

    public bool EstaEjecutandose()
    {
        return EstadoActual.Valor == TipoEstadoTarea.Ejecutandose;
    }
}

