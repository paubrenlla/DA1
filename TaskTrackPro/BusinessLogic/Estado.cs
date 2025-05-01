namespace BusinessLogic;

public class Estado
{
    private TipoEstadoTarea _valor;
    private DateTime? _fecha;
    
    public TipoEstadoTarea Valor
    {
        get => _valor;
        set => _valor = value;
    }

    public DateTime? Fecha
    {
        get => _fecha;
        set => _fecha = value;
    }
    
    
    
    public Estado(TipoEstadoTarea valor)
    {
        Valor = valor;
        Fecha = valor == TipoEstadoTarea.Efectuada ? DateTime.Now : null;
    }

    public void  MarcarComoEfectuada(DateTime fecha)
    {
        Valor = TipoEstadoTarea.Efectuada;
        Fecha = fecha;
    }

    public override string ToString()
    {
        if (Valor == TipoEstadoTarea.Efectuada)
        {
            return $"Estado: {Valor}, Fecha: {Fecha?.ToString("dd/MM/yyyy")}";
        }
        else 
        {
            return $"Estado: {Valor}";
        }
    }
}
