using Domain;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Tests
{
    [TestClass]
    public class AsignacionRecursoTareaDataAccess_Tests
    {
        private static readonly TimeSpan VALID_TIMESPAN = new TimeSpan(6, 5, 0, 0);
        private AsignacionRecursoTareaDataAccess repo;
        private Tarea tarea1;
        private Tarea tarea2;
        private Recurso recurso1;
        private Recurso recurso2;
        private AsignacionRecursoTarea asign1;
        private AsignacionRecursoTarea asign2;
        private AsignacionRecursoTarea asign3;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            SqlContext context = new SqlContext(options);
            repo = new AsignacionRecursoTareaDataAccess(context);
            tarea1 = new Tarea("Tara1", "descripcion",  DateTime.Now, VALID_TIMESPAN, false);
            tarea2 = new Tarea ("Tara2", "descripcion", DateTime.Now, VALID_TIMESPAN, false);
            recurso1 = new Recurso ("Recurso1", "Tipo", "Descripcion",  false, 8);
            recurso2 = new Recurso ("Recurso2", "Tipo", "Descripcion",  false, 8);

            asign1 = new AsignacionRecursoTarea (recurso1,  tarea1, 5);
            asign2 = new AsignacionRecursoTarea (recurso1,  tarea2, 3);
            asign3 = new AsignacionRecursoTarea (recurso2, tarea1, 8);
        }

        [TestMethod]
        public void AñadirAsignacionRecursoTareaCorrectamente()
        {
            repo.Add(asign1);
            List<AsignacionRecursoTarea> asignaciones = repo.GetAll();
            Assert.AreEqual(1, asignaciones.Count);
            AsignacionRecursoTarea actual = asignaciones[0];
            Assert.AreEqual(asign1.Recurso.Id,actual.Recurso.Id);
            Assert.AreEqual(asign1.Tarea.Id, actual.Tarea.Id);
            Assert.AreEqual(asign1.CantidadNecesaria, actual.CantidadNecesaria);
        }

        [TestMethod]
        public void EliminarAsignacionRecursoTarea()
        {
            repo.Add(asign1);

            Assert.IsTrue(repo.GetAll().Any(a =>
                a.Recurso.Id == asign1.Recurso.Id &&
                a.Tarea.Id == asign1.Tarea.Id &&
                a.CantidadNecesaria == asign1.CantidadNecesaria));
            repo.Remove(asign1);
            Assert.IsFalse(repo.GetAll().Any(a =>
                a.Recurso.Id == asign1.Recurso.Id &&
                a.Tarea.Id == asign1.Tarea.Id &&
                a.CantidadNecesaria == asign1.CantidadNecesaria
            ));
        }


        [TestMethod]
        public void ObtenerAsignacionPorID()
        {
            repo.Add(asign1);
            AsignacionRecursoTarea asignacion = repo.GetById(asign1.Id);
            Assert.IsNotNull(asignacion);
            Assert.AreEqual(asign1, asignacion);
        }

        [TestMethod]
        public void ObtenerTodasLasAsignaciones()
        {
            repo.Add(asign1);
            repo.Add(asign2);
            List<AsignacionRecursoTarea> asignaciones = repo.GetAll();
            Assert.AreEqual(2, asignaciones.Count);
        }

        [TestMethod]
        public void ObtenerAsignacionesPorTarea()
        {
            repo.Add(asign1);
            repo.Add(asign3);
            List<AsignacionRecursoTarea> asignacionesDeTarea = repo.GetByTarea(tarea1.Id);
            Assert.AreEqual(2, asignacionesDeTarea.Count);
            Assert.IsTrue(asignacionesDeTarea.All(a => a.Tarea.Id == tarea1.Id));
        }

        [TestMethod]
        public void ObtenerAsignacionesPorRecurso()
        {
            repo.Add(asign1);
            repo.Add(asign2);
            List<AsignacionRecursoTarea> asignacionesDeRecurso = repo.GetByRecurso(recurso1.Id);
            Assert.AreEqual(2, asignacionesDeRecurso.Count);
            Assert.IsTrue(asignacionesDeRecurso.All(a => a.Recurso.Id == recurso1.Id));
        }

        [TestMethod]
        public void ObtenerCantidadDeRecursoNecesaria()
        {
            repo.Add(asign1);
            int cantidad = repo.CantidadDelRecurso(asign1);
            Assert.AreEqual(5, cantidad);
        }
        
        [TestMethod]
        public void ObtenerAsignacionPorRecursoYTarea_DevuelveAsignacionCorrecta()
        {
            repo.Add(asign1);
            AsignacionRecursoTarea asignacion = repo.GetByRecursoYTarea(recurso1.Id, tarea1.Id);
            Assert.IsNotNull(asignacion);
            Assert.AreEqual(asign1, asignacion);
        }

        [TestMethod]
        public void ObtenerAsignacionPorRecursoYTarea_NoEncuentraAsignacionYDevuelveNull()
        {
            repo.Add(asign1);
            AsignacionRecursoTarea asignacion = repo.GetByRecursoYTarea(recurso2.Id, tarea2.Id);
            Assert.IsNull(asignacion);
        }

        [TestMethod]
        public void ObtenerAsignacionPorRecursoYTarea_EncuentraAsignacionConOtroRecursoYTarea()
        {
            repo.Add(asign1);
            repo.Add(asign2);
            repo.Add(asign3);

            AsignacionRecursoTarea asignacion = repo.GetByRecursoYTarea(recurso2.Id, tarea1.Id);
            Assert.IsNotNull(asignacion);
            Assert.AreEqual(asign3.Recurso.Id, asignacion.Recurso.Id);
            Assert.AreEqual(asign3.Tarea.Id,   asignacion.Tarea.Id);
        }
    }
}