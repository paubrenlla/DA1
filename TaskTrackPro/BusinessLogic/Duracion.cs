namespace BusinessLogic;

public class Duracion
{
    private int Cantidad { get; set; }
    private TipoDuracion Tipo { get; set; }

    public Duracion(int cantidad, TipoDuracion tipo)
    {
        Cantidad = cantidad;
        Tipo = tipo;
    }
}
