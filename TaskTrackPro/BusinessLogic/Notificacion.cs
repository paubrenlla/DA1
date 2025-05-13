public class Notificacion
{
    private string _mensaje;

    public string Mensaje
    {
        get => _mensaje;
        set => _mensaje = value;
    }
    public Notificacion(string mensaje)
    {
        Mensaje = mensaje;
    }
    
}