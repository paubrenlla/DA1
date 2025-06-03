using Domain;
using DTOs;
using IDataAcces;
using Moq;
using Services;

namespace Services_Tests
{
    [TestClass]
    public class UsuarioServiceTests
    {
        private UsuarioService _service;
        private Mock<IDataAccessUsuario> _mockUsuarioRepo;
        private Mock<IProyectoService> _mockProyectoService;
        
        private Usuario _usuario1;
        private Usuario _usuario2;
        private UsuarioDTO _dto1;
        private UsuarioDTO _dto2;

        [TestInitialize]
        public void SetUp()
        {
            _mockUsuarioRepo = new Mock<IDataAccessUsuario>();
            _mockProyectoService = new Mock<IProyectoService>();
            _service = new UsuarioService(_mockUsuarioRepo.Object);

            _usuario1 = new Usuario(
                "u1@test.com",
                "Nombre1",
                "Apellido1",
                "Contraseña1!",
                new DateTime(1990, 1, 1));
            _usuario2 = new Usuario(
                "u2@test.com",
                "Nombre2",
                "Apellido2",
                "Contraseña1!",
                new DateTime(1991, 1, 1));
            _usuario2.EsAdminSistema = true;

            _dto1 = new UsuarioDTO
            {
                Id = 1,
                Nombre = "Nombre1",
                Apellido = "Apellido1",
                Email = "u1@test.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Contraseña = "Contraseña1!"
            };

            _dto2 = new UsuarioDTO
            {
                Id = 2,
                Nombre = "Nombre2",
                Apellido = "Apellido2",
                Email = "u2@test.com",
                FechaNacimiento = new DateTime(1991, 1, 1),
                Contraseña = "Contraseña1!"
            };
        }

        [TestMethod]
        public void GetByIdDevuelveDTOBien()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            UsuarioDTO resultado = _service.GetById(1);

            Assert.AreEqual(_usuario1.Id, resultado.Id);
            Assert.AreEqual(_usuario1.Nombre, resultado.Nombre);
            Assert.AreEqual(_usuario1.Email, resultado.Email);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [TestMethod]
        public void CrearUsuarioGuardaUsuarioCorrectamente()
        {
            _mockUsuarioRepo.Setup(r => r.Add(It.IsAny<Usuario>()));

            _service.CrearUsuario(_dto1);

            _mockUsuarioRepo.Verify(r => r.Add(It.Is<Usuario>(u =>
                u.Nombre == _dto1.Nombre &&
                u.Email == _dto1.Email)), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteLanzaExcepcionCuandoEsAdminSistema()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(2)).Returns(_usuario2);

            _service.Delete(_dto2);
        }

        [TestMethod]
        public void GetByEmailDevuelveUsuarioCorrecto()
        {
            string email = "u1@test.com";
            _mockUsuarioRepo.Setup(r => r.BuscarUsuarioPorCorreo(email)).Returns(_usuario1);

            UsuarioDTO resultado = _service.GetByEmail(email);

            Assert.AreEqual(_usuario1.Id, resultado.Id);
            Assert.AreEqual(_usuario1.Email, resultado.Email);
            _mockUsuarioRepo.Verify(r => r.BuscarUsuarioPorCorreo(email), Times.Once);
        }

        [TestMethod]
        public void BuscarUsuarioPorCorreoYContraseñaDevuelveUsuarioCuandoCredencialesSonCorrectas()
        {
            string email = "u1@test.com";
            string contraseña = "Contraseña1!";
            _mockUsuarioRepo.Setup(r => r.buscarUsuarioPorCorreoYContraseña(email, contraseña)).Returns(_usuario1);

            UsuarioDTO resultado = _service.BuscarUsuarioPorCorreoYContraseña(email, contraseña);

            Assert.AreEqual(_usuario1.Id, resultado.Id);
            _mockUsuarioRepo.Verify(r => r.buscarUsuarioPorCorreoYContraseña(email, contraseña), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuscarUsuarioPorCorreoYContraseñaLanzaExcepcionCuandoCredencialesSonVacias()
        {
            _service.BuscarUsuarioPorCorreoYContraseña(string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuscarUsuarioPorCorreoYContraseñaLanzaExcepcionCuandoCredencialesSonInvalidas()
        {
            string email = "u1@test.com";
            string contraseña = "incorrecta";
            _mockUsuarioRepo.Setup(r => r.buscarUsuarioPorCorreoYContraseña(email, contraseña)).Returns((Usuario)null);

            _service.BuscarUsuarioPorCorreoYContraseña(email, contraseña);
        }

        [TestMethod]
        public void ConvertirEnAdminConvierteUsuarioCorrectamente()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            _service.ConvertirEnAdmin(_dto1);

            Assert.IsTrue(_usuario1.EsAdminSistema);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertirEnAdminLanzaExcepcionCuandoYaEsAdmin()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(2)).Returns(_usuario2);

            _service.ConvertirEnAdmin(_dto2);
        }

        [TestMethod]
        public void EsAdminDevuelveTrueCuandoUsuarioEsAdmin()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(2)).Returns(_usuario2);

            bool resultado = _service.EsAdmin(_dto2);

            Assert.IsTrue(resultado);
            _mockUsuarioRepo.Verify(r => r.GetById(2), Times.Once);
        }

        [TestMethod]
        public void EsAdminDevuelveFalseCuandoUsuarioNoEsAdmin()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            bool resultado = _service.EsAdmin(_dto1);

            Assert.IsFalse(resultado);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }
    }
}
