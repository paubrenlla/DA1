using Domain;
using DTOs;
using IDataAcces;
using Moq;
using Services;

namespace Services_Tests
{
    [TestClass]
    public class NotificacionServiceTests
    {
        private NotificacionService _service;
        private Mock<IDataAccessNotificacion> _mockNotificacionRepo;
        private Mock<IDataAccessUsuario> _mockUsuarioRepo;
        
        private Notificacion _notificacion1;
        private Notificacion _notificacion2;
        private NotificacionDTO _dto1;
        private NotificacionDTO _dto2;
        private Usuario _usuario;

        [TestInitialize]
        public void SetUp()
        {
            _mockNotificacionRepo = new Mock<IDataAccessNotificacion>();
            _mockUsuarioRepo = new Mock<IDataAccessUsuario>();
            _service = new NotificacionService(_mockNotificacionRepo.Object);

            _notificacion1 = new Notificacion("Mensaje 1");
            _notificacion2 = new Notificacion("Mensaje 2");

            _dto1 = new NotificacionDTO { Id = 1, Mensaje = "Mensaje 1" };
            _dto2 = new NotificacionDTO { Id = 2, Mensaje = "Mensaje 2" };

            _usuario = new Usuario("test@test.com", "Test", "User", "pass1Ord!@*", new DateTime(1990, 1, 1));
        }

        [TestMethod]
        public void Add_CreaYGuardaNotificacionCorrectamente()
        {
            _mockNotificacionRepo.Setup(r => r.Add(It.IsAny<Notificacion>()));

            _service.Add(_dto1);

            _mockNotificacionRepo.Verify(r => r.Add(It.Is<Notificacion>(n =>
                n.Mensaje == _dto1.Mensaje)), Times.Once);
        }

        [TestMethod]
        public void Remove_EliminaNotificacionExistente()
        {
            _mockNotificacionRepo.Setup(r => r.GetById(1)).Returns(_notificacion1);
            _mockNotificacionRepo.Setup(r => r.Remove(_notificacion1));

            _service.Remove(_dto1);

            _mockNotificacionRepo.Verify(r => r.Remove(_notificacion1), Times.Once);
        }

        [TestMethod]
        public void Remove_NoHaceNadaCuandoNotificacionNoExiste()
        {
            _mockNotificacionRepo.Setup(r => r.GetById(99)).Returns((Notificacion)null);

            _service.Remove(new NotificacionDTO { Id = 99 });

            _mockNotificacionRepo.Verify(r => r.Remove(It.IsAny<Notificacion>()), Times.Never);
        }

        [TestMethod]
        public void GetById_DevuelveNotificacionCuandoExiste()
        {
            _mockNotificacionRepo.Setup(r => r.GetById(1)).Returns(_notificacion1);

            var resultado = _service.GetById(1);

            Assert.AreEqual(_notificacion1.Id, resultado.Id);
            Assert.AreEqual(_notificacion1.Mensaje, resultado.Mensaje);
            _mockNotificacionRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [TestMethod]
        public void GetById_DevuelveNullCuandoNoExiste()
        {
            _mockNotificacionRepo.Setup(r => r.GetById(99)).Returns((Notificacion)null);

            var resultado = _service.GetById(99);

            Assert.IsNull(resultado);
            _mockNotificacionRepo.Verify(r => r.GetById(99), Times.Once);
        }

        [TestMethod]
        public void GetAll_DevuelveTodasLasNotificaciones()
        {
            var listaNotificaciones = new List<Notificacion> { _notificacion1, _notificacion2 };
            _mockNotificacionRepo.Setup(r => r.GetAll()).Returns(listaNotificaciones);

            var resultado = _service.GetAll();

            Assert.AreEqual(2, resultado.Count);
            Assert.IsTrue(resultado.Any(n => n.Mensaje == "Mensaje 1"));
            Assert.IsTrue(resultado.Any(n => n.Mensaje == "Mensaje 2"));
            _mockNotificacionRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [TestMethod]
        public void NotificacionesNoLeidas_DevuelveSoloNoLeidasParaUsuario()
        {
            var usuarioDto = new UsuarioDTO { Id = 1 };
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario);
            
            var notificacionesNoLeidas = new List<Notificacion> { _notificacion1 };
            _mockNotificacionRepo.Setup(r => r.NotificacionesNoLeidas(_usuario)).Returns(notificacionesNoLeidas);

            _service = new NotificacionService(_mockNotificacionRepo.Object) { _UsuarioRepo = _mockUsuarioRepo.Object };

            var resultado = _service.NotificacionesNoLeidas(usuarioDto);

            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual(_notificacion1.Id, resultado[0].Id);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
            _mockNotificacionRepo.Verify(r => r.NotificacionesNoLeidas(_usuario), Times.Once);
        }
    }
}