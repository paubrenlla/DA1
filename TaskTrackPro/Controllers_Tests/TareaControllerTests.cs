using Domain;
using Domain.Enums;
using IDataAcces;
using Repositorios;
using Controllers;
using DTOs;

namespace Controllers_Tests;

[TestClass]
    public class TareaControllerTests
    {
        private TareaController _tareaController;
        private IDataAccessTarea _repoTareas;
        private IDataAccessProyecto _repoProyectos;
        private IDataAccessUsuario _repoUsuarios;

        private Proyecto _proyectoEjemplo;
        private Tarea _tareaEjemplo;
        private Usuario _usuarioEjemplo;

        [TestInitialize]
        public void SetUp()
        {
            _repoTareas = new TareaDataAccess();
            _repoProyectos = new ProyectoDataAccess();
            _repoUsuarios = new UsuarioDataAccess();

            _tareaController = new TareaController(_repoTareas, _repoProyectos, _repoUsuarios);

            _proyectoEjemplo = new Proyecto("Proyecto Test", "Descripción Test", DateTime.Today.AddDays(1));
            _repoProyectos.Add(_proyectoEjemplo);

            _usuarioEjemplo = new Usuario("user@test.com", "User", "Test", "Contraseña1!", DateTime.Today.AddYears(-20));
            _repoUsuarios.Add(_usuarioEjemplo);

            _tareaEjemplo = new Tarea("Tarea Test", "Descripción Tarea", DateTime.Today.AddHours(9), TimeSpan.FromHours(4), false);
            _tareaEjemplo.Proyecto = _proyectoEjemplo;
            _proyectoEjemplo.TareasAsociadas.Add(_tareaEjemplo);
            _repoTareas.Add(_tareaEjemplo);
        }

        [TestMethod]
        public void BuscarTareaPorIdDevuelveDTO()
        {
            TareaDTO dto = _tareaController.BuscarTareaPorId(_tareaEjemplo.Id);

            Assert.IsNotNull(dto);
            Assert.AreEqual(_tareaEjemplo.Id, dto.Id);
          
        }
        
        [TestMethod]
        public void ListarTareasPorProyectoDevuelveTodas()
        {
            Tarea _tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddDays(2), TimeSpan.FromHours(3), true);
            _tarea2.Proyecto = _proyectoEjemplo;
            _proyectoEjemplo.TareasAsociadas.Add(_tarea2);
            _repoTareas.Add(_tarea2);
            List<TareaDTO> lista = _tareaController.ListarTareasPorProyecto(_proyectoEjemplo.Id);

            Assert.IsNotNull(lista);
            Assert.AreEqual(2, lista.Count);
            Assert.AreEqual(_tareaEjemplo.Id, lista[0].Id);
            Assert.AreEqual(_tarea2.Id, lista[1].Id);
        }
        
        [TestMethod]
        public void CrearTareaAgregaCorrectamente()
        {
            var dto = new TareaDTO
            {
                Titulo = "Nueva Tarea",
                Descripcion = "Descripción nueva",
                FechaInicio = DateTime.Today.AddDays(3),
                Duracion = TimeSpan.FromHours(4)
            };
            TareaDTO tareaCreada = _tareaController.CrearTarea(_proyectoEjemplo.Id, dto);

            Assert.IsNotNull(tareaCreada);
            Assert.AreEqual(dto.Titulo, tareaCreada.Titulo);
            List<TareaDTO> tareasDelProyecto = _tareaController.ListarTareasPorProyecto(_proyectoEjemplo.Id);
            Assert.IsTrue(tareasDelProyecto.Any(t => t.Id == tareaCreada.Id));
        }
        
        [TestMethod]
        public void ModificarTareaCambiaDatosCorrectamente()
        {
            var dto = new TareaDTO
            {
                Titulo = "Modificado",
                Descripcion = "Nueva descripción",
                FechaInicio = DateTime.Today.AddDays(5),
                Duracion = TimeSpan.FromHours(8)
            };

            _tareaController.ModificarTarea(_tareaEjemplo.Id, dto);

            var modificada = _repoTareas.GetById(_tareaEjemplo.Id);
            Assert.AreEqual(dto.Titulo, modificada.Titulo);
            Assert.AreEqual(dto.Descripcion, modificada.Descripcion);
            Assert.AreEqual(dto.FechaInicio, modificada.FechaInicio);
            Assert.AreEqual(dto.Duracion, modificada.Duracion);
        }
        
        [TestMethod]
        public void MarcarComoCompletadaCambiaBien()
        {
            _tareaController.MarcarComoEjecutandose(_tareaEjemplo.Id);
            
            _tareaController.MarcarComoCompletada(_tareaEjemplo.Id);
            
            Tarea tarea = _repoTareas.GetById(_tareaEjemplo.Id);
            Assert.IsNotNull(tarea);
            Assert.AreEqual(TipoEstadoTarea.Efectuada, tarea.EstadoActual.Valor);
        }
        
        [TestMethod]
        public void AgregarDependenciaAgregaCorrectamente()
        {
            Tarea _tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddDays(2), TimeSpan.FromHours(3), true);
            _tarea2.Proyecto = _proyectoEjemplo;
            _proyectoEjemplo.TareasAsociadas.Add(_tarea2);
            _repoTareas.Add(_tarea2);
            _tareaController.AgregarDependencia(_tareaEjemplo.Id, _tarea2.Id);            
            Tarea tarea1 = _repoTareas.GetById(_tareaEjemplo.Id);
            Tarea tarea2 = _repoTareas.GetById(_tarea2.Id);
            Assert.IsTrue(tarea1.TareasDependencia.Contains(tarea2));
            Assert.IsTrue(tarea2.TareasSucesoras.Contains(tarea1));
        }
        
        [TestMethod]
        public void AgregarUsuario_AgregaUsuarioCorrectamente()
        {
            _tareaController.AgregarUsuario(_tareaEjemplo.Id, _usuarioEjemplo.Id);
            var tarea = _repoTareas.GetById(_tareaEjemplo.Id);
            Assert.IsNotNull(tarea);
            Assert.IsTrue(tarea.UsuariosAsignados.Contains(_usuarioEjemplo));
        }

    }

