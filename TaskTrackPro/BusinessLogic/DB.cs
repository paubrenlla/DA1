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
        if (ListaUsuarios.Contains(user))
            throw new ArgumentException("Usuario ya existe");
        ListaUsuarios.Add(user);
    }
    
    public void agregarAdmin(Usuario user)
    {
        if (!ListaUsuarios.Contains(user))
            agregarUsuario(user);
        if (AdministradoresSistema.Contains(user))
            throw new ArgumentException("El usuario ya es administrador");
        AdministradoresSistema.Add(user);
    }

    public void eliminarUsuario(Usuario user)
    {
        if (AdministradoresSistema.Contains(user))
            throw new ArgumentException("El usuario es administrador");
        ListaUsuarios.Remove(user);
    }

    public void agregarProyecto(Proyecto proyecto)
    {
        if (ListaProyectos.Contains(proyecto))
            throw new ArgumentException("El proyecto ya existe");
        ListaProyectos.Add(proyecto);
    }

    public void eliminarProyecto(Proyecto proyecto)
    {
        ListaProyectos.Remove(proyecto);
    }

    public void agregarRecurso(Recurso recurso)
    {
        if (ListaRecursos.Contains(recurso))
            throw new ArgumentException("Este recurso ya existe"); 
        ListaRecursos.Add(recurso);
    }

    public void eliminarRecurso(Recurso recurso)
    {
        ListaRecursos.Remove(recurso);
    }
}