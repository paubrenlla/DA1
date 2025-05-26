using BusinessLogic;
using IDataAcces;
using Repositorios;
using Services;
using Services.DTOs;

namespace Services_Tests;

[TestClass]
public class UsuarioServiceTests
{
    private UsuarioService _usuarioService;
    private IDataAccessUsuario _repoUsuarios;
    private IDataAccessProyecto _repoProyectos;

    private Usuario _usuarioEjemplo;

    [TestInitialize]
    public void SetUp()
    {
        _repoUsuarios = new UsuarioDataAccess();
        _repoProyectos = new ProyectoDataAccess();

        _usuarioService = new UsuarioService(_repoUsuarios, _repoProyectos);

        _usuarioEjemplo = new Usuario("juan@mail.com", "Juan", "Pérez", "Contraseña1!", new DateTime(2000, 1, 1));
        _repoUsuarios.Add(_usuarioEjemplo);
    }

    [TestMethod]
    public void BuscarUsuarioPorIdDevuelveDTOCorrecto()
    {
        UsuarioDTO resultado = _usuarioService.BuscarUsuarioPorId(_usuarioEjemplo.Id);

        Assert.AreEqual(_usuarioEjemplo.Id, resultado.Id);
        Assert.AreEqual(_usuarioEjemplo.Email, resultado.Email);
        Assert.AreEqual(_usuarioEjemplo.Nombre, resultado.Nombre);
        Assert.AreEqual(_usuarioEjemplo.Apellido, resultado.Apellido);
    }
    
    [TestMethod]
    public void AgregarUsuarioValidoCorrectamente()
    {
        var dto = new UsuarioCreateDTO
        {
            Email = "nuevo@mail.com",
            Nombre = "Nuevo",
            Apellido = "Usuario",
            Contraseña = "Contraseña1!",
            FechaNacimiento = new DateTime(1990, 1, 1)
        };

        _usuarioService.AgregarUsuario(dto);

        UsuarioDTO resultado = _usuarioService.BuscarUsuarioPorCorreo(dto.Email);

        Assert.AreEqual(dto.Email, resultado.Email);
        Assert.AreEqual(dto.Nombre, resultado.Nombre);
        Assert.AreEqual(dto.Apellido, resultado.Apellido);
        Assert.AreEqual(dto.FechaNacimiento, resultado.FechaNacimiento);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuarioConCorreoQueYaExiste()
    {
        var dto = new UsuarioCreateDTO
        {
            Email = "juan@mail.com",
            Nombre = "Pepe",
            Apellido = "Jose",
            Contraseña = "Contraseña1!",
            FechaNacimiento = new DateTime(1995, 5, 5)
        };
        _usuarioService.AgregarUsuario(dto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuarioSinCorreo()
    {
        var dto = new UsuarioCreateDTO
        {
            Email = "",
            Nombre = "Pepe",
            Apellido = "Jose",
            Contraseña = "Contraseña1!",
            FechaNacimiento = new DateTime(1995, 5, 5)
        };
        _usuarioService.AgregarUsuario(dto);
    }
    
    [TestMethod]
    public void EliminarUsuarioExistenteEliminaCorrectamente()
    {
        _usuarioService.EliminarUsuario(Convertidor.AUsuarioDTO(_usuarioEjemplo));

        var resultado = _repoUsuarios.BuscarUsuarioPorCorreo(_usuarioEjemplo.Email);
        Assert.IsNull(resultado);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuarioQueEsAdminSistema()
    {
        _usuarioEjemplo.EsAdminSistema = true;
        var dto = Convertidor.AUsuarioDTO(_usuarioEjemplo);
        _usuarioService.EliminarUsuario(dto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarUsuarioQueEsAdminDeUnProyecto()
    {
        Proyecto proyecto = new Proyecto("Proyecto de Prueba", "Desc", DateTime.Now );
        proyecto.AsignarAdmin(_usuarioEjemplo);
        _repoProyectos.Add(proyecto);
        var dto = Convertidor.AUsuarioDTO(_usuarioEjemplo);
        _usuarioService.EliminarUsuario(dto);
    }

    [TestMethod]
    public void BuscarUsuarioPorCorreoYContraseña()
    {
        string email = _usuarioEjemplo.Email;
        string contraseña = "Contraseña1!";

        UsuarioDTO resultado = _usuarioService.BuscarUsuarioPorCorreoYContraseña(email, contraseña);

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

        _usuarioService.BuscarUsuarioPorCorreoYContraseña(email, contraseñaIncorrecta);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void BuscarUsuarioPorCorreoYContraseñaVacias()
    {
        _usuarioService.BuscarUsuarioPorCorreoYContraseña("", "Contraseña1!");
    }

    [TestMethod]
    public void ConvertirUsuarioEnAdmin()
    {
        UsuarioDTO dto = Convertidor.AUsuarioDTO(_usuarioEjemplo);

        _usuarioService.ConvertirEnAdmin(dto);

        Assert.IsTrue(_usuarioEjemplo.EsAdminSistema);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ConvertirEnAdminUsuarioQueYaEsAdmin()
    {
        _usuarioEjemplo.EsAdminSistema = true;
        UsuarioDTO dto = Convertidor.AUsuarioDTO(_usuarioEjemplo);

        _usuarioService.ConvertirEnAdmin(dto);
    }

    [TestMethod]
    public void VerSiUaurioEsAdmin()
    {
        _usuarioEjemplo.EsAdminSistema = true;
        UsuarioDTO dto = Convertidor.AUsuarioDTO(_usuarioEjemplo);

        bool esAdmin = _usuarioService.EsAdmin(dto);

        Assert.IsTrue(esAdmin);
    }


}