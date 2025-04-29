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
        if (string.IsNullOrEmpty(nombre)) 
            throw new ArgumentNullException("Se debe ingresar un nombre.");
        Id = _contadorId++;
        Nombre = nombre;
        Tipo = tipo;
        Descripcion = descripcion;
    }
}