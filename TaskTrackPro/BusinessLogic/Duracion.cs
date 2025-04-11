namespace BusinessLogic;

public class Duracion
{
    private int Cantidad { get; set; }
    private TipoDuracion Tipo { get; set; }

    public Duracion(int cantidad, TipoDuracion tipo)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser un número positivo mayor que cero.", nameof(cantidad));
        
        if (!Enum.IsDefined(typeof(TipoDuracion), tipo))
            throw new ArgumentException("El tipo de duración no es válido.", nameof(tipo));
        
        Cantidad = cantidad;
        Tipo = tipo;
    }
}
