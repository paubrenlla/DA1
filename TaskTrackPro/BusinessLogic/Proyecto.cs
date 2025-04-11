namespace BusinessLogic;

public class Proyecto
{
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaInicio { get; set; }
    public Proyecto(string nombre, string descripcion, DateTime fechaInicio)
    {
        Nombre = nombre;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
    }
}