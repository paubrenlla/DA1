using DataAccess;
using Domain;
using IDataAcces;
using Moq;
using Services.Observers;

namespace Services_Tests.Observers_Tests;

[TestClass]
public class ActualizadorEstadoTareasTests
{
    private static readonly TimeSpan VALID_TIMESPAN = TimeSpan.FromDays(2);

    [TestMethod]
    public void ActualizarTareasDeRecurso_LlamaActualizarEstadoEnTodasLasTareasAsignadas()
    {
        Recurso recurso = new Recurso("Recurso 1", "Tipo", "Descripción", true, 10);

        Tarea tarea1 = new Tarea("Tarea 1", "Descripcion", DateTime.Now, VALID_TIMESPAN, false);
        Tarea tarea2 = new Tarea("Tarea 2", "Descripcion", DateTime.Now, VALID_TIMESPAN, false);

        List<AsignacionRecursoTarea> asignaciones = new List<AsignacionRecursoTarea>
        {
            new AsignacionRecursoTarea(recurso, tarea1, 2),
            new AsignacionRecursoTarea(recurso, tarea2, 1)
        };

        Mock<IDataAccessTarea> mockTareaRepo = new Mock<IDataAccessTarea>();
        Mock<IDataAccessAsignacionRecursoTarea> mockAsignacionRepo = new Mock<IDataAccessAsignacionRecursoTarea>();
        mockAsignacionRepo.Setup(r => r.GetByRecurso(recurso.Id)).Returns(asignaciones);

        ActualizadorEstadoTareas actualizador = new ActualizadorEstadoTareas(mockAsignacionRepo.Object, mockTareaRepo.Object);

        actualizador.ActualizarTareasDeRecurso(recurso);

        // Verifica que el método GetByRecurso fue llamado una vez
        mockAsignacionRepo.Verify(r => r.GetByRecurso(recurso.Id), Times.Once);
    }
}