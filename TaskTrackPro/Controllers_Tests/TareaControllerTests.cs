using DTOs;
using Controllers;
using Domain.Enums;
using Moq;
using Services;

namespace Controllers_Tests
{
    [TestClass]
    public class TareaControllerTests
    {
        private Mock<ITareaService> _mockService;
        private TareaController _controller;

        private TareaDTO _dto1;
        private TareaDTO _dto2;
        private List<TareaDTO> _listaDtos;

        [TestInitialize]
        public void SetUp()
        {
            _mockService = new Mock<ITareaService>();
            _controller = new TareaController(_mockService.Object);

            _dto1 = new TareaDTO
            {
                Id = 1,
                Titulo = "T1",
                Descripcion = "Desc1",
                FechaInicio = DateTime.Today,
                Duracion = TimeSpan.FromHours(2)
            };
            _dto2 = new TareaDTO
            {
                Id = 2,
                Titulo = "T2",
                Descripcion = "Desc2",
                FechaInicio = DateTime.Today.AddDays(1),
                Duracion = TimeSpan.FromHours(3)
            };
            _listaDtos = new List<TareaDTO> { _dto1, _dto2 };
        }

        [TestMethod]
        public void BuscarTareaPorIdLlamaServiceYDevuelveDto()
        {
            _mockService.Setup(s => s.BuscarTareaPorId(1)).Returns(_dto1);

            TareaDTO resultado = _controller.BuscarTareaPorId(1);

            Assert.AreEqual(_dto1, resultado);
            _mockService.Verify(s => s.BuscarTareaPorId(1), Times.Once);
        }

        [TestMethod]
        public void ListarTareasPorProyectoLlamaServiceYDevuelveLista()
        {
            _mockService.Setup(s => s.ListarTareasPorProyecto(5)).Returns(_listaDtos);

            List<TareaDTO> resultado = _controller.ListarTareasPorProyecto(5);

            CollectionAssert.AreEqual(_listaDtos, resultado);
            _mockService.Verify(s => s.ListarTareasPorProyecto(5), Times.Once);
        }

        [TestMethod]
        public void CrearTareaLlamaServiceYDevuelveNuevoDto()
        {
            TareaDTO nuevoDto = new TareaDTO
            {
                Titulo = "Nueva",
                Descripcion = "NuevaDesc",
                FechaInicio = DateTime.Today.AddDays(2),
                Duracion = TimeSpan.FromHours(4)
            };
            TareaDTO creadoDto = new TareaDTO
            {
                Id = 10,
                Titulo = "Nueva",
                Descripcion = "NuevaDesc",
                FechaInicio = DateTime.Today.AddDays(2),
                Duracion = TimeSpan.FromHours(4)
            };

            _mockService.Setup(s => s.CrearTarea(3, nuevoDto)).Returns(creadoDto);

            TareaDTO resultado = _controller.CrearTarea(3, nuevoDto);

            Assert.AreEqual(creadoDto, resultado);
            _mockService.Verify(s => s.CrearTarea(3, nuevoDto), Times.Once);
        }

        [TestMethod]
        public void ModificarTareaLlamaServiceModificar()
        {
            TareaDTO dto = new TareaDTO
            {
                Id = 7,
                Titulo = "Mod",
                Descripcion = "ModDesc",
                FechaInicio = DateTime.Today.AddDays(5),
                Duracion = TimeSpan.FromHours(5)
            };

            _controller.ModificarTarea(7, dto, 1);

            _mockService.Verify(s => s.ModificarTarea(7, dto, 1), Times.Once);
        }

        [TestMethod]
        public void MarcarComoEjecutandoseLlamaService()
        {
            _controller.MarcarComoEjecutandose(4);

            _mockService.Verify(s => s.MarcarComoEjecutandose(4), Times.Once);
        }

        [TestMethod]
        public void MarcarComoCompletadaLlamaService()
        {
            _controller.MarcarComoCompletada(4);

            _mockService.Verify(s => s.MarcarComoCompletada(4), Times.Once);
        }

        [TestMethod]
        public void AgregarDependenciaLlamaService()
        {
            _controller.AgregarDependencia(2, 8, 1);

            _mockService.Verify(s => s.AgregarDependencia(2, 8, 1), Times.Once);
        }

        [TestMethod]
        public void AgregarUsuarioLlamaService()
        {
            _controller.AgregarUsuario(6, 12);

            _mockService.Verify(s => s.AgregarUsuario(6, 12), Times.Once);
        }
        
        [TestMethod]
        public void TieneSucesoras_RetornaTrueCuandoServiceDevuelveTrue()
        {
            _mockService
                .Setup(s => s.TieneSucesoras(_dto1))
                .Returns(false);

            bool resultado = _controller.TieneSucesoras(_dto1);

            Assert.IsFalse(resultado);
            _mockService.Verify(s => s.TieneSucesoras(_dto1), Times.Once);
        }
        
        [TestMethod]
        public void UsuarioPerteneceALaTareaLlamaServiceYDevuelveTrue()
        {
            int usuarioId = 7;
            int tareaId = 20;

            _mockService
                .Setup(s => s.UsuarioPerteneceALaTarea(usuarioId, tareaId))
                .Returns(true);

            bool resultado = _controller.UsuarioPerteneceALaTarea(usuarioId, tareaId);

            Assert.IsTrue(resultado);
            _mockService.Verify(s => s.UsuarioPerteneceALaTarea(usuarioId, tareaId), Times.Once);
        }
        
        [TestMethod]
        public void EliminarTarea_LlamaService()
        {
            int proyectoId = 42;
            int tareaId    = 123;

            _controller.EliminarTarea(proyectoId, tareaId);

            _mockService.Verify(s => s.EliminarTarea(proyectoId, tareaId), Times.Once);
        }

        
        [TestMethod]
        public void GetEstadoTarea_LlamaServiceYDevuelveEstadoPendiente()
        {
            int tareaId = 1;
            _mockService.Setup(s => s.GetEstadoTarea(tareaId)).Returns(TipoEstadoTarea.Pendiente);

            TipoEstadoTarea resultado = _controller.GetEstadoTarea(tareaId);

            Assert.AreEqual(TipoEstadoTarea.Pendiente, resultado);
            _mockService.Verify(s => s.GetEstadoTarea(tareaId), Times.Once);
        }

        [TestMethod]
        public void GetEstadoTarea_LlamaServiceYDevuelveEstadoRealizandose()
        {
            int tareaId = 2;
            _mockService.Setup(s => s.GetEstadoTarea(tareaId)).Returns(TipoEstadoTarea.Ejecutandose);

            TipoEstadoTarea resultado = _controller.GetEstadoTarea(tareaId);

            Assert.AreEqual(TipoEstadoTarea.Ejecutandose, resultado);
            _mockService.Verify(s => s.GetEstadoTarea(tareaId), Times.Once);
        }

        [TestMethod]
        public void GetEstadoTarea_LlamaServiceYDevuelveEstadoEfectuada()
        {
            int tareaId = 3;
            _mockService.Setup(s => s.GetEstadoTarea(tareaId)).Returns(TipoEstadoTarea.Efectuada);

            TipoEstadoTarea resultado = _controller.GetEstadoTarea(tareaId);

            Assert.AreEqual(TipoEstadoTarea.Efectuada, resultado);
            _mockService.Verify(s => s.GetEstadoTarea(tareaId), Times.Once);
        }

        [TestMethod]
        public void GetEstadoTarea_LlamaServiceYDevuelveEstadoBloqueada()
        {
            int tareaId = 4;
            _mockService.Setup(s => s.GetEstadoTarea(tareaId)).Returns(TipoEstadoTarea.Bloqueada);

            TipoEstadoTarea resultado = _controller.GetEstadoTarea(tareaId);

            Assert.AreEqual(TipoEstadoTarea.Bloqueada, resultado);
            _mockService.Verify(s => s.GetEstadoTarea(tareaId), Times.Once);
        }

        [TestMethod]
        public void ListarUsuariosDeTarea_ConTareaExistente_DevuelveListaUsuarios()
        {
            int tareaId = 1;
            var usuariosEsperados = new List<UsuarioDTO>
            {
                new UsuarioDTO { Id = 1, Nombre = "Usuario 1", Email = "u1@test.com" },
                new UsuarioDTO { Id = 2, Nombre = "Usuario 2", Email = "u2@test.com" }
            };
    
            _mockService.Setup(s => s.ListarUsuariosDeTarea(tareaId)).Returns(usuariosEsperados);
            
            var resultado = _controller.ListarUsuariosDeTarea(tareaId);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.AreEqual("Usuario 1", resultado[0].Nombre);
            _mockService.Verify(s => s.ListarUsuariosDeTarea(tareaId), Times.Once);
        }

        [TestMethod]
        public void ListarUsuariosDeTarea_ConTareaSinUsuarios_DevuelveListaVacia()
        {
            int tareaId = 2;
            var listaVacia = new List<UsuarioDTO>();
    
            _mockService.Setup(s => s.ListarUsuariosDeTarea(tareaId))
                .Returns(listaVacia);

            var resultado = _controller.ListarUsuariosDeTarea(tareaId);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
            _mockService.Verify(s => s.ListarUsuariosDeTarea(tareaId), Times.Once);
        }

        [TestMethod]
        public void ListarUsuariosDeTarea_ConTareaInexistente_DevuelveNull()
        {
            int tareaId = 999;
    
            _mockService.Setup(s => s.ListarUsuariosDeTarea(tareaId))
                .Returns((List<UsuarioDTO>)null);

            var resultado = _controller.ListarUsuariosDeTarea(tareaId);

            Assert.IsNull(resultado);
            _mockService.Verify(s => s.ListarUsuariosDeTarea(tareaId), Times.Once);
        }
        
        [TestMethod]
        public void EliminarMiembroDeTarea_LlamaService()
        {
            int miembroId = 15;
            int idTarea = 8;

            _controller.EliminarMiembroDeTarea(miembroId, idTarea);

            _mockService.Verify(s => s.EliminarUsuarioDeTarea(miembroId, idTarea), Times.Once);
        }
        
        [TestMethod]
        public void EliminarAUsuarioDeTareasDeProyecto_LlamaService()
        {
            int miembroId = 10;
            int proyectoId = 5;

            _controller.EliminarAUsuarioDeTareasDeProyecto(miembroId, proyectoId);

            _mockService.Verify(s => s.EliminarUsuarioDeTareasDeProyecto(miembroId, proyectoId), Times.Once);
        }
        
        [TestMethod]
        public void ObtenerDependenciasDeTarea_ConTareaConDependencias_DevuelveListaDependencias()
        {
            int tareaId = 1;
            var dependenciasEsperadas = new List<TareaDTO>
            {
                new TareaDTO { Id = 3, Titulo = "Dependencia 1", Descripcion = "Desc Dep 1" },
                new TareaDTO { Id = 4, Titulo = "Dependencia 2", Descripcion = "Desc Dep 2" }
            };

            _mockService.Setup(s => s.ObtenerDependenciasDeTarea(tareaId))
                .Returns(dependenciasEsperadas);

            List<TareaDTO>? resultado = _controller.ObtenerDependenciasDeTarea(tareaId);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.AreEqual("Dependencia 1", resultado[0].Titulo);
            _mockService.Verify(s => s.ObtenerDependenciasDeTarea(tareaId), Times.Once);
        }

        [TestMethod]
        public void ObtenerDependenciasDeTarea_ConTareaSinDependencias_DevuelveListaVacia()
        {
            int tareaId = 2;
            var listaVacia = new List<TareaDTO>();

            _mockService.Setup(s => s.ObtenerDependenciasDeTarea(tareaId))
                .Returns(listaVacia);

            List<TareaDTO>? resultado = _controller.ObtenerDependenciasDeTarea(tareaId);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
            _mockService.Verify(s => s.ObtenerDependenciasDeTarea(tareaId), Times.Once);
        }

        [TestMethod]
        public void ObtenerDependenciasDeTarea_ConTareaInexistente_DevuelveNull()
        {
            int tareaId = 999;

            _mockService.Setup(s => s.ObtenerDependenciasDeTarea(tareaId))
                .Returns((List<TareaDTO>?)null);

            List<TareaDTO>? resultado = _controller.ObtenerDependenciasDeTarea(tareaId);

            Assert.IsNull(resultado);
            _mockService.Verify(s => s.ObtenerDependenciasDeTarea(tareaId), Times.Once);
        }

        [TestMethod]
        public void EliminarDependencia_LlamaService()
        {
            int tareaId = 10;
            int dependenciaId = 15;

            _controller.EliminarDependencia(tareaId, dependenciaId, 1);

            _mockService.Verify(s => s.EliminarDependencia(tareaId, dependenciaId, 1), Times.Once);
        }

        [TestMethod]
        public void ListarTareasDelUsuario_LlamaServiceYDevuelveLista()
        {
            int usuarioId = 7;
            int proyectoId = 3;
            var tareasEsperadas = new List<TareaDTO> { _dto1, _dto2 };

            _mockService.Setup(s => s.ListarTareasDelUsuario(usuarioId, proyectoId))
                .Returns(tareasEsperadas);

            List<TareaDTO> resultado = _controller.ListarTareasDelUsuario(usuarioId, proyectoId);

            CollectionAssert.AreEqual(tareasEsperadas, resultado);
            _mockService.Verify(s => s.ListarTareasDelUsuario(usuarioId, proyectoId), Times.Once);
        }

        [TestMethod]
        public void ListarTareasDelUsuario_ConUsuarioSinTareas_DevuelveListaVacia()
        {
            int usuarioId = 8;
            int proyectoId = 4;
            var listaVacia = new List<TareaDTO>();

            _mockService.Setup(s => s.ListarTareasDelUsuario(usuarioId, proyectoId))
                .Returns(listaVacia);

            List<TareaDTO> resultado = _controller.ListarTareasDelUsuario(usuarioId, proyectoId);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
            _mockService.Verify(s => s.ListarTareasDelUsuario(usuarioId, proyectoId), Times.Once);
        }

        [TestMethod]
        public void PuedeCambiarDeEstado_RetornaTrue_CuandoServiceDevuelveTrue()
        {
            int tareaId = 5;

            _mockService.Setup(s => s.PuedeCambiarDeEstado(tareaId))
                .Returns(true);

            bool resultado = _controller.PuedeCambiarDeEstado(tareaId);

            Assert.IsTrue(resultado);
            _mockService.Verify(s => s.PuedeCambiarDeEstado(tareaId), Times.Once);
        }

        [TestMethod]
        public void PuedeCambiarDeEstado_RetornaFalse_CuandoServiceDevuelveFalse()
        {
            int tareaId = 6;

            _mockService.Setup(s => s.PuedeCambiarDeEstado(tareaId))
                .Returns(false);

            bool resultado = _controller.PuedeCambiarDeEstado(tareaId);

            Assert.IsFalse(resultado);
            _mockService.Verify(s => s.PuedeCambiarDeEstado(tareaId), Times.Once);
        }

        [TestMethod]
        public void ObtenerTareasParaAgregarDependencia_ConTareasDisponibles_DevuelveLista()
        {
            int tareaSeleccionadaId = 1;
            int proyectoId = 5;
            var tareasDisponibles = new List<TareaDTO>
            {
                new TareaDTO { Id = 10, Titulo = "Tarea Disponible 1" },
                new TareaDTO { Id = 11, Titulo = "Tarea Disponible 2" }
            };

            _mockService.Setup(s => s.ObtenerTareasParaAgregarDependencia(tareaSeleccionadaId, proyectoId))
                .Returns(tareasDisponibles);

            List<TareaDTO>? resultado = _controller.ObtenerTareasParaAgregarDependencia(tareaSeleccionadaId, proyectoId);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.AreEqual("Tarea Disponible 1", resultado[0].Titulo);
            _mockService.Verify(s => s.ObtenerTareasParaAgregarDependencia(tareaSeleccionadaId, proyectoId), Times.Once);
        }

        [TestMethod]
        public void ObtenerTareasParaAgregarDependencia_SinTareasDisponibles_DevuelveListaVacia()
        {
            int tareaSeleccionadaId = 2;
            int proyectoId = 6;
            var listaVacia = new List<TareaDTO>();

            _mockService.Setup(s => s.ObtenerTareasParaAgregarDependencia(tareaSeleccionadaId, proyectoId))
                .Returns(listaVacia);

            List<TareaDTO>? resultado = _controller.ObtenerTareasParaAgregarDependencia(tareaSeleccionadaId, proyectoId);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
            _mockService.Verify(s => s.ObtenerTareasParaAgregarDependencia(tareaSeleccionadaId, proyectoId), Times.Once);
        }

        [TestMethod]
        public void ObtenerTareasParaAgregarDependencia_ConTareaInvalida_DevuelveNull()
        {
            int tareaSeleccionadaId = 999;
            int proyectoId = 7;

            _mockService.Setup(s => s.ObtenerTareasParaAgregarDependencia(tareaSeleccionadaId, proyectoId))
                .Returns((List<TareaDTO>?)null);

            List<TareaDTO>? resultado = _controller.ObtenerTareasParaAgregarDependencia(tareaSeleccionadaId, proyectoId);

            Assert.IsNull(resultado);
            _mockService.Verify(s => s.ObtenerTareasParaAgregarDependencia(tareaSeleccionadaId, proyectoId), Times.Once);
        }

        [TestMethod]
        public void PuedeAgregarDependencias_RetornaTrue_CuandoServiceDevuelveTrue()
        {
            int tareaId = 3;

            _mockService.Setup(s => s.PuedeAgregarDependencias(tareaId))
                .Returns(true);

            bool resultado = _controller.PuedeAgregarDependencias(tareaId);

            Assert.IsTrue(resultado);
            _mockService.Verify(s => s.PuedeAgregarDependencias(tareaId), Times.Once);
        }

        [TestMethod]
        public void PuedeAgregarDependencias_RetornaFalse_CuandoServiceDevuelveFalse()
        {
            int tareaId = 4;

            _mockService.Setup(s => s.PuedeAgregarDependencias(tareaId))
                .Returns(false);

            bool resultado = _controller.PuedeAgregarDependencias(tareaId);

            Assert.IsFalse(resultado);
            _mockService.Verify(s => s.PuedeAgregarDependencias(tareaId), Times.Once);
        }

        [TestMethod]
        public void TieneDependencias_RetornaTrue_CuandoServiceDevuelveTrue()
        {
            _mockService.Setup(s => s.TieneDependencias(_dto1))
                .Returns(true);

            bool resultado = _controller.TieneDependencias(_dto1);

            Assert.IsTrue(resultado);
            _mockService.Verify(s => s.TieneDependencias(_dto1), Times.Once);
        }

        [TestMethod]
        public void TieneDependencias_RetornaFalse_CuandoServiceDevuelveFalse()
        {
            _mockService.Setup(s => s.TieneDependencias(_dto2))
                .Returns(false);

            bool resultado = _controller.TieneDependencias(_dto2);

            Assert.IsFalse(resultado);
            _mockService.Verify(s => s.TieneDependencias(_dto2), Times.Once);
        }
        
        [TestMethod]
        public void PuedeEliminarTarea_RetornaTrue_CuandoServiceDevuelveTrue()
        {
            _mockService.Setup(s => s.PuedeEliminarTarea(_dto1))
                .Returns(true);

            bool resultado = _controller.PuedeEliminarTarea(_dto1);

            Assert.IsTrue(resultado);
            _mockService.Verify(s => s.PuedeEliminarTarea(_dto1), Times.Once);
        }

        [TestMethod]
        public void PuedeEliminarTarea_RetornaFalse_CuandoServiceDevuelveFalse()
        {
            _mockService.Setup(s => s.PuedeEliminarTarea(_dto2))
                .Returns(false);

            bool resultado = _controller.PuedeEliminarTarea(_dto2);

            Assert.IsFalse(resultado);
            _mockService.Verify(s => s.PuedeEliminarTarea(_dto2), Times.Once);
        }

        [TestMethod]
        public void ActualizarEstadoTarea_ConEstadoPendiente_LlamaService()
        {
            TipoEstadoTarea estado = TipoEstadoTarea.Pendiente;

            _controller.ActualizarEstadoTarea(estado, _dto1);

            _mockService.Verify(s => s.ActualizarEstadoTarea(estado, _dto1), Times.Once);
        }

        [TestMethod]
        public void ActualizarEstadoTarea_ConEstadoEjecutandose_LlamaService()
        {
            TipoEstadoTarea estado = TipoEstadoTarea.Ejecutandose;

            _controller.ActualizarEstadoTarea(estado, _dto2);

            _mockService.Verify(s => s.ActualizarEstadoTarea(estado, _dto2), Times.Once);
        }

        [TestMethod]
        public void ActualizarEstadoTarea_ConEstadoEfectuada_LlamaService()
        {
            TipoEstadoTarea estado = TipoEstadoTarea.Efectuada;

            _controller.ActualizarEstadoTarea(estado, _dto1);

            _mockService.Verify(s => s.ActualizarEstadoTarea(estado, _dto1), Times.Once);
        }

        [TestMethod]
        public void ActualizarEstadoTarea_ConEstadoBloqueada_LlamaService()
        {
            TipoEstadoTarea estado = TipoEstadoTarea.Bloqueada;

            _controller.ActualizarEstadoTarea(estado, _dto2);

            _mockService.Verify(s => s.ActualizarEstadoTarea(estado, _dto2), Times.Once);
        }
    }
}
