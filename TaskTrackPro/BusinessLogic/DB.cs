namespace BusinessLogic;

public class DB
{
    public List<Usuario> AdministradoresSistema { get; set; }
    public List<Proyecto> ListaProyectos { get; set; }
    public List<Recurso> ListaRecursos { get; set; }
    public List<Usuario> ListaUsuarios { get; set; }
    public List<Notificacion> ListaNotificaciones { get; set; }

    public DB()
    {
        AdministradoresSistema = new List<Usuario>();
        ListaProyectos = new List<Proyecto>();
        ListaRecursos = new List<Recurso>();
        ListaUsuarios = new List<Usuario>();
        ListaNotificaciones = new List<Notificacion>();
    }
    
    public DB(bool precargarDatos) : this()
    {
        Usuario usuarioAdmin = new Usuario("admin@admin.com", "Administrador",  "Administrador", "Administrador1!", new DateTime(2000, 3, 17));
        Usuario usuario1 = new Usuario("mateomcelano@gmail.com", "Mateo", "Muñiz", "Contraseña1!", new DateTime(2002, 4, 24));
        Usuario usuario2 = new Usuario("bruno@gmail.com", "Bruno", "Fernández", "ClaveSegura2#", new DateTime(1988, 9, 30));
        Usuario usuario3 = new Usuario("fgavello@gmail.com", "Bruno", "Fernández", "Fgavello.2025!", new DateTime(1988, 9, 30));
        Usuario usuario4 = new Usuario("pb@paumail.com", "Paula", "Brenlla", "Contraseña1!", new DateTime(2004, 2, 24));

        ListaUsuarios.Add(usuarioAdmin);
        ListaUsuarios.Add(usuario1);
        ListaUsuarios.Add(usuario2);
        ListaUsuarios.Add(usuario3);
        ListaUsuarios.Add(usuario4);
        
        AdministradoresSistema.Add(usuarioAdmin);
        AdministradoresSistema.Add(usuario1);
        
        Proyecto proyecto1 = new Proyecto("Proyecto prueba1", "Este es un proyecto de prueba1", DateTime.Now);
        Proyecto proyecto2 = new Proyecto("Proyecto prueba2", "Este es un proyecto de prueba2", DateTime.Now);
        Proyecto proyecto3 = new Proyecto("Proyecto prueba3", "Este es un proyecto de prueba3", DateTime.Now);
        
        ListaProyectos.Add(proyecto1);
        ListaProyectos.Add(proyecto2);
        ListaProyectos.Add(proyecto3);
        
        proyecto1.AsignarAdmin(usuario1);
        proyecto2.AsignarAdmin(usuario1);
        proyecto3.AsignarAdmin(usuario3);

        proyecto1.agregarMiembro(usuario1);
        proyecto1.agregarMiembro(usuario2);
        proyecto1.agregarMiembro(usuario3);

        proyecto2.agregarMiembro(usuario1);
        proyecto2.agregarMiembro(usuario3);
        proyecto2.agregarMiembro(usuario4);

        proyecto3.agregarMiembro(usuario1);
        proyecto3.agregarMiembro(usuario2);
        
        Tarea tarea1 = new Tarea("Planificación inicial", "Definir metas y responsables", new DateTime(2025, 5, 14, 9, 0, 0), TimeSpan.FromHours(2), true);
        Tarea tarea2 = new Tarea("Revisión de requisitos", "Revisión con el cliente", new DateTime(2025, 5, 15, 10, 0, 0), TimeSpan.FromHours(3), false);
        proyecto1.agregarTarea(tarea1);
        proyecto1.agregarTarea(tarea2);

        Tarea tarea3 = new Tarea("Diseño de arquitectura", "Definir la estructura general del sistema", new DateTime(2025, 5, 16, 9, 0, 0), TimeSpan.FromHours(4), true);
        Tarea tarea4 = new Tarea("Prueba de conceptos", "Prototipo funcional", new DateTime(2025, 5, 17, 14, 0, 0), TimeSpan.FromHours(2), false);
        Tarea tarea5 = new Tarea("Desarrollo base de datos", "Diseño e implementación del modelo", new DateTime(2025, 5, 18, 9, 0, 0), TimeSpan.FromHours(5), true);
        proyecto2.agregarTarea(tarea3);
        proyecto2.agregarTarea(tarea4);
        proyecto2.agregarTarea(tarea5);

        Tarea tarea6 = new Tarea("Testing de integración", "Pruebas de módulos integrados", new DateTime(2025, 5, 20, 10, 0, 0), TimeSpan.FromHours(3), false);
        Tarea tarea7 = new Tarea("Documentación", "Generar documentación para el usuario final", new DateTime(2025, 5, 21, 11, 0, 0), TimeSpan.FromHours(2), false);
        proyecto3.agregarTarea(tarea6);
        proyecto3.agregarTarea(tarea7);

        proyecto3.AsignarUsuarioATarea(usuario1, tarea6);
        proyecto3.AsignarUsuarioATarea(usuario2, tarea7);

        proyecto2.AsignarUsuarioATarea(usuario1, tarea3);
        proyecto2.AsignarUsuarioATarea(usuario3, tarea4);
        proyecto2.AsignarUsuarioATarea(usuario4, tarea5);

        proyecto1.AsignarUsuarioATarea(usuario1, tarea1);
        proyecto1.AsignarUsuarioATarea(usuario2, tarea1);
        proyecto1.AsignarUsuarioATarea(usuario2, tarea2);
        proyecto1.AsignarUsuarioATarea(usuario3, tarea2);
        
        Recurso recurso1 = new Recurso("Auto", "Vehiculo","El auto de la empresa", false, 1, proyecto1);
        Recurso recurso2 = new Recurso("Desarrollador backend", "Empleado", "Desarrollador con preferencia backend", true, 3);
        Recurso recurso3 = new Recurso("Desarrollador frontend", "Empleado",  "Desarrollador con preferencia frontend", true, 3);
        Recurso recurso4 = new Recurso("UX/UI Designer", "Empleado",  "Diseñador", true, 2);
        Recurso recurso5 = new Recurso("Computadora", "Materiales", "Una por empleado maximo", false, 10);
        
        ListaRecursos.Add(recurso1);
        ListaRecursos.Add(recurso2);
        ListaRecursos.Add(recurso3);
        ListaRecursos.Add(recurso4);
        ListaRecursos.Add(recurso5);
        
        proyecto1.agregarRecurso(recurso2);
        proyecto1.agregarRecurso(recurso4);

        proyecto2.agregarRecurso(recurso1);
        proyecto2.agregarRecurso(recurso2);
        proyecto2.agregarRecurso(recurso5); 

        proyecto3.agregarRecurso(recurso3);
        proyecto3.agregarRecurso(recurso4); 
        proyecto3.agregarRecurso(recurso5);
        
        tarea1.AgregarRecurso(recurso2, 2);
        tarea2.AgregarRecurso(recurso4, 1); 

        tarea3.AgregarRecurso(recurso2, 2);
        tarea4.AgregarRecurso(recurso1, 1);
        tarea5.AgregarRecurso(recurso2, 1);
        tarea5.AgregarRecurso(recurso5, 2); 

        tarea6.AgregarRecurso(recurso3, 2); 
        tarea7.AgregarRecurso(recurso1, 1);
        tarea7.AgregarRecurso(recurso5, 1);
        
        tarea1.AgregarDependencia(tarea2);
        tarea4.AgregarDependencia(tarea3);
        tarea4.AgregarDependencia(tarea5);
    }
    
    public DB(Usuario user) : this()
    {
        AdministradoresSistema.Add(user);
        ListaUsuarios.Add(user);
    }
    
    public void agregarUsuario(Usuario user)
    {        
        if (ListaUsuarios.Contains(user) || ExisteUsuarioConCorreo(user))
            throw new ArgumentException("Usuario ya existe");
        ListaUsuarios.Add(user);
    }

    private bool ExisteUsuarioConCorreo(Usuario user)
    {
        return ListaUsuarios.Any(u => u.Email == user.Email);
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
    public Usuario? buscarUsuarioPorCorreo(string email)
    {
        return ListaUsuarios.FirstOrDefault(u => u.Email == email);
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


    public Proyecto buscarProyectoPorId(int id)
    {
        return ListaProyectos.FirstOrDefault(p => p.Id == id);
    }
  
    public Recurso? buscarRecursoPorId(int id)
    {
        return ListaRecursos.FirstOrDefault(r => r.Id == id);
    }
}