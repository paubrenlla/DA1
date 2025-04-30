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
    private Estado _estadoActual;
    private List<Tarea> _tareasDependencia = new List<Tarea>();

    public IReadOnlyList<Tarea> TareasDependencia => _tareasDependencia.AsReadOnly();
    
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
                throw new ArgumentNullException(nameof(value), "La descripción es requerido y no puede estar vacío.");
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
        set => _duracion = value;
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
    
    public Tarea(string titulo, string descripcion, DateTime fechaInicio, TimeSpan duracion, bool esCritica)
    {
        Id = ++_contadorId;
        Titulo = titulo;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
        Duracion = duracion;
        EsCritica = esCritica;
        EstadoActual = new Estado(TipoEstadoTarea.Pendiente);  // Inicializamos como Pendiente
    }
    

    public void modificarEstado(TipoEstadoTarea nuevoEstado, DateTime fecha)
    {   
            EstadoActual.Valor = nuevoEstado;
            EstadoActual.Fecha = fecha;
        }
    
    public void AgregarDependencia(Tarea tarea)
    {
        if (tarea == null)
            throw new ArgumentNullException(nameof(tarea));

        _tareasDependencia.Add(tarea);
        modificarEstado(TipoEstadoTarea.Bloqueada,DateTime.Now);
    }
    public void ActualizarEstadoSegunDependencias()
    {
        if (_tareasDependencia.Count == 0) 
            return;

        // Verifica si todas las dependencias (directas e indirectas) están efectuadas
        bool todasEfectuadas = VerificarDependenciasCompletadas(_tareasDependencia);

        EstadoActual.Valor = todasEfectuadas ? TipoEstadoTarea.Pendiente : TipoEstadoTarea.Bloqueada;
    }

// Método recursivo para verificar dependencias anidadas
    private bool VerificarDependenciasCompletadas(IEnumerable<Tarea> dependencias)
    {
        foreach (var tarea in dependencias)
        {
            // Si la tarea dependiente no está efectuada, retorna false inmediatamente
            if (tarea.EstadoActual.Valor != TipoEstadoTarea.Efectuada)
                return false;

            // Si la tarea dependiente tiene sus propias dependencias, verifica recursivamente
            if (tarea._tareasDependencia.Count > 0 && !VerificarDependenciasCompletadas(tarea._tareasDependencia))
                return false;
        }

        return true; // Todas las dependencias (anidadas) están efectuadas
    }
}

