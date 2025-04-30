namespace BusinessLogic;

public class Recurso
{
    private static int _contadorId = 1;
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public string Descripcion { get; set; }
    public bool SePuedeCompartir { get; set; }
    public int CantidadDelRecurso { get; set; }
    public int CantidadEnUso { get; set; }
    public Proyecto? ProyectoAlQuePertenece { get; set; }
    public int Id { get; set; }

    public Recurso(string nombre, string tipo, string descripcion, bool sePuedeCompartir, int cantidadDelRecurso) 
        : this(nombre, tipo, descripcion, sePuedeCompartir, cantidadDelRecurso, null) {}

    public Recurso(string nombre, string tipo, string descripcion, bool sePuedeCompartir, int cantidadDelRecurso, Proyecto? proyectoAlQuePertenece) 
    {
        if (string.IsNullOrEmpty(nombre)) 
            throw new ArgumentNullException("Se debe ingresar un nombre.");
        
        if (string.IsNullOrEmpty(tipo))
            throw new ArgumentNullException("Se debe ingresar un tipo.");
        
        if (cantidadDelRecurso <= 0)
            throw new ArgumentException("La cantidad del recurso debe ser mayor a 0.");

        Id = _contadorId++;
        Nombre = nombre;
        Tipo = tipo;
        Descripcion = descripcion;
        SePuedeCompartir = sePuedeCompartir;
        CantidadDelRecurso = cantidadDelRecurso;
        CantidadEnUso = 0;
        ProyectoAlQuePertenece = proyectoAlQuePertenece;

    }
}