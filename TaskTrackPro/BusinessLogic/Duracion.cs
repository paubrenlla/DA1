namespace BusinessLogic;

public class Duracion
{
    private int Cantidad { get; set; }
    private TipoDuracion Tipo { get; set; }

    public Duracion(int cantidad, TipoDuracion tipo)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser un número positivo mayor que cero.", nameof(cantidad));
        Cantidad = cantidad;
        Tipo = tipo;
    }
}
