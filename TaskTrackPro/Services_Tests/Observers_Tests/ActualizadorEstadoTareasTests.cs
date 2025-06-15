using DataAccess;
using Domain;
using Domain.Enums;
using IDataAcces;
using Moq;
using Services.Observers;

namespace Services_Tests.Observers_Tests;

[TestClass]
public class ActualizadorEstadoTareasTests
{
    private static readonly TimeSpan VALID_TIMESPAN = TimeSpan.FromDays(2);
    private Mock<IDataAccessAsignacionRecursoTarea> _mockAsignacionRepo;
    private Mock<IDataAccessTarea> _mockTareaRepo;
    private ActualizadorEstadoTareas _actualizador;

    [TestInitialize]
    public void Setup()
    {
        _mockAsignacionRepo = new Mock<IDataAccessAsignacionRecursoTarea>();
        _mockTareaRepo = new Mock<IDataAccessTarea>();
        _actualizador = new ActualizadorEstadoTareas(_mockAsignacionRepo.Object, _mockTareaRepo.Object);
    }

    [TestMethod]
    public void ActualizarTareasDeRecurso_ConRecursosSuficientes_ActualizaEstadoConTrue()
    {
        Recurso recurso = new Recurso("Recurso 1", "Tipo", "Descripción", true, 10);
        Tarea tarea = new Tarea("Tarea 1", "Descripcion", DateTime.Now, VALID_TIMESPAN, false);

        List<AsignacionRecursoTarea> asignacionesDelRecurso = new List<AsignacionRecursoTarea>
        {
            new AsignacionRecursoTarea(recurso, tarea, 5)
        };

        List<AsignacionRecursoTarea> asignacionesDeTarea = new List<AsignacionRecursoTarea>
        {
            new AsignacionRecursoTarea(recurso, tarea, 5)
        };

        _mockAsignacionRepo.Setup(r => r.GetByRecurso(recurso.Id)).Returns(asignacionesDelRecurso);
        _mockAsignacionRepo.Setup(r => r.GetByTarea(tarea.Id)).Returns(asignacionesDeTarea);

        _actualizador.ActualizarTareasDeRecurso(recurso);
        
        _mockTareaRepo.Verify(r => r.Update(It.Is<Tarea>(t => t.Id == tarea.Id)), Times.Once);
    }

    [TestMethod]
    public void ActualizarTareasDeRecurso_ConRecursosInsuficientes_ActualizaEstadoConFalse()
    {
        Recurso recurso = new Recurso("Recurso 1", "Tipo", "Descripción", true, 3);
        recurso.ConsumirRecurso(1); 
        
        Tarea tarea = new Tarea("Tarea 1", "Descripcion", DateTime.Now, VALID_TIMESPAN, false);

        List<AsignacionRecursoTarea> asignacionesDelRecurso = new List<AsignacionRecursoTarea>
        {
            new AsignacionRecursoTarea(recurso, tarea, 5)
        };

        List<AsignacionRecursoTarea> asignacionesDeTarea = new List<AsignacionRecursoTarea>
        {
            new AsignacionRecursoTarea(recurso, tarea, 5)
        };

        _mockAsignacionRepo.Setup(r => r.GetByRecurso(recurso.Id)).Returns(asignacionesDelRecurso);
        _mockAsignacionRepo.Setup(r => r.GetByTarea(tarea.Id)).Returns(asignacionesDeTarea);

        _actualizador.ActualizarTareasDeRecurso(recurso);

        _mockTareaRepo.Verify(r => r.Update(It.Is<Tarea>(t => t.Id == tarea.Id)), Times.Once);
    }

    [TestMethod]
    public void ActualizarTareasDeRecurso_SinAsignaciones_NoActualizaNada()
    {
        Recurso recurso = new Recurso("Recurso 1", "Tipo", "Descripción", true, 10);
        List<AsignacionRecursoTarea> asignacionesVacias = new List<AsignacionRecursoTarea>();

        _mockAsignacionRepo.Setup(r => r.GetByRecurso(recurso.Id)).Returns(asignacionesVacias);

        _actualizador.ActualizarTareasDeRecurso(recurso);

        _mockAsignacionRepo.Verify(r => r.GetByRecurso(recurso.Id), Times.Once);
        _mockTareaRepo.Verify(r => r.Update(It.IsAny<Tarea>()), Times.Never);
    }

    [TestMethod]
    public void ActualizarTareasDeRecurso_ConMultiplesRecursos_VerificaCorrectamente()
    {
        Recurso recurso1 = new Recurso("Recurso 1", "Tipo", "Descripción", true, 10);
        Recurso recurso2 = new Recurso("Recurso 2", "Tipo", "Descripción", true, 5);
        
        Tarea tarea = new Tarea("Tarea 1", "Descripcion", DateTime.Now, VALID_TIMESPAN, false);

        List<AsignacionRecursoTarea> asignacionesDelRecurso1 = new List<AsignacionRecursoTarea>
        {
            new AsignacionRecursoTarea(recurso1, tarea, 3)
        };

        List<AsignacionRecursoTarea> asignacionesDeTarea = new List<AsignacionRecursoTarea>
        {
            new AsignacionRecursoTarea(recurso1, tarea, 3),
            new AsignacionRecursoTarea(recurso2, tarea, 2)
        };

        _mockAsignacionRepo.Setup(r => r.GetByRecurso(recurso1.Id)).Returns(asignacionesDelRecurso1);
        _mockAsignacionRepo.Setup(r => r.GetByTarea(tarea.Id)).Returns(asignacionesDeTarea);

        _actualizador.ActualizarTareasDeRecurso(recurso1);

        _mockAsignacionRepo.Verify(r => r.GetByRecurso(recurso1.Id), Times.Once);
        _mockAsignacionRepo.Verify(r => r.GetByTarea(tarea.Id), Times.Once);
        _mockTareaRepo.Verify(r => r.Update(tarea), Times.Once);
    }

    [TestMethod]
    public void VerificarRecursosDisponibles_ConRecursosSuficientes_RetornaTrue()
    {
        Recurso recurso = new Recurso("Recurso 1", "Tipo", "Descripción", true, 10);
        Tarea tarea = new Tarea("Tarea 1", "Descripcion", DateTime.Now, VALID_TIMESPAN, false);

        List<AsignacionRecursoTarea> asignaciones = new List<AsignacionRecursoTarea>
        {
            new AsignacionRecursoTarea(recurso, tarea, 5)
        };

        _mockAsignacionRepo.Setup(r => r.GetByTarea(tarea.Id)).Returns(asignaciones);

        List<AsignacionRecursoTarea> asignacionesDelRecurso = new List<AsignacionRecursoTarea>
        {
            new AsignacionRecursoTarea(recurso, tarea, 3)
        };

        _mockAsignacionRepo.Setup(r => r.GetByRecurso(recurso.Id)).Returns(asignacionesDelRecurso);
        _mockAsignacionRepo.Setup(r => r.GetByTarea(tarea.Id)).Returns(asignaciones);

        _actualizador.ActualizarTareasDeRecurso(recurso);

        _mockTareaRepo.Verify(r => r.Update(tarea), Times.Once);
    }

    [TestMethod]
    public void ActualizarTareasDeRecurso_ConRecursoNull_DeberiaLanzarExcepcion()
    {
        Recurso recursoNull = null;

        Assert.ThrowsException<NullReferenceException>(() => 
            _actualizador.ActualizarTareasDeRecurso(recursoNull));
    }
}