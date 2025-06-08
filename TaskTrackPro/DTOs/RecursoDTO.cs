namespace DTOs;

public class RecursoDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public string Descripcion { get; set; }
    public bool SePuedeCompartir { get; set; }
    public int CantidadDelRecurso { get; set; }
    public int CantidadEnUso { get; set; }
}