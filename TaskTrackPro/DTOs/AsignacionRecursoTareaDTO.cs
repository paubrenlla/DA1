namespace DTOs;

public class AsignacionRecursoTareaDTO
{
    public int Id { get; set; }
    public TareaDTO Tarea { get; set; }
    public RecursoDTO Recurso { get; set; }
    public int Cantidad { get; set; }
}