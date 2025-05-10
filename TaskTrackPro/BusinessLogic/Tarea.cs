using BusinessLogic.Enums;

namespace BusinessLogic;

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
    private List<RecursoNecesario> _recursos =  new List<RecursoNecesario>();
    private List<Usuario> _usuariosAsignados  = new List<Usuario>();

    private TimeSpan _holgura;
    

    public DateTime EarlyStart { get; set; }
    public DateTime LateStart { get; set; }
    public DateTime EarlyFinish { get; set; }
    public DateTime LateFinish { get; set; }

public IReadOnlyList<Usuario> UsuariosAsignados => _usuariosAsignados.AsReadOnly();
    public IReadOnlyList<Tarea> TareasDependencia => _tareasDependencia.AsReadOnly();
    public IReadOnlyList<Tarea> TareasSucesoras => _tareasSucesoras.AsReadOnly();
    public IReadOnlyList<RecursoNecesario> Recursos => _recursos.AsReadOnly();
    private static readonly TimeSpan DuracionMinimaTarea = TimeSpan.FromHours(1);
    
    public Tarea(string titulo, string descripcion, DateTime fechaInicio, TimeSpan duracion, bool esCritica)
    {
        Titulo = titulo;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
        Duracion = duracion;
        EsCritica = esCritica;
        Id = ++_contadorId;
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
    }
    public void ActualizarEstado()
    {
        if (TareasDependencia.Count == 0) 
            return;

        if (VerificarDependenciasCompletadas() && VerificarRecursosDisponibles())
        {
            ModificarEstado(TipoEstadoTarea.Pendiente, DateTime.Now);
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
        
        foreach (RecursoNecesario recursoNecesario in Recursos)
        {
            if (recursoNecesario.Recurso == recurso)
            {
                recursoNecesario.CantidadNecesaria += cantidadNecesaria;
                return;
            }
        }
        _recursos.Add(new RecursoNecesario(recurso, cantidadNecesaria));
        ActualizarEstado();
    }

    public bool VerificarRecursosDisponibles()
    {
        foreach (RecursoNecesario recursoNecesario in Recursos)
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
        foreach (RecursoNecesario recursoNecesario in Recursos)
        {
            recursoNecesario.Recurso.ConsumirRecurso(recursoNecesario.CantidadNecesaria);
        }
    }

    public void LiberarRecursos()
    {
        foreach (RecursoNecesario recursoNecesario in Recursos)
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

        ReevaluarTareasPosteriores();
    }

    public void MarcarTareaComoEjecutandose()
    {
        if (VerificarDependenciasCompletadas() && VerificarRecursosDisponibles())
        {
            ModificarEstado(TipoEstadoTarea.Ejecutandose, DateTime.Now);
            ConsumirRecursos();
            //TODO: reevaluar todas las tareas de DB por si usan los mismos recursos
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
        _usuariosAsignados.Add(usuario);
    }
    
    
}

