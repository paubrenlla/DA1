using Domain;
using IDataAcces;
using DataAccess;
using Controllers;
using DTOs;

namespace Controllers_Tests;

[TestClass]
public class UsuarioControllerTests
{
    private UsuarioController _usuarioController;
    private IDataAccessUsuario _repoUsuarios;
    private IDataAccessProyecto _repoProyectos;

    private Usuario _usuarioEjemplo;

    [TestInitialize]
    public void SetUp()
    {
        _repoUsuarios = new UsuarioDataAccess();
        _repoProyectos = new ProyectoDataAccess();

        _usuarioController = new UsuarioController(_repoUsuarios, _repoProyectos);

        _usuarioEjemplo = new Usuario("juan@mail.com", "Juan", "Pérez", "Contraseña1!", new DateTime(2000, 1, 1));
        _repoUsuarios.Add(_usuarioEjemplo);
    }

    [TestMethod]
    public void BuscarUsuarioPorIdDevuelveDTOCorrecto()
    {
        UsuarioDTO resultado = _usuarioController.BuscarUsuarioPorId(_usuarioEjemplo.Id);

        Assert.AreEqual(_usuarioEjemplo.Id, resultado.Id);
        Assert.AreEqual(_usuarioEjemplo.Email, resultado.Email);
        Assert.AreEqual(_usuarioEjemplo.Nombre, resultado.Nombre);
        Assert.AreEqual(_usuarioEjemplo.Apellido, resultado.Apellido);
    }
    
    [TestMethod]
    public void AgregarUsuarioValidoCorrectamente()
    {
        var dto = new UsuarioDTO
        {
            Email = "nuevo@mail.com",
            Nombre = "Nuevo",
            Apellido = "Usuario",
            Contraseña = "Contraseña1!",
            FechaNacimiento = new DateTime(1990, 1, 1)
        };

        _usuarioController.AgregarUsuario(dto);

        UsuarioDTO resultado = _usuarioController.BuscarUsuarioPorCorreo(dto.Email);

        Assert.AreEqual(dto.Email, resultado.Email);
        Assert.AreEqual(dto.Nombre, resultado.Nombre);
        Assert.AreEqual(dto.Apellido, resultado.Apellido);
        Assert.AreEqual(dto.FechaNacimiento, resultado.FechaNacimiento);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuarioConCorreoQueYaExiste()
    {
        var dto = new UsuarioDTO
        {
            Email = "juan@mail.com",
            Nombre = "Pepe",
            Apellido = "Jose",
            Contraseña = "Contraseña1!",
            FechaNacimiento = new DateTime(1995, 5, 5)
        };
        _usuarioController.AgregarUsuario(dto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuarioSinCorreo()
    {
        var dto = new UsuarioDTO
        {
            Email = "",
            Nombre = "Pepe",
            Apellido = "Jose",
            Contraseña = "Contraseña1!",
            FechaNacimiento = new DateTime(1995, 5, 5)
        };
        _usuarioController.AgregarUsuario(dto);
    }
    
    [TestMethod]
    public void EliminarUsuarioExistenteEliminaCorrectamente()
    {
        _usuarioController.EliminarUsuario(Convertidor.AUsuarioDTO(_usuarioEjemplo));

        var resultado = _repoUsuarios.BuscarUsuarioPorCorreo(_usuarioEjemplo.Email);
        Assert.IsNull(resultado);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuarioQueEsAdminSistema()
    {
        _usuarioEjemplo.EsAdminSistema = true;
        var dto = Convertidor.AUsuarioDTO(_usuarioEjemplo);
        _usuarioController.EliminarUsuario(dto);
    }

    // [TestMethod]
    // [ExpectedException(typeof(ArgumentException))]
    // public void EliminarUsuarioQueEsAdminDeUnProyecto()
    // {
    //     Proyecto proyecto = new Proyecto("Proyecto de Prueba", "Desc", DateTime.Now );
    //     proyecto.AsignarAdmin(_usuarioEjemplo);
    //     _repoProyectos.Add(proyecto);
    //     var dto = Convertidor.AUsuarioDTO(_usuarioEjemplo);
    //     _usuarioController.EliminarUsuario(dto);
    // }

    [TestMethod]
    public void BuscarUsuarioPorCorreoYContraseña()
    {
        string email = _usuarioEjemplo.Email;
        string contraseña = "Contraseña1!";

        UsuarioDTO resultado = _usuarioController.BuscarUsuarioPorCorreoYContraseña(email, contraseña);

        Assert.AreEqual(_usuarioEjemplo.Id, resultado.Id);
        Assert.AreEqual(_usuarioEjemplo.Email, resultado.Email);
        Assert.AreEqual(_usuarioEjemplo.Nombre, resultado.Nombre);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void BuscarUsuarioPorCorreoYContraseñaSinUsuariosAsociados()
    {
        string email = _usuarioEjemplo.Email;
        string contraseñaIncorrecta = "ContraseñaIncorrecta123";

        _usuarioController.BuscarUsuarioPorCorreoYContraseña(email, contraseñaIncorrecta);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void BuscarUsuarioPorCorreoYContraseñaVacias()
    {
        _usuarioController.BuscarUsuarioPorCorreoYContraseña("", "Contraseña1!");
    }

    [TestMethod]
    public void ConvertirUsuarioEnAdmin()
    {
        UsuarioDTO dto = Convertidor.AUsuarioDTO(_usuarioEjemplo);

        _usuarioController.ConvertirEnAdmin(dto);

        Assert.IsTrue(_usuarioEjemplo.EsAdminSistema);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ConvertirEnAdminUsuarioQueYaEsAdmin()
    {
        _usuarioEjemplo.EsAdminSistema = true;
        UsuarioDTO dto = Convertidor.AUsuarioDTO(_usuarioEjemplo);

        _usuarioController.ConvertirEnAdmin(dto);
    }

    [TestMethod]
    public void VerSiUaurioEsAdmin()
    {
        _usuarioEjemplo.EsAdminSistema = true;
        UsuarioDTO dto = Convertidor.AUsuarioDTO(_usuarioEjemplo);

        bool esAdmin = _usuarioController.EsAdmin(dto);

        Assert.IsTrue(esAdmin);
    }


}