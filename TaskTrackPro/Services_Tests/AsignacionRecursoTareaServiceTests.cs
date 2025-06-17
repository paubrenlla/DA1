using DataAccess;
using Domain;
using Domain.Enums;
using DTOs;
using IDataAcces;
using Microsoft.EntityFrameworkCore;
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
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new SqlContext(options);
            _repoRecursos = new RecursoDataAccess(context);
            _repoTareas = new TareaDataAccess(context);
            _repoAsignaciones = new AsignacionRecursoTareaDataAccess(context);
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
        public void CrearAsignacionRecursoTarea_CreaNuevaSiNoExiste()
        {
            AsignacionRecursoTareaDTO dto = new AsignacionRecursoTareaDTO
            {
                Recurso = Convertidor.ARecursoDTO(_recurso2),
                Tarea = Convertidor.ATareaDTO(_tarea1),
                Cantidad = 5
            };

            AsignacionRecursoTareaDTO resultado = _service.CrearAsignacionRecursoTarea(dto);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(dto.Cantidad, resultado.Cantidad);
        }

        [TestMethod]
        public void CrearAsignacionRecursoTarea_ActualizaCantidadSiYaExiste()
        {
            AsignacionRecursoTarea aAsignar = new AsignacionRecursoTarea(_recurso1, _tarea1, 3);
            _repoAsignaciones.Add(aAsignar);

            AsignacionRecursoTareaDTO dto = new AsignacionRecursoTareaDTO
            {
                Recurso = Convertidor.ARecursoDTO(_recurso1),
                Tarea = Convertidor.ATareaDTO(_tarea1),
                Cantidad = 4
            };

            AsignacionRecursoTareaDTO resultado = _service.CrearAsignacionRecursoTarea(dto);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(7, resultado.Cantidad);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CrearAsignacionRecursoTarea_LanzaExcepcionSiCantidadSuperaDisponibilidad()
        {
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea1, 8));

            AsignacionRecursoTareaDTO dto = new AsignacionRecursoTareaDTO
            {
                Recurso = Convertidor.ARecursoDTO(_recurso1),
                Tarea = Convertidor.ATareaDTO(_tarea1),
                Cantidad = 5 // Esto haría que la cantidad total fuera 8 + 5 = 13, mayor que la disponible (10)
            };

            _service.CrearAsignacionRecursoTarea(dto);
        }
        
        [TestMethod]
        public void EliminarRecursosDeTarea_EliminaCorrectamenteLosRecursos()
        {
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea1, 2));

            _service.EliminarRecursosDeTarea(_tarea1.Id);

            List<AsignacionRecursoTarea> asignacionesRestantes = _repoAsignaciones.GetByTarea(_tarea1.Id);
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
                Recurso = Convertidor.ARecursoDTO(_recurso1),
                Tarea = Convertidor.ATareaDTO(_tarea1),
                Cantidad = 4
            };

            AsignacionRecursoTareaDTO asignacionCreadaDto = _service.CrearAsignacionRecursoTarea(dtoIn);
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
            
            _service.EliminarRecursoDeTarea(asignacion.Tarea.Id, asignacion.Recurso.Id);
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
        public void RecursoEsExclusivo_DevuelveFalseSiMasDeUnaTareaUsaElRecurso()
        {
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea1, 2));
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea2, 3));

            bool resultado = _service.RecursoEsExclusivo(_recurso1.Id);
            Assert.IsFalse(resultado);
        }
        
        [TestMethod]
        public void RecursoEsExclusivo_DevuelveTrueSiSoloUnaTareaLoUsa()
        {
            _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea1, 2));

            Assert.IsTrue(_service.RecursoEsExclusivo(_recurso1.Id));
        }
        
        [TestMethod]
        public void ActualizarEstadoDeTareasConRecurso_CambiaElEstadoDeLasTareas()
        {
            Recurso recurso = new Recurso("Recurso", "Tipo", "Descripción", false, 1);
            _repoRecursos.Add(recurso);

            Tarea tarea1 = new Tarea("Tarea 1", "Desc", DateTime.Now, VALID_TIMESPAN, false);
            Tarea tarea2 = new Tarea("Tarea 2", "Desc", DateTime.Now, VALID_TIMESPAN, false);
            _repoTareas.Add(tarea1);
            _repoTareas.Add(tarea2);

            AsignacionRecursoTarea asign1 = new AsignacionRecursoTarea(recurso, tarea1, 1);
            AsignacionRecursoTarea asign2 = new AsignacionRecursoTarea(recurso, tarea2, 1);
            _repoAsignaciones.Add(asign1);
            _repoAsignaciones.Add(asign2);

            recurso.ConsumirRecurso(1);

            _service.ActualizarEstadoDeTareasConRecurso(recurso.Id);

            Tarea tarea1Actualizada = _repoTareas.GetById(tarea1.Id);
            Tarea tarea2Actualizada = _repoTareas.GetById(tarea2.Id);

            Assert.AreNotEqual(TipoEstadoTarea.Pendiente, tarea1Actualizada.EstadoActual.Valor);

            Assert.AreNotEqual(TipoEstadoTarea.Pendiente, tarea2Actualizada.EstadoActual.Valor);
        }

        
        [TestMethod]
        public void GetAsignacionesDeTarea_DevuelveListaVacia_CuandoTareaNoTieneAsignaciones()
        {
            List<AsignacionRecursoTareaDTO> resultado = _service.GetAsignacionesDeTarea(_tarea1.Id);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
        }

        [TestMethod]
        public void GetAsignacionesDeTarea_DevuelveUnaAsignacion_CuandoTareaTieneUnaAsignacion()
        {
            AsignacionRecursoTarea asignacion = new AsignacionRecursoTarea(_recurso1, _tarea1, 3);
            _repoAsignaciones.Add(asignacion);

            List<AsignacionRecursoTareaDTO> resultado = _service.GetAsignacionesDeTarea(_tarea1.Id);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual(asignacion.Id, resultado[0].Id);
            Assert.AreEqual(_recurso1.Nombre, resultado[0].Recurso.Nombre);
            Assert.AreEqual(3, resultado[0].Cantidad);
        }

        [TestMethod]
        public void GetAsignacionesDeTarea_DevuelveMultiplesAsignaciones_CuandoTareaTieneVariasAsignaciones()
        {
            AsignacionRecursoTarea asignacion1 = new AsignacionRecursoTarea(_recurso1, _tarea1, 2);
            AsignacionRecursoTarea asignacion2 = new AsignacionRecursoTarea(_recurso2, _tarea1, 4);
            _repoAsignaciones.Add(asignacion1);
            _repoAsignaciones.Add(asignacion2);

            List<AsignacionRecursoTareaDTO> resultado = _service.GetAsignacionesDeTarea(_tarea1.Id);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.IsTrue(resultado.Any(a => a.Recurso.Nombre == _recurso1.Nombre && a.Cantidad == 2));
            Assert.IsTrue(resultado.Any(a => a.Recurso.Nombre == _recurso2.Nombre && a.Cantidad == 4));
        }

        [TestMethod]
        public void GetAsignacionesDeTarea_NoDevuelveAsignacionesDeOtrasTareas()
        {
            AsignacionRecursoTarea asignacionTarea1 = new AsignacionRecursoTarea(_recurso1, _tarea1, 2);
            AsignacionRecursoTarea asignacionTarea2 = new AsignacionRecursoTarea(_recurso2, _tarea2, 3);
            _repoAsignaciones.Add(asignacionTarea1);
            _repoAsignaciones.Add(asignacionTarea2);

            List<AsignacionRecursoTareaDTO> resultado = _service.GetAsignacionesDeTarea(_tarea1.Id);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual(_recurso1.Nombre, resultado[0].Recurso.Nombre);
            Assert.AreEqual(_tarea1.Titulo, resultado[0].Tarea.Titulo);
        }

        [TestMethod]
        public void ObtenerAsignacionesRecursoEnFecha_DevuelveAsignacionesEnFechaExacta()
        {
            DateTime inicio = new DateTime(2025, 6, 15);
            DateTime fin = inicio.AddDays(2);

            Tarea tarea = new Tarea("Tarea", "desc", inicio, TimeSpan.FromDays(2), false);
            tarea.EarlyFinish = fin;

            _repoTareas.Add(tarea);

            AsignacionRecursoTarea asignacion = new AsignacionRecursoTarea(_recurso1, tarea, 2);
            _repoAsignaciones.Add(asignacion);

            var resultado = _service.ObtenerAsignacionesRecursoEnFecha(_recurso1.Id, inicio.AddDays(1)); // 16 de junio

            Assert.AreEqual(1, resultado.Count);
        }

        [TestMethod]
        public void ObtenerAsignacionesRecursoEnFecha_DevuelveVaciaSiFechaFueraDeRango()
        {
            DateTime fechaTarea = new DateTime(2025, 6, 10);
            TimeSpan duracion = new TimeSpan(2, 0, 0, 0); // Tarea del 10 al 12

            Tarea tarea = new Tarea("Tarea fuera de rango", "Desc", fechaTarea, duracion, false);
            _repoTareas.Add(tarea);

            AsignacionRecursoTarea asignacion = new AsignacionRecursoTarea(_recurso1, tarea, 1);
            _repoAsignaciones.Add(asignacion);

            DateTime fechaConsulta = new DateTime(2025, 6, 15); // Fuera del rango
            List<AsignacionRecursoTareaDTO> resultado = _service.ObtenerAsignacionesRecursoEnFecha(_recurso1.Id, fechaConsulta);

            Assert.AreEqual(0, resultado.Count);
        }

        [TestMethod]
        public void ObtenerAsignacionesRecursoEnFecha_DevuelveTodasSiNoSeFiltraPorFecha()
        {
            Tarea tarea1 = new Tarea("Tarea 1", "Desc", DateTime.Today, VALID_TIMESPAN, false);
            Tarea tarea2 = new Tarea("Tarea 2", "Desc", DateTime.Today.AddDays(5), VALID_TIMESPAN, false);
            _repoTareas.Add(tarea1);
            _repoTareas.Add(tarea2);

            AsignacionRecursoTarea asign1 = new AsignacionRecursoTarea(_recurso1, tarea1, 2);
            AsignacionRecursoTarea asign2 = new AsignacionRecursoTarea(_recurso1, tarea2, 3);
            _repoAsignaciones.Add(asign1);
            _repoAsignaciones.Add(asign2);

            List<AsignacionRecursoTareaDTO> resultado = _service.ObtenerAsignacionesRecursoEnFecha(_recurso1.Id, null);

            Assert.AreEqual(2, resultado.Count);
            Assert.IsTrue(resultado.Any(a => a.Id == asign1.Id));
            Assert.IsTrue(resultado.Any(a => a.Id == asign2.Id));
        }

        [TestMethod]
        public void ObtenerAsignacionesRecursoEnFecha_NoDevuelveAsignacionesDeOtroRecurso()
        {
            Tarea tarea = new Tarea("Tarea", "Desc", DateTime.Today, VALID_TIMESPAN, false);
            _repoTareas.Add(tarea);

            AsignacionRecursoTarea asignacion = new AsignacionRecursoTarea(_recurso2, tarea, 1);
            _repoAsignaciones.Add(asignacion);

            List<AsignacionRecursoTareaDTO> resultado = _service.ObtenerAsignacionesRecursoEnFecha(_recurso1.Id, DateTime.Today);

            Assert.AreEqual(0, resultado.Count);
        }

    }
}