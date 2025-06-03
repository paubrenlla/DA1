using DTOs;
using Domain;
using Domain.Enums;
using DataAccess;
using IDataAcces;
using Services;

namespace Services_Tests
{
    [TestClass]
    public class TareaServiceTests
    {
        private IDataAccessTarea _repoTareas;
        private IDataAccessProyecto _repoProyectos;
        private IDataAccessUsuario _repoUsuarios;
        private TareaService _service;

        private Proyecto _proyectoEjemplo;
        private Tarea _tareaEjemplo;
        private Usuario _usuarioEjemplo;

        [TestInitialize]
        public void SetUp()
        {
            _repoTareas = new TareaDataAccess();
            _repoProyectos = new ProyectoDataAccess();
            _repoUsuarios = new UsuarioDataAccess();

            _service = new TareaService(_repoTareas, _repoProyectos, _repoUsuarios);

            _proyectoEjemplo = new Proyecto(
                "Proyecto Test",
                "Descripción Test",
                DateTime.Today.AddDays(1));
            _repoProyectos.Add(_proyectoEjemplo);

            _usuarioEjemplo = new Usuario(
                "user@test.com",
                "User",
                "Test",
                "Password1!",
                DateTime.Today.AddYears(-20));
            _repoUsuarios.Add(_usuarioEjemplo);

            _tareaEjemplo = new Tarea(
                "Tarea Test",
                "Descripción Tarea",
                DateTime.Today.AddHours(9),
                TimeSpan.FromHours(4),
                esCritica: false);
            _tareaEjemplo.Proyecto = _proyectoEjemplo;
            _proyectoEjemplo.TareasAsociadas.Add(_tareaEjemplo);
            _repoTareas.Add(_tareaEjemplo);
        }

        [TestMethod]
        public void BuscarTareaPorIdDevuelveDTO()
        {
            TareaDTO resultado = _service.BuscarTareaPorId(_tareaEjemplo.Id);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(_tareaEjemplo.Id, resultado.Id);
            Assert.AreEqual(_tareaEjemplo.Titulo, resultado.Titulo);
            Assert.AreEqual(_tareaEjemplo.Descripcion, resultado.Descripcion);
        }

        [TestMethod]
        public void ListarTareasPorProyectoDevuelveTodas()
        {
            Tarea tarea2 = new Tarea(
                "Tarea 2",
                "Desc 2",
                DateTime.Today.AddDays(2),
                TimeSpan.FromHours(3),
                esCritica: true);
            tarea2.Proyecto = _proyectoEjemplo;
            _proyectoEjemplo.TareasAsociadas.Add(tarea2);
            _repoTareas.Add(tarea2);

            List<TareaDTO> lista = _service.ListarTareasPorProyecto(_proyectoEjemplo.Id);

            Assert.IsNotNull(lista);
            Assert.AreEqual(2, lista.Count);

            Assert.IsTrue(lista.Exists(t => t.Id == _tareaEjemplo.Id));
            Assert.IsTrue(lista.Exists(t => t.Id == tarea2.Id));
        }


        [TestMethod]
        public void CrearTareaAgregaCorrectamente()
        {
            TareaDTO dtoNueva = new TareaDTO
            {
                Titulo = "Nueva Tarea",
                Descripcion = "Descripción nueva",
                FechaInicio = DateTime.Today.AddDays(3),
                Duracion = TimeSpan.FromHours(4)
            };

            TareaDTO creada = _service.CrearTarea(_proyectoEjemplo.Id, dtoNueva);

            Assert.IsNotNull(creada);
            Assert.AreEqual(dtoNueva.Titulo, creada.Titulo);
            Assert.AreEqual(dtoNueva.Descripcion, creada.Descripcion);

            List<TareaDTO> tareasDelProyecto = _service.ListarTareasPorProyecto(_proyectoEjemplo.Id);
            bool contiene = tareasDelProyecto.Any(t => t.Id == creada.Id);
            Assert.IsTrue(contiene);
        }

        [TestMethod]
        public void ModificarTareaCambiaDatosCorrectamente()
        {
            TareaDTO dtoModificada = new TareaDTO
            {
                Titulo = "Modificado",
                Descripcion = "Nueva descripción",
                FechaInicio = DateTime.Today.AddDays(5),
                Duracion = TimeSpan.FromHours(8)
            };

            _service.ModificarTarea(_tareaEjemplo.Id, dtoModificada);

            Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
            Assert.AreEqual(dtoModificada.Titulo, tareaGuardada.Titulo);
            Assert.AreEqual(dtoModificada.Descripcion, tareaGuardada.Descripcion);
        }

        [TestMethod]
        public void MarcarComoEjecutandoseCambiaEstado()
        {
            _service.MarcarComoEjecutandose(_tareaEjemplo.Id);

            Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
            Assert.IsNotNull(tareaGuardada.EstadoActual);
            Assert.AreEqual(TipoEstadoTarea.Ejecutandose, tareaGuardada.EstadoActual.Valor);
        }

        [TestMethod]
        public void MarcarComoCompletadaCambiaBien()
        {
            _service.MarcarComoEjecutandose(_tareaEjemplo.Id);
            _service.MarcarComoCompletada(_tareaEjemplo.Id);

            Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
            Assert.IsNotNull(tareaGuardada.EstadoActual);
            Assert.AreEqual(TipoEstadoTarea.Efectuada, tareaGuardada.EstadoActual.Valor);
        }

        [TestMethod]
        public void AgregarDependenciaAgregaCorrectamente()
        {
            Tarea tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddHours(1),TimeSpan.FromHours(3), esCritica: true);
            tarea2.Proyecto = _proyectoEjemplo;
            _proyectoEjemplo.TareasAsociadas.Add(tarea2);
            _repoTareas.Add(tarea2);

            _service.AgregarDependencia(_tareaEjemplo.Id, tarea2.Id);

            Tarea tareaOriginal = _repoTareas.GetById(_tareaEjemplo.Id);
            Tarea tareaDependencia = _repoTareas.GetById(tarea2.Id);

            Assert.IsTrue(tareaOriginal.TareasDependencia.Contains(tareaDependencia));
            Assert.IsTrue(tareaDependencia.TareasSucesoras.Contains(tareaOriginal));
        }

        [TestMethod]
        public void AgregarUsuarioAgregaUsuarioCorrectamente()
        {
            _service.AgregarUsuario(_tareaEjemplo.Id, _usuarioEjemplo.Id);

            Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
            Assert.IsNotNull(tareaGuardada);
            Assert.IsTrue(tareaGuardada.UsuariosAsignados.Contains(_usuarioEjemplo));
        }
    }
}
