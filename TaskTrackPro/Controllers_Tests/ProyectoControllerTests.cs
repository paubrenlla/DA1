using DTOs;
using Controllers;
using Moq;
using Services;

namespace Controllers_Tests
{
    [TestClass]
    public class ProyectoControllerTests
    {
        private Mock<IProyectoService> _mockService;
        private ProyectoController _controller;

        private ProyectoDTO _dto1;
        private ProyectoDTO _dto2;
        private List<ProyectoDTO> _listaDtos;
        private List<TareaDTO> _listaCriticas;
        private List<TareaDTO> _listaNoCrit;
        private List<TareaDTO> _listaOrden;

        [TestInitialize]
        public void SetUp()
        {
            _mockService = new Mock<IProyectoService>();
            _controller = new ProyectoController(_mockService.Object);

            _dto1 = new ProyectoDTO
            {
                Id = 1,
                Nombre = "P1",
                Descripcion = "Desc1",
                FechaInicio = DateTime.Today
            };
            _dto2 = new ProyectoDTO
            {
                Id = 2,
                Nombre = "P2",
                Descripcion = "Desc2",
                FechaInicio = DateTime.Today.AddDays(1)
            };
            _listaDtos = new List<ProyectoDTO> { _dto1, _dto2 };

            _listaCriticas = new List<TareaDTO>
            {
                new TareaDTO { Id = 1 }, new TareaDTO { Id = 2 }
            };
            _listaNoCrit = new List<TareaDTO>
            {
                new TareaDTO { Id = 3 }
            };
            _listaOrden = new List<TareaDTO>
            {
                new TareaDTO { Id = 1 }, new TareaDTO { Id = 3 }, new TareaDTO { Id = 2 }
            };
        }

        [TestMethod]
        public void BuscarProyectoPorIdLlamaServiceYDevuelveDto()
        {
            _mockService
                .Setup(s => s.GetById(1))
                .Returns(_dto1);

            ProyectoDTO resultado = _controller.BuscarProyectoPorId(1);

            Assert.AreEqual(_dto1, resultado);
            _mockService.Verify(s => s.GetById(1), Times.Once);
        }

        [TestMethod]
        public void GetAllLlamaServiceYDevuelveLista()
        {
            _mockService
                .Setup(s => s.GetAll())
                .Returns(_listaDtos);

            List<ProyectoDTO> resultado = _controller.GetAll();

            CollectionAssert.AreEqual(_listaDtos, resultado);
            _mockService.Verify(s => s.GetAll(), Times.Once);
        }

        [TestMethod]
        public void AgregarProyectoLlamaServiceYDevuelveNuevoDto()
        {
            ProyectoDTO nuevoDto = new ProyectoDTO
            {
                Nombre = "Nuevo",
                Descripcion = "Nueva desc",
                FechaInicio = DateTime.Today
            };
            ProyectoDTO creadoDto = new ProyectoDTO
            {
                Id = 5,
                Nombre = "Nuevo",
                Descripcion = "Nueva desc",
                FechaInicio = DateTime.Today
            };

            _mockService
                .Setup(s => s.CrearProyecto(nuevoDto))
                .Returns(creadoDto);

            ProyectoDTO resultado = _controller.AgregarProyecto(nuevoDto);

            Assert.AreEqual(creadoDto, resultado);
            _mockService.Verify(s => s.CrearProyecto(nuevoDto), Times.Once);
        }

        [TestMethod]
        public void EliminarProyectoLlamaService()
        {
            _controller.EliminarProyecto(42);

            _mockService.Verify(s => s.Delete(42), Times.Once);
        }

        [TestMethod]
        public void ModificarProyectoLlamaService()
        {
            ProyectoDTO dto = new ProyectoDTO
            {
                Id = 7,
                Nombre = "X",
                Descripcion = "Y",
                FechaInicio = DateTime.Today
            };

            _controller.ModificarProyecto(dto);

            _mockService.Verify(s => s.ModificarProyecto(dto), Times.Once);
        }

        [TestMethod]
        public void EsAdminDeAlgunProyectoDevuelveLoQueDevuelveService()
        {
            _mockService.Setup(s => s.UsuarioEsAdminDeAlgunProyecto(10)).Returns(true);

            bool esAdmin = _controller.EsAdminDeAlgunProyecto(10);

            Assert.IsTrue(esAdmin);
            _mockService.Verify(s => s.UsuarioEsAdminDeAlgunProyecto(10), Times.Once);
        }

        [TestMethod]
        public void ProyectosDelUsuarioLlamaAServiceYDevuelveListaDeProyectos()
        {
            _mockService.Setup(s => s.ProyectosDelUsuario(3)).Returns(_listaDtos);

            List<ProyectoDTO> resultado = _controller.ProyectosDelUsuario(3);

            CollectionAssert.AreEqual(_listaDtos, resultado);
            _mockService.Verify(s => s.ProyectosDelUsuario(3), Times.Once);
        }

        [TestMethod]
        public void EliminarAsignacionesUsuarioLlamaService()
        {
            _controller.EliminarAsignacionesUsuario(8);

            _mockService.Verify(s => s.EliminarAsignacionesDeUsuario(8), Times.Once);
        }

        [TestMethod]
        public void UsuarioEsAdminEnProyectoDevuelveLoQueDevuelveService()
        {
            _mockService
                .Setup(s => s.UsuarioEsAdminEnProyecto(2, 99))
                .Returns(false);

            bool resultado = _controller.UsuarioEsAdminEnProyecto(2, 99);

            Assert.IsFalse(resultado);
            _mockService.Verify(s => s.UsuarioEsAdminEnProyecto(2, 99), Times.Once);
        }
        
        [TestMethod]
        public void AsignarAdminProyecto_LlamaServiceConParametrosCorrectos()
        {
            int usuarioId = 7;
            int proyectoId = 4;

            _controller.AsignarAdminProyecto(usuarioId, proyectoId);

            _mockService.Verify(s => s.AsignarAdminDeProyecto(usuarioId, proyectoId), Times.Once);
        }

        [TestMethod]
        public void GetAdminDeProyecto_LlamaServiceYDevuelveUsuarioDTO()
        {
            int proyectoId = 123;
            UsuarioDTO adminEsperado = new UsuarioDTO
            {
                Id = 10,
                Nombre = "Admin",
                Email = "admin@example.com"
            };

            _mockService
                .Setup(s => s.GetAdminDeProyecto(proyectoId))
                .Returns(adminEsperado);

            UsuarioDTO resultado = _controller.GetAdminDeProyecto(proyectoId);

            Assert.AreEqual(adminEsperado, resultado);
            _mockService.Verify(s => s.GetAdminDeProyecto(proyectoId), Times.Once);
        }
        
        [TestMethod]
        public void GetMiembrosDeProyecto_LlamaServiceYDevuelveLista()
        {
            int proyectoId = 1;
            List<UsuarioDTO> miembrosEsperados = new List<UsuarioDTO>
            {
                new UsuarioDTO { Id = 1, Nombre = "Juan", Email = "juan@example.com" },
                new UsuarioDTO { Id = 2, Nombre = "Ana", Email = "ana@example.com" }
            };

            _mockService
                .Setup(s => s.GetMiembrosDeProyecto(proyectoId))
                .Returns(miembrosEsperados);

            List<UsuarioDTO>? resultado = _controller.GetMiembrosDeProyecto(proyectoId);

            CollectionAssert.AreEqual(miembrosEsperados, resultado);
            _mockService.Verify(s => s.GetMiembrosDeProyecto(proyectoId), Times.Once);
        }

        [TestMethod]
        public void AgregarMiembroProyecto_LlamaServiceConParametrosCorrectos()
        {
            int usuarioId = 5;
            int proyectoId = 10;

            _controller.AgregarMiembroProyecto(usuarioId, proyectoId);

            _mockService.Verify(s => s.AgregarMiembroProyecto(usuarioId, proyectoId), Times.Once);
        }

        [TestMethod]
        public void EliminarMiembro_LlamaServiceConParametrosCorrectos()
        {
            int miembroId = 3;
            int proyectoId = 7;

            _controller.EliminarMiembro(miembroId, proyectoId);

            _mockService.Verify(s => s.EliminarMiembroDeProyecto(miembroId, proyectoId), Times.Once);
        }
        
        [TestMethod]
        public void ObtenerRutaCritica_LlamaServiceYRetorna()
        {
            _mockService.Setup(s => s.ObtenerRutaCritica(10)).Returns(_listaCriticas);
            List<TareaDTO> resultado = _controller.ObtenerRutaCritica(10);
            CollectionAssert.AreEqual(_listaCriticas, resultado);
            _mockService.Verify(s => s.ObtenerRutaCritica(10), Times.Once);
        }

        [TestMethod]
        public void TareasNoCriticas_LlamaServiceYRetorna()
        {
            _mockService.Setup(s => s.TareasNoCriticas(20)).Returns(_listaNoCrit);
            List<TareaDTO> resultado = _controller.TareasNoCriticas(20);
            CollectionAssert.AreEqual(_listaNoCrit, resultado);
            _mockService.Verify(s => s.TareasNoCriticas(20), Times.Once);
        }

        [TestMethod]
        public void TareasOrdenadasPorInicio_LlamaServiceYRetorna()
        {
            _mockService.Setup(s => s.TareasOrdenadasPorInicio(30)).Returns(_listaOrden);
            List<TareaDTO> resultado = _controller.TareasOrdenadasPorInicio(30);
            CollectionAssert.AreEqual(_listaOrden, resultado);
            _mockService.Verify(s => s.TareasOrdenadasPorInicio(30), Times.Once);
        }
        
        [TestMethod]
        public void AsignarLiderProyecto_LlamaServiceConParametrosCorrectos()
        {
            int usuarioLiderId = 15;
            int nuevoId = 8;

            _controller.AsignarLiderProyecto(usuarioLiderId, nuevoId);

            _mockService.Verify(s => s.AsignarLiderDeProyecto(usuarioLiderId, nuevoId), Times.Once);
        }

        [TestMethod]
        public void GetLiderDeProyecto_LlamaServiceYDevuelveUsuarioDTO()
        {
            int proyectoId = 456;
            UsuarioDTO liderEsperado = new UsuarioDTO
            {
                Id = 25,
                Nombre = "Líder",
                Email = "lider@example.com"
            };

            _mockService
                .Setup(s => s.GetLiderDeProyecto(proyectoId))
                .Returns(liderEsperado);

            UsuarioDTO resultado = _controller.GetLiderDeProyecto(proyectoId);

            Assert.AreEqual(liderEsperado, resultado);
            _mockService.Verify(s => s.GetLiderDeProyecto(proyectoId), Times.Once);
        }

        [TestMethod]
        public void UsuarioEsLiderDeProyecto_DevuelveLoQueDevuelveService()
        {
            int usuarioId = 12;
            int proyectoId = 88;

            _mockService
                .Setup(s => s.UsuarioEsLiderDeProyecto(usuarioId, proyectoId))
                .Returns(true);

            bool resultado = _controller.UsuarioEsLiderDeProyecto(usuarioId, proyectoId);

            Assert.IsTrue(resultado);
            _mockService.Verify(s => s.UsuarioEsLiderDeProyecto(usuarioId, proyectoId), Times.Once);
        }
    }
}
