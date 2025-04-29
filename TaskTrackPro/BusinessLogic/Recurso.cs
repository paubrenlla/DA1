namespace BusinessLogic;

public class Recurso
{
    private static int _contadorId = 1;
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public string Descripcion { get; set; }
    public int Id { get; set; }

    public Recurso(string nombre, string tipo, string descripcion)
    {
        Id = _contadorId++;
        Nombre = nombre;
        Tipo = tipo;
        Descripcion = descripcion;
    }
}