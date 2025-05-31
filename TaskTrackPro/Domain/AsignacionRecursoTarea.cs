namespace Domain;

public class AsignacionRecursoTarea
{
    public Recurso Recurso { get; set; }
    public Tarea Tarea { get; set; }
    public int CantidadNecesaria { get; set; }

    public AsignacionRecursoTarea(Recurso recurso, Tarea tarea, int cantidadNecesaria)
    {
        if (recurso == null)
            throw new ArgumentNullException(nameof(recurso), "El recurso no puede ser nulo.");
        
        if (tarea == null)
            throw new ArgumentNullException(nameof(tarea), "La tarea no puede ser nulo.");

        if (cantidadNecesaria <= 0)
            throw new ArgumentOutOfRangeException(nameof(cantidadNecesaria));
        
        Recurso = recurso;
        Tarea = tarea;
        CantidadNecesaria = cantidadNecesaria;
    }
}