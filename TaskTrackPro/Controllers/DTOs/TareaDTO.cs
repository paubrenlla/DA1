namespace Controllers.DTOs;

public class TareaDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public DateTime FechaInicio { get; set; }
    public TimeSpan Duracion { get; set; }
    public bool EsCritica { get; set; }
    public string Estado { get; set; }
}