namespace BusinessLogic;

public class Proyecto
{
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaInicio { get; set; }
    public Proyecto(string nombre, string descripcion, DateTime fechaInicio)
    {
        if (descripcion.Length > 400)
            throw new ArgumentException("La descripción no puede superar los 400 caracteres.");
        
        Nombre = nombre;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
    }
}