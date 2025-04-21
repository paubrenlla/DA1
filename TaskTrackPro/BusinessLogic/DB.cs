namespace BusinessLogic;

public class DB
{
    public List<Usuario> AdministradoresSistema { get; set; }
    public List<Proyecto> ListaProyectos { get; set; }
    public List<Recurso> ListaRecursos { get; set; }
    public List<Usuario> ListaUsuarios { get; set; }

    public DB()
    {
        AdministradoresSistema = new List<Usuario>();
        ListaProyectos = new List<Proyecto>();
        ListaRecursos = new List<Recurso>();
        ListaUsuarios = new List<Usuario>();
    }

    public DB(Usuario user) : this()
    {
        AdministradoresSistema.Add(user);
        ListaUsuarios.Add(user);
    }
    
    public void agregarUsuario(Usuario user)
    {
        ListaUsuarios.Add(user);
    }
    
    public void agregarAdmin(Usuario user)
    {
        agregarUsuario(user);
        AdministradoresSistema.Add(user);
    }
}