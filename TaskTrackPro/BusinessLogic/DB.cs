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

    public DB(bool precargarDatos) : this()
    {
        Usuario usuario1 = new Usuario("mateomcelano@gmail.com", "Mateo", "Muñiz", "Contraseña1!", new DateTime(2002, 4, 24));
        Usuario usuario2 = new Usuario("bruno@gmail.com", "Bruno", "Fernández", "ClaveSegura2#", new DateTime(1988, 9, 30));
        ListaUsuarios.Add(usuario1);
        AdministradoresSistema.Add(usuario1);
        ListaUsuarios.Add(usuario2);
        
        Proyecto proyecto1 = new Proyecto("Proyecto prueba", "Este es un proyecto de prueba", DateTime.Now);
        ListaProyectos.Add(proyecto1);

        Recurso recurso1 = new Recurso("Auto", "Vehiculo","El auto de la empresa", false, 1, proyecto1);
        ListaRecursos.Add(recurso1);
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
    
    public Usuario? buscarUsuarioPorId(int id)
    {
        return ListaUsuarios.FirstOrDefault(u => u.Id == id);
    }
     public Usuario? buscarUsuarioPorCorreoYContraseña(string email, string contraseña)
    {
        return ListaUsuarios.FirstOrDefault(u =>
            u.Email == email && u.Pwd == Usuario.EncriptarPassword(contraseña));
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