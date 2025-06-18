using DTOs;
using Controllers;
using Moq;
using IServices;

namespace Controllers_Tests
{
    [TestClass]
    public class NotificacionControllerTests
    {
        private Mock<INotificacionService> _mockService;
        private NotificacionController _controller;

        private NotificacionDTO _dto1;
        private NotificacionDTO _dto2;
        private UsuarioDTO _usuarioDto;

        [TestInitialize]
        public void SetUp()
        {
            _mockService = new Mock<INotificacionService>();
            _controller = new NotificacionController(_mockService.Object);

            _usuarioDto = new UsuarioDTO
            {
                Id = 1,
                Nombre = "Test User",
                Email = "test@test.com",
            };

            _dto1 = new NotificacionDTO
            {
                Id = 1,
                Mensaje = "Notificación 1"
            };

            _dto2 = new NotificacionDTO
            {
                Id = 2,
                Mensaje = "Notificación 2"
            };
        }

        [TestMethod]
        public void Add_LlamaService()
        {
            _controller.Add(_dto1);

            _mockService.Verify(s => s.Add(_dto1), Times.Once);
        }

        [TestMethod]
        public void Remove_LlamaService()
        {
            _controller.Remove(_dto2);

            _mockService.Verify(s => s.Remove(_dto2), Times.Once);
        }

        [TestMethod]
        public void GetById_ConIdExistente_DevuelveNotificacion()
        {
            _mockService
                .Setup(s => s.GetById(1))
                .Returns(_dto1);

            NotificacionDTO resultado = _controller.GetById(1);

            Assert.AreEqual(_dto1, resultado);
            _mockService.Verify(s => s.GetById(1), Times.Once);
        }

        [TestMethod]
        public void GetById_ConIdInexistente_DevuelveNull()
        {
            _mockService
                .Setup(s => s.GetById(99))
                .Returns((NotificacionDTO?)null);

            NotificacionDTO resultado = _controller.GetById(99);

            Assert.IsNull(resultado);
            _mockService.Verify(s => s.GetById(99), Times.Once);
        }

        [TestMethod]
        public void GetAll_DevuelveTodasLasNotificaciones()
        {
            List<NotificacionDTO> listaEsperada = new List<NotificacionDTO> { _dto1, _dto2 };
            
            _mockService
                .Setup(s => s.GetAll())
                .Returns(listaEsperada);

            List<NotificacionDTO> resultado = _controller.GetAll();

            CollectionAssert.AreEqual(listaEsperada, resultado);
            _mockService.Verify(s => s.GetAll(), Times.Once);
        }

        [TestMethod]
        public void NotificacionesNoLeidas_DevuelveNotificacionesNoLeidasParaUsuario()
        {
            List<NotificacionDTO> listaEsperada = new List<NotificacionDTO> { _dto1 };
            
            _mockService
                .Setup(s => s.NotificacionesNoLeidas(_usuarioDto))
                .Returns(listaEsperada);

            List<NotificacionDTO> resultado = _controller.NotificacionesNoLeidas(_usuarioDto);

            CollectionAssert.AreEqual(listaEsperada, resultado);
            _mockService.Verify(s => s.NotificacionesNoLeidas(_usuarioDto), Times.Once);
        }
    }
}