namespace BusinessLogic;

public class RecursoNecesario
{
    public Recurso Recurso { get; set; }
    public int CantidadNecesaria { get; set; }

    public RecursoNecesario(Recurso recurso, int cantidadNecesaria)
    {
        Recurso = recurso;
        CantidadNecesaria = cantidadNecesaria;
    }
}