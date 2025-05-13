using BusinessLogic;

public class Notificacion
{
    private string _mensaje;
    private List<Usuario> _usuariosNotificados;

    public string Mensaje
    {
        get => _mensaje;
        set => _mensaje = value;
    }
    public Notificacion(string mensaje)
    {
        Mensaje = mensaje;
        UsuariosNotificados = new List<Usuario>();
    }
    
    public List<Usuario> UsuariosNotificados
    {
        get => _usuariosNotificados;
        set => _usuariosNotificados = value;
    }

    public void AgregarUsuario(Usuario usuario)
    {
        UsuariosNotificados.Add(usuario);
    }
}