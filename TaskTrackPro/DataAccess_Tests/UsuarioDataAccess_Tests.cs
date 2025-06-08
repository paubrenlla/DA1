using Domain;
using DataAccess;

namespace DataAccess_Tests;

[TestClass]
public class DataAccessUsuarioTest
{
    private UsuarioDataAccess usuarioRepo;

    [TestInitialize]
    public void SetUp()
    {
        usuarioRepo = new UsuarioDataAccess();
    }

    [TestMethod]
    public void AgregarUsarioComun()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        usuarioRepo.Add(user);

        Assert.AreEqual(1, usuarioRepo.GetAll().Count);

        Usuario user2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!",
            new DateTime(2000, 01, 01));

        usuarioRepo.Add(user2);
        Assert.AreEqual(2, usuarioRepo.GetAll().Count);
        Assert.AreSame(user2, usuarioRepo.GetAll()[1]);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsarioQueYaExisteEnElSistema()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        usuarioRepo.Add(user);

        Usuario user2 = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!",
            new DateTime(2000, 01, 01));

        usuarioRepo.Add(user2);
    }

    [TestMethod]
    public void EliminarUsuario()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        usuarioRepo.Add(user);

        Usuario user2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!",
            new DateTime(2000, 01, 01));

        usuarioRepo.Add(user2);
        Assert.AreEqual(2, usuarioRepo.GetAll().Count);
        Assert.AreSame(user2, usuarioRepo.GetAll()[1]);

        usuarioRepo.Remove(user2);
        Assert.AreEqual(1, usuarioRepo.GetAll().Count);
        Assert.IsFalse(usuarioRepo.GetAll().Contains(user2));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuarioQueEsAdmin()
    {
        Usuario user = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        usuarioRepo.Add(user);
        user.EsAdminSistema = true;

        usuarioRepo.Remove(user);
    }

    [TestMethod]
    public void BuscarUsuarioPorIdDevuelveUsuarioCorrecto()
    {
        Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", "123AAaa!!", new DateTime(2000, 1, 1));
        Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", "456AAaa!!", new DateTime(1999, 2, 2));
        usuarioRepo.Add(u1);
        usuarioRepo.Add(u2);

        Usuario resultado = usuarioRepo.GetById(u2.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(u2.Email, resultado.Email);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void BuscarUsuarioPorIdDevuelveNullSiNoExiste()
    {
        Usuario resultado = usuarioRepo.GetById(999);

        Assert.IsNull(resultado);
    }
    
    [TestMethod]
    public void BuscarUsuarioPorCorreoYContraseña()
    {
        string email2="b@b.com";
        string contraseña2= "456AAaa!!";
        Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", "123AAaa!!", new DateTime(2000, 1, 1));
        Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", "456AAaa!!", new DateTime(1999, 2, 2));
        usuarioRepo.Add(u1);
        usuarioRepo.Add(u2);

        Usuario resultado = usuarioRepo.buscarUsuarioPorCorreoYContraseña(email2,Usuario.EncriptarPassword(contraseña2));

        Assert.IsNotNull(resultado);
        Assert.AreEqual(u2.Email, resultado.Email);
    }
    
    [TestMethod]
    public void BuscarUsuarioPorCorreo()
    {
        string email2="b@b.com";
        Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", "123AAaa!!", new DateTime(2000, 1, 1));
        Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", "456AAaa!!", new DateTime(1999, 2, 2));
        usuarioRepo.Add(u1);
        usuarioRepo.Add(u2);

        Usuario resultado = usuarioRepo.BuscarUsuarioPorCorreo(email2);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(u2.Email, resultado.Email);
    }
}