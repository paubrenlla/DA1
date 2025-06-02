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
            
            // El constructor real solo usa usuarioRepo, pero necesitamos mockear proyectoService para las pruebas
            _service = new UsuarioService(_mockUsuarioRepo.Object);
            
            // Configurar el servicio para usar el mock de proyectoService (necesitarías un setter o modificar el constructor)
            // Esto es un problema de diseño - el servicio debería recibir todas sus dependencias por constructor
            // Asumiré que se modifica el constructor para aceptar IProyectoService
            
            _usuario1 = new Usuario("u1@test.com", "Nombre1", "Apellido1", "Contraseña1!", new DateTime(1990, 1, 1));
            _usuario2 = new Usuario("u2@test.com", "Nombre2", "Apellido2", "Contraseña1!", new DateTime(1991, 1, 1));
            _usuario2.EsAdminSistema = true;

            _dto1 = new UsuarioDTO
            {
                Id = 1,
                Nombre = "Nombre1",
                Apellido = "Apellido1",
                Email = "u1@test.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Contraseña = "Contraseña1!",
            };

            _dto2 = new UsuarioDTO
            {
                Id = 2,
                Nombre = "Nombre2",
                Apellido = "Apellido2",
                Email = "u2@test.com",
                FechaNacimiento = new DateTime(1991, 1, 1),
                Contraseña = "Contraseña1!",
            };
        }

        [TestMethod]
        public void GetById_DevuelveDTOBien()
        {
            // Arrange
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            // Act
            UsuarioDTO resultado = _service.GetById(1);

            // Assert
            Assert.AreEqual(_usuario1.Id, resultado.Id);
            Assert.AreEqual(_usuario1.Nombre, resultado.Nombre);
            Assert.AreEqual(_usuario1.Email, resultado.Email);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [TestMethod]
        public void CrearUsuario_GuardaUsuarioCorrectamente()
        {
            // Arrange
            _mockUsuarioRepo.Setup(r => r.Add(It.IsAny<Usuario>()));

            // Act
            _service.CrearUsuario(_dto1);

            // Assert
            _mockUsuarioRepo.Verify(r => r.Add(It.Is<Usuario>(u => 
                u.Nombre == _dto1.Nombre && 
                u.Email == _dto1.Email)), Times.Once);
        }

        // [TestMethod]
        // public void Delete_EliminaUsuarioCuandoNoEsAdmin()
        // {
        //     // Arrange
        //     _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);
        //     _mockProyectoService.Setup(s => s.UsuarioEsAdminDeAlgunProyecto(1)).Returns(false);
        //
        //     // Act
        //     _service.Delete(_dto1);
        //
        //     // Assert
        //     _mockUsuarioRepo.Verify(r => r.Remove(_usuario1), Times.Once);
        // }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Delete_LanzaExcepcionCuandoEsAdminSistema()
        {
            // Arrange
            _mockUsuarioRepo.Setup(r => r.GetById(2)).Returns(_usuario2);

            // Act
            _service.Delete(_dto2);

            // Assert - ExpectedException
        }

        // [TestMethod]
        // [ExpectedException(typeof(ArgumentException))]
        // public void Delete_LanzaExcepcionCuandoEsAdminProyecto()
        // {
        //     // Arrange
        //     _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);
        //     _mockProyectoService.Setup(s => s.UsuarioEsAdminDeAlgunProyecto(1)).Returns(true);
        //
        //     // Act
        //     _service.Delete(_dto1);
        //
        //     // Assert - ExpectedException
        // }

        [TestMethod]
        public void GetByEmail_DevuelveUsuarioCorrecto()
        {
            // Arrange
            string email = "u1@test.com";
            _mockUsuarioRepo.Setup(r => r.BuscarUsuarioPorCorreo(email)).Returns(_usuario1);

            // Act
            UsuarioDTO resultado = _service.GetByEmail(email);

            // Assert
            Assert.AreEqual(_usuario1.Id, resultado.Id);
            Assert.AreEqual(_usuario1.Email, resultado.Email);
            _mockUsuarioRepo.Verify(r => r.BuscarUsuarioPorCorreo(email), Times.Once);
        }

        [TestMethod]
        public void BuscarUsuarioPorCorreoYContraseña_DevuelveUsuarioCuandoCredencialesSonCorrectas()
        {
            // Arrange
            string email = "u1@test.com";
            string contraseña = "Contraseña1!";
            _mockUsuarioRepo.Setup(r => r.buscarUsuarioPorCorreoYContraseña(email, contraseña)).Returns(_usuario1);

            // Act
            UsuarioDTO resultado = _service.BuscarUsuarioPorCorreoYContraseña(email, contraseña);

            // Assert
            Assert.AreEqual(_usuario1.Id, resultado.Id);
            _mockUsuarioRepo.Verify(r => r.buscarUsuarioPorCorreoYContraseña(email, contraseña), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuscarUsuarioPorCorreoYContraseña_LanzaExcepcionCuandoCredencialesSonVacias()
        {
            // Act
            _service.BuscarUsuarioPorCorreoYContraseña("", "");
            
            // Assert - ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuscarUsuarioPorCorreoYContraseña_LanzaExcepcionCuandoCredencialesSonInvalidas()
        {
            // Arrange
            string email = "u1@test.com";
            string contraseña = "incorrecta";
            _mockUsuarioRepo.Setup(r => r.buscarUsuarioPorCorreoYContraseña(email, contraseña)).Returns((Usuario)null);

            // Act
            _service.BuscarUsuarioPorCorreoYContraseña(email, contraseña);
            
            // Assert - ExpectedException
        }

        [TestMethod]
        public void ConvertirEnAdmin_ConvierteUsuarioCorrectamente()
        {
            // Arrange
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            // Act
            _service.ConvertirEnAdmin(_dto1);

            // Assert
            Assert.IsTrue(_usuario1.EsAdminSistema);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertirEnAdmin_LanzaExcepcionCuandoYaEsAdmin()
        {
            // Arrange
            _mockUsuarioRepo.Setup(r => r.GetById(2)).Returns(_usuario2);

            // Act
            _service.ConvertirEnAdmin(_dto2);
            
            // Assert - ExpectedException
        }

        [TestMethod]
        public void EsAdmin_DevuelveTrueCuandoUsuarioEsAdmin()
        {
            // Arrange
            _mockUsuarioRepo.Setup(r => r.GetById(2)).Returns(_usuario2);

            // Act
            bool resultado = _service.EsAdmin(_dto2);

            // Assert
            Assert.IsTrue(resultado);
            _mockUsuarioRepo.Verify(r => r.GetById(2), Times.Once);
        }

        [TestMethod]
        public void EsAdmin_DevuelveFalseCuandoUsuarioNoEsAdmin()
        {
            // Arrange
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            // Act
            bool resultado = _service.EsAdmin(_dto1);

            // Assert
            Assert.IsFalse(resultado);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }
    }
}