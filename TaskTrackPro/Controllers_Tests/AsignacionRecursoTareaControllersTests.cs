using Controllers;
using DTOs;
using Domain;
using IDataAcces;
using DataAccess;
using Services;

namespace Controllers_Tests;

[TestClass]
public class AsignacionRecursoTareaControllersTests
{
    private static readonly TimeSpan VALID_TIMESPAN = new TimeSpan(6, 5, 0, 0);

    private AsignacionRecursoTareaControllers _controller;
    private IAsignacionRecursoTareaService _service;

    private IDataAccessAsignacionRecursoTarea _repoAsignaciones;
    private IDataAccessTarea _repoTareas;
    private IDataAccessRecurso _repoRecursos;

    private Tarea _tareaEjemplo;
    private Recurso _recursoEjemplo;

    [TestInitialize]
    public void SetUp()
    {
        _repoAsignaciones = new AsignacionRecursoTareaDataAccess();
        _repoTareas = new TareaDataAccess();
        _repoRecursos = new RecursoDataAccess();

        _service = new AsignacionRecursoTareaService(_repoRecursos, _repoTareas, _repoAsignaciones);
        _controller = new AsignacionRecursoTareaControllers(_service);

        _tareaEjemplo = new Tarea("Tarea Valida", "descripcion", DateTime.Now, VALID_TIMESPAN, false);
        _repoTareas.Add(_tareaEjemplo);

        _recursoEjemplo = new Recurso("Computadora", "tipo", "desripcion", false, 8 );
        _repoRecursos.Add(_recursoEjemplo);
    }

    [TestMethod]
    public void CrearAsignacionRecursoTarea_CreaCorrectamente()
    {
        RecursoDTO recursoDTO = Convertidor.ARecursoDTO(_recursoEjemplo);
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);
        AsignacionRecursoTareaDTO dto = Convertidor.AAsignacionRecursoTareaDTO(new AsignacionRecursoTarea(_recursoEjemplo, _tareaEjemplo, 2));

        AsignacionRecursoTareaDTO resultado = _controller.CrearAsignacionRecursoTarea(dto);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(dto.Tarea.Id, resultado.Tarea.Id);
        Assert.AreEqual(dto.Recurso.Id, resultado.Recurso.Id);
    }

    [TestMethod]
    public void GetAll_DevuelveListaDeAsignaciones()
    {
        RecursoDTO recursoDTO = Convertidor.ARecursoDTO(_recursoEjemplo);
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);
        AsignacionRecursoTareaDTO dto = Convertidor.AAsignacionRecursoTareaDTO(new AsignacionRecursoTarea(_recursoEjemplo, _tareaEjemplo, 2));

        _controller.CrearAsignacionRecursoTarea(dto);

        List<AsignacionRecursoTareaDTO> asignaciones = _controller.GetAll();

        Assert.IsTrue(asignaciones.Count > 0);
        Assert.IsTrue(asignaciones.Any(a => a.Tarea.Id == dto.Tarea.Id && a.Recurso.Id == dto.Recurso.Id));
    }

    [TestMethod]
    public void EliminarRecursoDeTarea_EliminaCorrectamente()
    {
        RecursoDTO recursoDTO = Convertidor.ARecursoDTO(_recursoEjemplo);
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);
        AsignacionRecursoTareaDTO dto = Convertidor.AAsignacionRecursoTareaDTO(new AsignacionRecursoTarea(_recursoEjemplo, _tareaEjemplo, 2));

        _controller.CrearAsignacionRecursoTarea(dto);

        _controller.EliminarRecursoDeTarea(dto.Tarea.Id, dto.Recurso.Id);

        List<AsignacionRecursoTareaDTO> asignaciones = _controller.GetAll();
        Assert.IsFalse(asignaciones.Any(a => a.Tarea.Id == dto.Tarea.Id && a.Recurso.Id == dto.Recurso.Id));
    }

    [TestMethod]
    public void ModificarAsignacion_ModificaCorrectamente()
    {
        AsignacionRecursoTareaDTO asignacionInicial = Convertidor.AAsignacionRecursoTareaDTO(
            new AsignacionRecursoTarea(_recursoEjemplo, _tareaEjemplo, cantidadNecesaria: 2)
        );

        AsignacionRecursoTareaDTO asignacionCreada = _controller.CrearAsignacionRecursoTarea(asignacionInicial);

        asignacionCreada.Cantidad = 5;
        _controller.ModificarAsignacion(asignacionCreada);

        AsignacionRecursoTareaDTO asignacionModificada = _controller.GetById(asignacionCreada.Id);
        Assert.IsNotNull(asignacionModificada);
        Assert.AreEqual(5, asignacionModificada.Cantidad);

    }
    
    [TestMethod]
    public void RecursosDeLaTarea_DevuelveRecursosCorrectamente()
    {
        RecursoDTO recursoDTO = Convertidor.ARecursoDTO(_recursoEjemplo);
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);
        AsignacionRecursoTareaDTO dto = Convertidor.AAsignacionRecursoTareaDTO(new AsignacionRecursoTarea(_recursoEjemplo, _tareaEjemplo, 2));

        _controller.CrearAsignacionRecursoTarea(dto);

        List<RecursoDTO> recursos = _controller.RecursosDeLaTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(recursos);
        Assert.IsTrue(recursos.Any(r => r.Id == _recursoEjemplo.Id));
    }

    [TestMethod]
    public void RecursoEsExclusivo_DevuelveCorrecto()
    {
        bool exclusivo = _controller.RecursoEsExclusivo(_recursoEjemplo.Id);
        Assert.IsTrue(exclusivo);
    }

    [TestMethod]
    public void VerificarRecursosDeTareaDisponibles_DevuelveTrueInicialmente()
    {
        bool disponible = _controller.VerificarRecursosDeTareaDisponibles(_tareaEjemplo.Id);
        Assert.IsTrue(disponible);
    }
    
    [TestMethod]
    public void GetById_RetornaAsignacionCorrecta()
    {
        RecursoDTO recursoDTO = Convertidor.ARecursoDTO(_recursoEjemplo);
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);
        AsignacionRecursoTareaDTO dto = Convertidor.AAsignacionRecursoTareaDTO(new AsignacionRecursoTarea(_recursoEjemplo, _tareaEjemplo, 2));

        AsignacionRecursoTareaDTO asignacionCreada = _controller.CrearAsignacionRecursoTarea(dto);
        AsignacionRecursoTareaDTO asignacionObtenida = _controller.GetById(asignacionCreada.Id);

        Assert.IsNotNull(asignacionObtenida);
        Assert.AreEqual(asignacionCreada.Id, asignacionObtenida.Id);
        Assert.AreEqual(asignacionCreada.Recurso.Id, asignacionObtenida.Recurso.Id);
        Assert.AreEqual(asignacionCreada.Tarea.Id, asignacionObtenida.Tarea.Id);
        Assert.AreEqual(asignacionCreada.Cantidad, asignacionObtenida.Cantidad);
    }
    
    [TestMethod]
    public void EliminarRecursosDeTarea_EliminaTodosLosRecursosDeUnaTarea()
    {
        AsignacionRecursoTareaDTO dto = Convertidor.AAsignacionRecursoTareaDTO(new AsignacionRecursoTarea(_recursoEjemplo, _tareaEjemplo, 2));
        _controller.CrearAsignacionRecursoTarea(dto);

        _controller.EliminarRecursosDeTarea(_tareaEjemplo.Id);

        List<RecursoDTO> recursos = _controller.RecursosDeLaTarea(_tareaEjemplo.Id);
        Assert.IsFalse(recursos.Any());
    }

    [TestMethod]
    public void ActualizarEstadoDeTareasConRecurso_NoLanzaExcepcion()
    {
        _controller.ActualizarEstadoDeTareasConRecurso(_recursoEjemplo.Id);
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void GetAsignacionesDeTarea_DevuelveListaVacia_CuandoTareaNoTieneAsignaciones()
    {
        List<AsignacionRecursoTareaDTO> resultado = _controller.GetAsignacionesDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void GetAsignacionesDeTarea_DevuelveUnaAsignacion_CuandoTareaTieneUnaAsignacion()
    {
        AsignacionRecursoTareaDTO dto = Convertidor.AAsignacionRecursoTareaDTO(
            new AsignacionRecursoTarea(_recursoEjemplo, _tareaEjemplo, 3)
        );
        AsignacionRecursoTareaDTO asignacionCreada = _controller.CrearAsignacionRecursoTarea(dto);

        List<AsignacionRecursoTareaDTO> resultado = _controller.GetAsignacionesDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(asignacionCreada.Id, resultado[0].Id);
        Assert.AreEqual(_recursoEjemplo.Id, resultado[0].Recurso.Id);
        Assert.AreEqual(_tareaEjemplo.Id, resultado[0].Tarea.Id);
        Assert.AreEqual(3, resultado[0].Cantidad);
    }

    [TestMethod]
    public void GetAsignacionesDeTarea_DevuelveMultiplesAsignaciones_CuandoTareaTieneVariasAsignaciones()
    {
        Recurso recurso2 = new Recurso("Recurso2", "tipo2", "descripcion2", false, 5);
        _repoRecursos.Add(recurso2);

        AsignacionRecursoTareaDTO dto1 = Convertidor.AAsignacionRecursoTareaDTO(
            new AsignacionRecursoTarea(_recursoEjemplo, _tareaEjemplo, 2)
        );
        AsignacionRecursoTareaDTO dto2 = Convertidor.AAsignacionRecursoTareaDTO(
            new AsignacionRecursoTarea(recurso2, _tareaEjemplo, 4)
        );

        _controller.CrearAsignacionRecursoTarea(dto1);
        _controller.CrearAsignacionRecursoTarea(dto2);

        List<AsignacionRecursoTareaDTO> resultado = _controller.GetAsignacionesDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(2, resultado.Count);
        Assert.IsTrue(resultado.Any(a => a.Recurso.Id == _recursoEjemplo.Id && a.Cantidad == 2));
        Assert.IsTrue(resultado.Any(a => a.Recurso.Id == recurso2.Id && a.Cantidad == 4));
    }

    [TestMethod]
    public void GetAsignacionesDeTarea_NoDevuelveAsignacionesDeOtrasTareas()
    {
        Tarea tarea2 = new Tarea("Tarea2", "desc2", DateTime.Now, VALID_TIMESPAN, false);
        _repoTareas.Add(tarea2);

        AsignacionRecursoTareaDTO dtoTarea1 = Convertidor.AAsignacionRecursoTareaDTO(
            new AsignacionRecursoTarea(_recursoEjemplo, _tareaEjemplo, 2)
        );
        AsignacionRecursoTareaDTO dtoTarea2 = Convertidor.AAsignacionRecursoTareaDTO(
            new AsignacionRecursoTarea(_recursoEjemplo, tarea2, 3)
        );

        _controller.CrearAsignacionRecursoTarea(dtoTarea1);
        _controller.CrearAsignacionRecursoTarea(dtoTarea2);

        List<AsignacionRecursoTareaDTO> resultado = _controller.GetAsignacionesDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(_tareaEjemplo.Id, resultado[0].Tarea.Id);
        Assert.AreEqual(2, resultado[0].Cantidad);
    }
}
