namespace BusinessLogic;

public class Duracion
{
    private int _cantidad;
    private TipoDuracion _tipo;

    public Duracion(int cantidad, TipoDuracion tipo)
    {
        Cantidad = cantidad;
        Tipo = tipo;
    }

    public int Cantidad
    {
        get => _cantidad;
        set
        {
            if (value <= 0)
                throw new ArgumentException("La cantidad debe ser un número positivo mayor que cero.", nameof(value));
            _cantidad = value;
        }
    }

    public TipoDuracion Tipo
    {
        get => _tipo;
        set
        {
            if (!Enum.IsDefined(typeof(TipoDuracion), value))
                throw new ArgumentException("El tipo de duración no es válido.", nameof(value));
            _tipo = value;
        }
    }
}
