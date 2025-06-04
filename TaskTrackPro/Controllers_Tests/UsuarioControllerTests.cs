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

        [TestInitialize]
        public void SetUp()
        {
            _mockService = new Mock<IUsuarioService>();
            _controller = new UsuarioController(_mockService.Object);

            _dto1 = new UsuarioDTO
            {
                Id = 1,
                Nombre = "Usuario1",
                Email = "usuario1@test.com",
                Contraseña = "pass1"
            };
            
            _dto2 = new UsuarioDTO
            {
                Id = 2,
                Nombre = "Usuario2",
                Email = "usuario2@test.com",
                Contraseña = "pass2"
            };
        }

        [TestMethod]
        public void BuscarUsuarioPorId_LlamaServiceYDevuelveDto()
        {
            _mockService
                .Setup(s => s.GetById(1))
                .Returns(_dto1);

            UsuarioDTO resultado = _controller.BuscarUsuarioPorId(1);

            Assert.AreEqual(_dto1, resultado);
            _mockService.Verify(s => s.GetById(1), Times.Once);
        }

        [TestMethod]
        public void AgregarUsuario_LlamaService()
        {
            _controller.AgregarUsuario(_dto1);

            _mockService.Verify(s => s.CrearUsuario(_dto1), Times.Once);
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
            string email = "test@test.com";
            string contraseña = "password";
            
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

            UsuarioDTO resultado = _controller.BuscarUsuarioPorCorreo(email);

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
    }
}