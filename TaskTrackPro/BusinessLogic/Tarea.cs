namespace BusinessLogic;

public class Tarea
{
    private static int _contadorId = 0;
    private int _id;
    private string _titulo;
    private string _descripcion;
    private DateTime _fechaInicio;
    private Duracion _duracion;
    private bool _esCritica;
    private Estado _estadoActual;
    
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
    
    public Duracion Duracion
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
    
    public Tarea(string titulo, string descripcion, DateTime fechaInicio, Duracion duracion, bool esCritica)
    {
        Id = ++_contadorId;
        Titulo = titulo;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
        Duracion = duracion;
        EsCritica = esCritica;
        EstadoActual = new Estado(TipoEstadoTarea.Pendiente);  // Inicializamos como Pendiente
    }
}

