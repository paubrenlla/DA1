namespace BusinessLogic;

public class Proyecto
{
    private static int _contadorId = 0; // contador compartido entre todas las instancias

    public int Id { get; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaInicio { get; set; }
    public List<Tarea> TareasAsociadas { get; set; }
    public Proyecto(string nombre, string descripcion, DateTime fechaInicio)
    {
        if (fechaInicio.Date < DateTime.Now.Date)
            throw new ArgumentException("La fecha de inicio no puede ser anterior a hoy.");
        
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede ser nulo o vacío.");
        
        if (descripcion.Length > 400)
            throw new ArgumentException("La descripción no puede superar los 400 caracteres.");
        Id = _contadorId++;
        Nombre = nombre;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
        TareasAsociadas = new List<Tarea>();
    }

    public void agregarTarea(Tarea tarea)
    {
        TareasAsociadas.Add(tarea);
    }
    
    public void eliminarTarea(Tarea tarea)
    {
        TareasAsociadas.Remove(tarea);
    }
}