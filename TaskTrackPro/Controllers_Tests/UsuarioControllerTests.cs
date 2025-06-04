using DTOs;
using Controllers;
using Moq;
using Services;

namespace Controllers_Tests
{
    [TestClass]
    public class UsuarioControllerTests
    {
        private Mock<IUsuarioService> _mockService;
        private UsuarioController _controller;

        private UsuarioDTO _dto1;
        private UsuarioDTO _dto2;
        private UsuarioConContraseñaDTO _dtoConPwd;

        [TestInitialize]
        public void SetUp()
        {
            _mockService = new Mock<IUsuarioService>();
            _controller = new UsuarioController(_mockService.Object);

            _dto1 = new UsuarioDTO
            {
                Id              = 1,
                Nombre          = "Usuario1",
                Apellido        = "Apellido1",
                Email           = "usuario1@test.com",
                FechaNacimiento = new DateTime(1990, 1, 1)
            };

            _dto2 = new UsuarioDTO
            {
                Id              = 2,
                Nombre          = "Usuario2",
                Apellido        = "Apellido2",
                Email           = "usuario2@test.com",
                FechaNacimiento = new DateTime(1991, 2, 2)
            };

            _dtoConPwd = new UsuarioConContraseñaDTO
            {
                Id              = 3,
                Email           = "nuevo@test.com",
                Nombre          = "Nuevo",
                Apellido        = "Usuario",
                FechaNacimiento = new DateTime(2000, 1, 1),
                Contraseña      = "PwdSegura1!"
            };
        }

        [TestMethod]
        public void GetById_LlamaServiceYDevuelveDto()
        {
            _mockService
                .Setup(s => s.GetById(1))
                .Returns(_dto1);

            UsuarioDTO resultado = _controller.GetById(1);

            Assert.AreEqual(_dto1, resultado);
            _mockService.Verify(s => s.GetById(1), Times.Once);
        }

        [TestMethod]
        public void CrearUsuario_LlamaService()
        {
            _controller.CrearUsuario(_dtoConPwd);

            _mockService.Verify(s => s.CrearUsuario(_dtoConPwd), Times.Once);
        }

        [TestMethod]
        public void EliminarUsuario_LlamaService()
        {
            _controller.EliminarUsuario(_dto2);

            _mockService.Verify(s => s.Delete(_dto2), Times.Once);
        }

        [TestMethod]
        public void BuscarUsuarioPorCorreoYContraseña_LlamaServiceYDevuelveDto()
        {
            string email       = "test@test.com";
            string contraseña  = "password";

            _mockService
                .Setup(s => s.BuscarUsuarioPorCorreoYContraseña(email, contraseña))
                .Returns(_dto1);

            UsuarioDTO resultado = _controller.BuscarUsuarioPorCorreoYContraseña(email, contraseña);

            Assert.AreEqual(_dto1, resultado);
            _mockService.Verify(s => s.BuscarUsuarioPorCorreoYContraseña(email, contraseña), Times.Once);
        }

        [TestMethod]
        public void BuscarUsuarioPorCorreo_LlamaServiceYDevuelveDto()
        {
            string email = "usuario1@test.com";

            _mockService
                .Setup(s => s.GetByEmail(email))
                .Returns(_dto1);

            UsuarioDTO resultado = _controller.GetByEmail(email);

            Assert.AreEqual(_dto1, resultado);
            _mockService.Verify(s => s.GetByEmail(email), Times.Once);
        }

        [TestMethod]
        public void ConvertirEnAdmin_LlamaService()
        {
            _controller.ConvertirEnAdmin(_dto1);

            _mockService.Verify(s => s.ConvertirEnAdmin(_dto1), Times.Once);
        }

        [TestMethod]
        public void EsAdmin_DevuelveLoQueDevuelveService()
        {
            _mockService
                .Setup(s => s.EsAdmin(_dto1))
                .Returns(true);

            bool resultado = _controller.EsAdmin(_dto1);

            Assert.IsTrue(resultado);
            _mockService.Verify(s => s.EsAdmin(_dto1), Times.Once);
        }

        [TestMethod]
        public void ModificarUsuario_LlamaService()
        {
            _controller.ModificarUsuario(_dtoConPwd);

            _mockService.Verify(s => s.ModificarUsuario(_dtoConPwd), Times.Once);
        }

        [TestMethod]
        public void ResetearContraseña_LlamaServiceYDevuelveEncriptada()
        {
            _mockService
                .Setup(s => s.ResetearContraseña(3))
                .Returns("hash123");

            string resultado = _controller.ResetearContraseña(3);

            Assert.AreEqual("hash123", resultado);
            _mockService.Verify(s => s.ResetearContraseña(3), Times.Once);
        }

        [TestMethod]
        public void GenerarContraseñaAleatoria_LlamaServiceYDevuelveEncriptada()
        {
            _mockService
                .Setup(s => s.GenerarContraseñaAleatoria(3))
                .Returns("nuevoHash");

            string resultado = _controller.GenerarContraseñaAleatoria(3);

            Assert.AreEqual("nuevoHash", resultado);
            _mockService.Verify(s => s.GenerarContraseñaAleatoria(3), Times.Once);
        }

        [TestMethod]
        public void DesencriptarContraseña_LlamaServiceYDevuelveTextoPlano()
        {
            _mockService
                .Setup(s => s.DesencriptarContraseña(3))
                .Returns("textoPlano");

            string resultado = _controller.DesencriptarContraseña(3);

            Assert.AreEqual("textoPlano", resultado);
            _mockService.Verify(s => s.DesencriptarContraseña(3), Times.Once);
        }
    }
}
