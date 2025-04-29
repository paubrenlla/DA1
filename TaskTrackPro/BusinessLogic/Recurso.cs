namespace BusinessLogic;

public class Recurso
{
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public string Descripcion { get; set; }

    public Recurso(string nombre, string tipo, string descripcion)
    {
        Nombre = nombre;
        Tipo = tipo;
        Descripcion = descripcion;
    }
}