using BusinessLogic;

namespace BusinessLogic_Tests;

[TestClass]
public class DBTests
{
    [TestMethod]
    public void ConstructorSinParametros()
    {
        DB db = new DB();
        
        Assert.AreEqual(0, db.AdministradoresSistema.Count);
        Assert.AreEqual(0, db.ListaProyectos.Count);
        Assert.AreEqual(0, db.ListaRecursos.Count);
        Assert.AreEqual(0, db.ListaUsuarios.Count);
        Assert.AreEqual(0, db.ListaNotificaciones.Count);
    }
    
    [TestMethod]
    public void ConstructorConDatosPrecargados()
    {
        DB db = new DB(true);
        
        Assert.IsFalse(0 == db.AdministradoresSistema.Count);
        Assert.IsFalse(0 == db.ListaUsuarios.Count);
        Assert.IsFalse(0 == db.ListaProyectos.Count);
        Assert.IsFalse(0 == db.ListaRecursos.Count);
        Assert.IsFalse(0 == db.ListaNotificaciones.Count);
    }
    
    
    [TestMethod]
    public void ConstructorConUsarioAdmin()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        DB db = new DB(user);
        
        Assert.AreEqual(1, db.AdministradoresSistema.Count);
        Assert.AreEqual(0, db.ListaProyectos.Count);
        Assert.AreEqual(0, db.ListaRecursos.Count);
        Assert.AreEqual(1, db.ListaUsuarios.Count);
        Assert.AreSame(user, db.AdministradoresSistema[0]);
        Assert.AreSame(user, db.ListaUsuarios[0]);
    }
    
    [TestMethod]
    public void AgregarUsarioComun()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        DB db = new DB(user);
        
        Assert.AreEqual(1, db.ListaUsuarios.Count);
        
        Usuario user2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
        db.agregarUsuario(user2);
        Assert.AreEqual(2, db.ListaUsuarios.Count);
        Assert.AreSame(user2, db.ListaUsuarios[1]);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsarioQueYaExisteEnElSistema()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        DB db = new DB(user);
        
        Usuario user2 = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
        db.agregarUsuario(user2);
        db.agregarUsuario(user2);
    }
    
    [TestMethod]
    public void EliminarUsuario()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        DB db = new DB(user);
        
        Usuario user2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
        db.agregarUsuario(user2);
        Assert.AreEqual(2, db.ListaUsuarios.Count);
        Assert.AreSame(user2, db.ListaUsuarios[1]);
        
        db.eliminarUsuario(user2);
        Assert.AreEqual(1, db.ListaUsuarios.Count);
        Assert.IsFalse(db.ListaUsuarios.Contains(user2));
    }
    
    [ExpectedException(typeof(ArgumentException))]
    [TestMethod]
    public void EliminarUsuarioNoPuedeEliminarAdminDeProyecto()
    {
        DB db = new DB();
        Usuario usuario = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        db.agregarUsuario(usuario);
        Proyecto proyecto = new Proyecto("Proyecto","descripcion", DateTime.Today);
        proyecto.AsignarAdmin(usuario);
        db.agregarProyecto(proyecto);
        
        db.eliminarUsuario(usuario);
    }
    
    [TestMethod]
    public void EliminarUsuarioBorraAsignacionesDeProyectos()
    {
        DB db = new DB();
        Usuario usuario = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        Usuario usuario2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        db.agregarUsuario(usuario);
        Proyecto proyecto = new Proyecto("Proyecto","descripcion", DateTime.Today);
        db.agregarProyecto(proyecto);
        proyecto.agregarMiembro(usuario);
        proyecto.AsignarAdmin(usuario2);
        Tarea tarea = new Tarea("Tarea", "descripcion", DateTime.Today, TimeSpan.FromDays(1), false);
        tarea.AgregarUsuario(usuario);
        proyecto.agregarTarea(tarea);
        
        db.eliminarUsuario(usuario);
        
        Assert.IsTrue(tarea.UsuariosAsignados.Count == 0);
        Assert.IsTrue(proyecto.Miembros.Count == 0);
        Assert.IsFalse(tarea.UsuariosAsignados.Contains(usuario));
        Assert.IsFalse(proyecto.Miembros.Contains(usuario));
    }
    
    [TestMethod]
    public void AgregarAdmin()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        DB db = new DB(user);
        
        Usuario user2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
        db.agregarAdmin(user2);
        Assert.AreEqual(2, db.AdministradoresSistema.Count);
        Assert.AreSame(user2, db.AdministradoresSistema[1]);
        Assert.AreEqual(2, db.ListaUsuarios.Count);
        Assert.AreSame(user2, db.ListaUsuarios[1]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarAdminQueYaExisteEnElSistema()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        DB db = new DB(user);
        
        db.agregarAdmin(user);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuarioQueEsAdmin()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        DB db = new DB(user);
        
        db.eliminarUsuario(user);
    }
    
    [ExpectedException(typeof(ArgumentException))]
    [TestMethod]
    public void AgregarAdminQueYaEraUsuarioComun()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        DB db = new DB(user);
        
        Usuario user2 = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
        db.agregarUsuario(user2);
    }

    [TestMethod]
    public void AgregarNotificacion()
    {
        DB db = new DB();

        Notificacion notificacion = new Notificacion("esta es una notificacion de prueba");
        
        db.agregarNotificacion(notificacion);
        
        Assert.IsTrue(db.ListaNotificaciones.Count!=0);
        Assert.IsTrue(db.ListaNotificaciones.Contains(notificacion));
    }
    
    [TestMethod]
    public void AgregarProyecto()
    {
        DB db = new DB();
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jej";
        DateTime fechaInicio = DateTime.Today;
        

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        db.agregarProyecto(proyecto);
        Assert.AreEqual(1, db.ListaProyectos.Count);
        Assert.AreSame(proyecto, db.ListaProyectos[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarProyectoYaExistenteEnElSistema()
    {
        DB db = new DB();
        
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jej";
        DateTime fechaInicio = DateTime.Today;
        

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        db.agregarProyecto(proyecto);
        db.agregarProyecto(proyecto);
    }
    
    [TestMethod]
    public void EliminarProyecto()
    {
        DB db = new DB();
        
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jej";
        DateTime fechaInicio = DateTime.Today;
        

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        db.agregarProyecto(proyecto);
        
        Assert.AreEqual(1, db.ListaProyectos.Count);
        Assert.AreSame(proyecto, db.ListaProyectos[0]);
        
        db.eliminarProyecto(proyecto);
        Assert.AreEqual(0, db.ListaProyectos.Count);
        Assert.IsFalse(db.ListaProyectos.Contains(proyecto));
    }
    
    [TestMethod]
    public void AgregarRecurso()
    {
        DB db = new DB();
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion, false, 5);
        db.agregarRecurso(recurso);
        Assert.AreEqual(1, db.ListaRecursos.Count);
        Assert.AreSame(recurso, db.ListaRecursos[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarRecursoYaExistenteEnElSistema()
    {
        DB db = new DB();
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion, false, 2);
        db.agregarRecurso(recurso);
        db.agregarRecurso(recurso);
    }
    
    [TestMethod]
    public void EliminarRecurso()
    {
        DB db = new DB();
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion, false, 2);
        db.agregarRecurso(recurso);
        Assert.AreEqual(1, db.ListaRecursos.Count);
        Assert.AreSame(recurso, db.ListaRecursos[0]);
        
        db.eliminarRecurso(recurso);
        Assert.AreEqual(0, db.ListaRecursos.Count);
        Assert.IsFalse(db.ListaRecursos.Contains(recurso));
    }
    
    [TestMethod]
    public void BuscarUsuarioPorIdDevuelveUsuarioCorrecto()
    {
        DB db = new DB();
        Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", "123AAaa!!", new DateTime(2000, 1, 1));
        Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", "456AAaa!!", new DateTime(1999, 2, 2));
        db.agregarUsuario(u1);
        db.agregarUsuario(u2);

        Usuario resultado = db.buscarUsuarioPorId(u2.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(u2.Email, resultado.Email);
    }
    
    [TestMethod]
    public void BuscarUsuarioPorIdDevuelveNullSiNoExiste()
    {
        DB db = new DB();

        Usuario resultado = db.buscarUsuarioPorId(999);

        Assert.IsNull(resultado);
    }
    
    [TestMethod]
    public void BuscarUsuarioPorCorreoYContraseña()
    {
        DB db = new DB();
        string email2="b@b.com";
        string contraseña2= "456AAaa!!";
        Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", "123AAaa!!", new DateTime(2000, 1, 1));
        Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", "456AAaa!!", new DateTime(1999, 2, 2));
        db.agregarUsuario(u1);
        db.agregarUsuario(u2);

        Usuario resultado = db.buscarUsuarioPorCorreoYContraseña(email2,contraseña2);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(u2.Email, resultado.Email);
    }
    
    [TestMethod]
    public void BuscarUsuarioPorCorreo()
    {
        DB db = new DB();
        string email2="b@b.com";
        Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", "123AAaa!!", new DateTime(2000, 1, 1));
        Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", "456AAaa!!", new DateTime(1999, 2, 2));
        db.agregarUsuario(u1);
        db.agregarUsuario(u2);

        Usuario resultado = db.buscarUsuarioPorCorreo(email2);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(u2.Email, resultado.Email);
    }
    
    [ExpectedException(typeof(ArgumentException))]
    [TestMethod]
    public void NoPuedenHaberCorreosRepetidos()
    {
        DB db = new DB();
        Usuario usuario1 = new Usuario("correo@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Usuario usuario2 = new Usuario("correo@gmail.com", "Nombre2", "Apellido2", "EsValida1!", new DateTime(2000, 1, 1));
        db.agregarUsuario(usuario1);
        db.agregarUsuario(usuario2);
    }
    
    [TestMethod]
    public void BuscarProyectoPorIdDevuelveProyectoCorrecto()
    { 
        DB db = new DB();
        Proyecto p1= new Proyecto("Proyecto 1", "desc", DateTime.Today);
        Proyecto p2= new Proyecto("Proyecto 1", "desc", DateTime.Today);
        
        db.agregarProyecto(p1);
        db.agregarProyecto(p2);
        
        Proyecto resultado = db.buscarProyectoPorId(p2.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(p2.Id, resultado.Id);
        Assert.AreEqual(p2.Nombre, resultado.Nombre);
    }

    [TestMethod]
    public void BuscarRecursoPorIdDevuelveUsuarioCorrecto()
    {
        DB db = new DB();
        Recurso r1 = new Recurso("Auto", "Rojo", "Ferrari", false, 1);
        Recurso r2 = new Recurso("Computadora", "Equipos", "Lenovo", true, 5);
        db.agregarRecurso(r1);
        db.agregarRecurso(r2);

        Recurso resultado = db.buscarRecursoPorId(r2.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(r2.Nombre, resultado.Nombre);
    }
    
    [TestMethod]
    public void BuscarRecursoPorIdDevuelveNullSiNoExiste()
    {
        DB db = new DB();

        Recurso resultado = db.buscarRecursoPorId(999);

        Assert.IsNull(resultado);
    }
    
    [TestMethod]
    public void BuscarNotificacionPorIdDevuelveNotificacionCorrecta()
    {
        DB db = new DB();

        Notificacion notificacion = new Notificacion("Notificación de prueba");
        Notificacion notificacion2 = new Notificacion("Notificación de prueba");
        
        db.agregarNotificacion(notificacion);
        db.agregarNotificacion(notificacion2);

        Notificacion resultado= db.buscarNotificaciónPorId(notificacion.Id);
        
        Assert.AreEqual(resultado, notificacion);
    }

    [TestMethod]
    public void DevolverNotificacionesNoLeidasDeUnUsuario()
    {
        DB db = new DB();
        Notificacion notificacion = new Notificacion("Notificación de prueba");
        Notificacion notificacion2 = new Notificacion("Notificación de prueba");
        Usuario usuario1 = new Usuario("correo@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));

        db.agregarNotificacion(notificacion);
        db.agregarNotificacion(notificacion2);
        db.agregarUsuario(usuario1);
        notificacion.AgregarUsuario(usuario1);
        notificacion2.AgregarUsuario(usuario1);
        notificacion.MarcarComoVista(usuario1);
        
        List<Notificacion> noLeidas = db.NotificacionesNoLeidas(usuario1);
        
        Assert.IsNotNull(noLeidas);
        Assert.AreEqual(notificacion2.Id, noLeidas[0].Id);
    }


    [TestMethod]
    public void VerSiUnUsuarioEsAdmin()
    {
        Usuario usuario1 = new Usuario("correo@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        DB db = new DB(usuario1);
        bool esAdmin = db.UsuarioEsAdmin(usuario1);
        Assert.IsTrue(esAdmin);
    }
    
    [TestMethod]
    public void DevolverProyectosDeUnUsuario()
    {
        DB db = new DB();
        Usuario usuario1 = new Usuario("correo@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Usuario usuario2 = new Usuario("correo2@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Proyecto p1= new Proyecto("Proyecto 1", "desc", DateTime.Today);
        Proyecto p2= new Proyecto("Proyecto 2", "desc", DateTime.Today);
        Proyecto p3= new Proyecto("Proyecto 3", "desc", DateTime.Today);
        p1.AsignarAdmin(usuario1);
        p2.AsignarAdmin(usuario2);
        p3.AsignarAdmin(usuario2);
        p2.agregarMiembro(usuario1);
        db.agregarUsuario(usuario1);
        db.agregarUsuario(usuario2);
        db.agregarProyecto(p1);
        db.agregarProyecto(p2);
        db.agregarProyecto(p3);
        
        List<Proyecto> proyectosDelUsuario = db.ProyectosDelUsuario(usuario1);
        
        Assert.IsTrue(proyectosDelUsuario.Count == 2);
        Assert.IsTrue(proyectosDelUsuario.Contains(p1));
        Assert.IsTrue(proyectosDelUsuario.Contains(p2));
        Assert.IsFalse(proyectosDelUsuario.Contains(p3));
    }
    
    [TestMethod]
    public void VerSiUsuarioEsAdminDeUnProyecto()
    {
        DB db = new DB();
        Usuario usuario1 = new Usuario("correo@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Proyecto p1= new Proyecto("Proyecto 1", "desc", DateTime.Today);
        p1.AsignarAdmin(usuario1);
        db.agregarProyecto(p1);
        
       bool esAdmin = db.UsuarioEsAdminProyecto(usuario1, p1);
        
        Assert.IsTrue(esAdmin);
    }
    
}