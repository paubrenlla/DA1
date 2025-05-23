using BusinessLogic;
using Repositorios;

namespace DataAccess_Tests;

[TestClass]
public class DataAccessUsuarioTest
{
    private UsuarioDataAccess UsuarioRepo;

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

        Usuario user2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!",
            new DateTime(2000, 01, 01));

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

        Usuario user2 = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!",
            new DateTime(2000, 01, 01));

        UsuarioRepo.Add(user2);
    }

    [TestMethod]
    public void EliminarUsuario()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        UsuarioRepo.Add(user);

        Usuario user2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!",
            new DateTime(2000, 01, 01));

        UsuarioRepo.Add(user2);
        Assert.AreEqual(2, UsuarioRepo.GetAll().Count);
        Assert.AreSame(user2, UsuarioRepo.GetAll()[1]);

        UsuarioRepo.Remove(user2);
        Assert.AreEqual(1, UsuarioRepo.GetAll().Count);
        Assert.IsFalse(UsuarioRepo.GetAll().Contains(user2));
    }

    /*[ExpectedException(typeof(ArgumentException))]
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
    }*/


    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuarioQueEsAdmin()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        UsuarioRepo.Add(user);
        user.EsAdminSistema = true;

        UsuarioRepo.Remove(user);
    }
    /*[TestMethod] //TODO Moverlo a UsuarioLogic
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
    }*/

    [TestMethod]
    public void BuscarUsuarioPorIdDevuelveUsuarioCorrecto()
    {
        DB db = new DB();
        Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", "123AAaa!!", new DateTime(2000, 1, 1));
        Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", "456AAaa!!", new DateTime(1999, 2, 2));
        UsuarioRepo.Add(u1);
        UsuarioRepo.Add(u2);

        Usuario resultado = UsuarioRepo.GetById(u2.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(u2.Email, resultado.Email);
    }
    
    [TestMethod]
    public void BuscarUsuarioPorIdDevuelveNullSiNoExiste()
    {
        Usuario resultado = UsuarioRepo.GetById(999);

        Assert.IsNull(resultado);
    }
    
    [TestMethod]
    public void BuscarUsuarioPorCorreoYContraseña()
    {
        string email2="b@b.com";
        string contraseña2= "456AAaa!!";
        Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", "123AAaa!!", new DateTime(2000, 1, 1));
        Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", "456AAaa!!", new DateTime(1999, 2, 2));
        UsuarioRepo.Add(u1);
        UsuarioRepo.Add(u2);

        Usuario resultado = UsuarioRepo.buscarUsuarioPorCorreoYContraseña(email2,contraseña2);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(u2.Email, resultado.Email);
    }
    
    [TestMethod]
    public void BuscarUsuarioPorCorreo()
    {
        string email2="b@b.com";
        Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", "123AAaa!!", new DateTime(2000, 1, 1));
        Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", "456AAaa!!", new DateTime(1999, 2, 2));
        UsuarioRepo.Add(u1);
        UsuarioRepo.Add(u2);

        Usuario resultado = UsuarioRepo.BuscarUsuarioPorCorreo(email2);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(u2.Email, resultado.Email);
    }
}