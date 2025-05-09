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
    }
    
    [TestMethod]
    public void ConstructorConDatosPrecargados()
    {
        DB db = new DB(true);
        
        Assert.IsFalse(0 == db.AdministradoresSistema.Count);
        Assert.IsFalse(0 == db.ListaUsuarios.Count);
        Assert.IsFalse(0 == db.ListaProyectos.Count);
        Assert.IsFalse(0 == db.ListaRecursos.Count);
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
        
        Usuario user2 = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
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
        
        Usuario user2 = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
        db.agregarUsuario(user2);
        Assert.AreEqual(2, db.ListaUsuarios.Count);
        Assert.AreSame(user2, db.ListaUsuarios[1]);
        
        db.eliminarUsuario(user2);
        Assert.AreEqual(1, db.ListaUsuarios.Count);
        Assert.IsFalse(db.ListaUsuarios.Contains(user2));
    }
    
    [TestMethod]
    public void AgregarAdmin()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        DB db = new DB(user);
        
        Usuario user2 = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
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
    
    [TestMethod]
    public void AgregarAdminQueYaEraUsuarioComun()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        DB db = new DB(user);
        
        Usuario user2 = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
        db.agregarUsuario(user2);
        db.agregarAdmin(user2);
        Assert.AreEqual(2, db.AdministradoresSistema.Count);
        Assert.AreSame(user2, db.AdministradoresSistema[1]);
        Assert.AreEqual(2, db.ListaUsuarios.Count);
        Assert.AreSame(user2, db.ListaUsuarios[1]);
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
}