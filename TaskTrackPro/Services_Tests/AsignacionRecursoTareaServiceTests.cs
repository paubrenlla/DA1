using DataAccess;
using Domain;
using Domain.Enums;
using DTOs;
using IDataAcces;
using Services;

namespace Services_Tests
{
    [TestClass]
    public class AsignacionRecursoTareaServiceTests
    {
        private AsignacionRecursoTareaService _service;
        private IDataAccessRecurso _repoRecursos;
        private IDataAccessTarea _repoTareas;
        private IDataAccessAsignacionRecursoTarea _repoAsignaciones;
        private Recurso _recurso1;
        private Recurso _recurso2;
        private Tarea _tarea1;
        private Tarea _tarea2;

        private static readonly TimeSpan VALID_TIMESPAN = new TimeSpan(6, 5, 0, 0);

        [TestInitialize]
        public void SetUp()
        {
            _repoRecursos = new RecursoDataAccess();
            _repoTareas = new TareaDataAccess();
            _repoAsignaciones = new AsignacionRecursoTareaDataAccess();
            _service = new AsignacionRecursoTareaService(_repoRecursos, _repoTareas, _repoAsignaciones);

            _recurso1 = new Recurso("Recurso1", "Tipo1", "Desc1", false, 10);
            _recurso2 = new Recurso("Recurso2", "Tipo2", "Desc2", false, 5);
            _repoRecursos.Add(_recurso1);
            _repoRecursos.Add(_recurso2);

            _tarea1 = new Tarea("Tarea1", "DescTarea1", DateTime.Today, VALID_TIMESPAN, true);
            _tarea2 = new Tarea("Tarea2", "DescTarea2", DateTime.Today, VALID_TIMESPAN, false);
            _repoTareas.Add(_tarea1);
            _repoTareas.Add(_tarea2);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetById_LanzaExcepcionSiAsignacionNoExiste()
        {
            _service.GetById(999);
        }
        
        [TestMethod]
        public void CrearProyecto_ConRecursoCompartido_NoDebeLanzarError()
        {
            AsignacionRecursoTareaDTO dto = new AsignacionRecursoTareaDTO
            {
                Recurso = new RecursoDTO 
                { 
                    Nombre = "Recurso Compartido", 
                    Tipo = "Recurso", 
                    Descripcion = "Prueba", 
                    SePuedeCompartir = true, 
                    CantidadDelRecurso = 10 
                },
                Tarea = new TareaDTO
                {
                    Titulo = "TareaNueva", 
                    FechaInicio = DateTime.Today, 
                    Duracion = VALID_TIMESPAN, 
                    EsCritica = false, 
                    Descripcion = "Descripcion"
                },
                Cantidad = 2
            };

            AsignacionRecursoTareaDTO resultado = _service.CrearProyecto(dto);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(dto.Recurso.Nombre, resultado.Recurso.Nombre);
        }
        
        [TestMethod]
        public void EliminarRecursosDeTarea_EliminaCorrectamenteLosRecursos()
        {
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea1, 2));
            //_repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso2, _tarea1, 3));

            _service.EliminarRecursosDeTarea(_tarea1.Id);

            List<AsignacionRecursoTarea> asignacionesRestantes = _repoAsignaciones.GetByTarea(_tarea1);
            Assert.AreEqual(0, asignacionesRestantes.Count);
        }
        
        [TestMethod]
        public void GetByIdDevuelveDTOBien()
        {
            AsignacionRecursoTarea asignacion = new AsignacionRecursoTarea(_recurso1, _tarea1, 2);
            _repoAsignaciones.Add(asignacion);

            AsignacionRecursoTareaDTO dto = _service.GetById(asignacion.Id);

            Assert.AreEqual(asignacion.Id, dto.Id);
            Assert.AreEqual(asignacion.Recurso.Nombre, dto.Recurso.Nombre);
            Assert.AreEqual(asignacion.Tarea.Titulo, dto.Tarea.Titulo);
        }

        [TestMethod]
        public void GetAllDevuelveListaDeDTOs()
        {
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea1, 2));
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso2, _tarea2, 3));

            List<AsignacionRecursoTareaDTO> list = _service.GetAll();
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void CrearAsignacionCreaLaAsignacionCorrectamenteYLaGuarda()
        {
            AsignacionRecursoTareaDTO dtoIn = new AsignacionRecursoTareaDTO
            {
                Recurso = new RecursoDTO
                {
                    Nombre = "Nuevo", 
                    Tipo = "TipoNuevo", 
                    Descripcion = "DescNuevo", 
                    SePuedeCompartir = true, 
                    CantidadDelRecurso = 10
                },
                
                Tarea = new TareaDTO
                {
                    Titulo = "NuevaTarea", 
                    Descripcion = "DescTareaNueva", 
                    FechaInicio = DateTime.Today, 
                    Duracion = VALID_TIMESPAN, 
                    EsCritica = true
                },
                Cantidad = 4
            };

            AsignacionRecursoTareaDTO asignacionCreadaDto = _service.CrearProyecto(dtoIn);
            Assert.IsNotNull(asignacionCreadaDto);
            Assert.AreEqual(dtoIn.Recurso.Nombre, asignacionCreadaDto.Recurso.Nombre);
            Assert.AreEqual(dtoIn.Tarea.Titulo, asignacionCreadaDto.Tarea.Titulo);
        }

        [TestMethod]
        public void ModificarAsignacion_ModificaCorrectamente()
        {
            AsignacionRecursoTarea asignacion = new AsignacionRecursoTarea(_recurso1, _tarea1, 3);
            _repoAsignaciones.Add(asignacion);

            AsignacionRecursoTareaDTO dto = new AsignacionRecursoTareaDTO { Id = asignacion.Id, Cantidad = 5 };
            _service.ModificarAsignacion(dto);

            AsignacionRecursoTarea asignacionModificada = _repoAsignaciones.GetById(asignacion.Id);
            Assert.AreEqual(5, asignacionModificada.CantidadNecesaria);
        }

        [TestMethod]
        public void EliminarRecursoDeTarea_EliminaAsignacionCorrectamente()
        {
            AsignacionRecursoTarea asignacion = new AsignacionRecursoTarea(_recurso1, _tarea1, 2);
            _repoAsignaciones.Add(asignacion);

            Assert.IsNotNull(_repoAsignaciones.GetById(asignacion.Id));
            _service.EliminarRecursoDeTarea(asignacion.Id);
            Assert.IsNull(_repoAsignaciones.GetById(asignacion.Id));
        }

        [TestMethod]
        public void RecursosDeLaTarea_DevuelveListaRecursosDTO()
        {
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea1, 2));
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso2, _tarea1, 3));

            List<RecursoDTO> recursosDTO = _service.RecursosDeLaTarea(_tarea1.Id);
            Assert.AreEqual(2, recursosDTO.Count);
        }

        [TestMethod]
        public void RecursoEsExclusivo_DevuelveTrueSiMasDeUnaTareaUsaElRecurso()
        {
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea1, 2));
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea2, 3));

            bool resultado = _service.RecursoEsExclusivo(_recurso1.Id);
            Assert.IsTrue(resultado);
        }
        
        [TestMethod]
        public void RecursoEsExclusivo_DevuelveFalseSiSoloUnaTareaLoUsa()
        {
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea1, 2));

            Assert.IsFalse(_service.RecursoEsExclusivo(_recurso1.Id));
        }
        
        [TestMethod]
        public void ActualizarEstadoDeTareasConRecurso_CambiaElEstadoDeLasTareas()
        {
            Recurso recurso = new Recurso("Recurso", "Tipo", "Descripción", false, 1);
            Tarea tarea1 = new Tarea("Tarea 1", "Desc", DateTime.Now, VALID_TIMESPAN, false);
            Tarea tarea2 = new Tarea("Tarea 2", "Desc", DateTime.Now, VALID_TIMESPAN, false);

            _repoRecursos.Add(recurso);
            _repoTareas.Add(tarea1);
            _repoTareas.Add(tarea2);
            
            AsignacionRecursoTarea asignacion1 = new AsignacionRecursoTarea(recurso, tarea1, 1);
            AsignacionRecursoTarea asignacion2 = new AsignacionRecursoTarea(recurso, tarea2, 2);
            
            _repoAsignaciones.Add(asignacion1);
            _repoAsignaciones.Add(asignacion2);

            recurso.ConsumirRecurso(1);
            
            _service.ActualizarEstadoDeTareasConRecurso(recurso.Id);

            Assert.AreNotEqual(TipoEstadoTarea.Pendiente, tarea1.EstadoActual.Valor);
            Assert.AreNotEqual(TipoEstadoTarea.Pendiente, tarea2.EstadoActual.Valor);
        }

    }
}