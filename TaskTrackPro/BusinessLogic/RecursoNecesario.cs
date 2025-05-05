namespace BusinessLogic;

public class RecursoNecesario
{
    public Recurso Recurso { get; set; }
    public int CantidadNecesaria { get; set; }

    public RecursoNecesario(Recurso recurso, int cantidadNecesaria)
    {
        if (recurso == null)
            throw new ArgumentNullException(nameof(recurso), "El recurso no puede ser nulo.");

        if (cantidadNecesaria <= 0)
            throw new ArgumentOutOfRangeException(nameof(cantidadNecesaria));
        
        Recurso = recurso;
        CantidadNecesaria = cantidadNecesaria;
    }
}