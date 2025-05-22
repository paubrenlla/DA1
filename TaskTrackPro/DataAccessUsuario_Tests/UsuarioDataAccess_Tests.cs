using BusinessLogic;
using Repositorios;

namespace DataAccessUsuario_Tests;

[TestClass]
public class DataAccessUsuarioTest
{
    private UsuarioDataAccess UsuarioRepo;
    private Usuario usuario;

    [TestInitialize]
    public void SetUp()
    {
        UsuarioRepo = new UsuarioDataAccess();
    }
    
     [TestMethod]
    public void AgregarUsarioComun()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        UsuarioRepo.Add(user);
        
        Assert.AreEqual(1, UsuarioRepo.GetAll().Count);
        
        Usuario user2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
        UsuarioRepo.Add(user2);
        Assert.AreEqual(2, UsuarioRepo.GetAll().Count);
        Assert.AreSame(user2, UsuarioRepo.GetAll()[1]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsarioQueYaExisteEnElSistema()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        UsuarioRepo.Add(user);
        
        Usuario user2 = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
        UsuarioRepo.Add(user2);
    }
    
    [TestMethod]
    public void EliminarUsuario()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        UsuarioRepo.Add(user);
        
        Usuario user2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        
        UsuarioRepo.Add(user2);
        Assert.AreEqual(2, UsuarioRepo.GetAll().Count);
        Assert.AreSame(user2, UsuarioRepo.GetAll()[1]);
        
        UsuarioRepo.Remove(user2);
        Assert.AreEqual(1, UsuarioRepo.GetAll().Count);
        Assert.IsFalse(UsuarioRepo.GetAll().Contains(user2));
    }
    
    [ExpectedException(typeof(ArgumentException))]
    [TestMethod]
    //TODO Moverlo a Usuario Service
    public void EliminarUsuarioNoPuedeEliminarAdminDeProyecto()
    {
        Usuario usuario = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        UsuarioRepo.Add(usuario);
        Proyecto proyecto = new Proyecto("Proyecto","descripcion", DateTime.Today);
        proyecto.AsignarAdmin(usuario);
       // db.agregarProyecto(proyecto);
        
        //db.eliminarUsuario(usuario);
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuarioQueEsAdmin()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        UsuarioRepo.Add(user);
        user.EsAdminSistema = true;
        
        UsuarioRepo.Remove(user);
    }
    [TestMethod] //TODO Moverlo a UsuarioLogic
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
    
}