namespace BusinessLogic;

public class Estado
{
    private TipoEstadoTarea _valor;
    private DateTime? _fecha;

    public TipoEstadoTarea Valor
    {
        get { return _valor; }
    }

    public DateTime? Fecha
    {
        get { return _fecha; }
    }

    // Constructor de Estado
    public Estado(TipoEstadoTarea valor)
    {
        _valor = valor;
        _fecha = valor == TipoEstadoTarea.Efectuada ? DateTime.Now : null;
    }

}
