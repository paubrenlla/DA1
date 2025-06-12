namespace Domain;

public class AsignacionRecursoTarea
{
    private static int _contadorId = 1;
    public Recurso Recurso { get; set; }
    public Tarea Tarea { get; set; }
    public int CantidadNecesaria { get; set; }
    public int Id { get; }
    
    public AsignacionRecursoTarea() { }

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
        Id = _contadorId++;
    }

    public void Modificar(int cantidadNueva)
    {
        if (cantidadNueva <= 0)
            throw new ArgumentOutOfRangeException(nameof(cantidadNueva));
        
        if (cantidadNueva > Recurso.CantidadDelRecurso)
            throw new ArgumentOutOfRangeException(nameof(cantidadNueva), "No hay suficientes recursos");
        
        CantidadNecesaria = cantidadNueva;
    }
}