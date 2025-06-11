using DataAccess;
using Services;
using Domain;
using Domain.Observers;
using IDataAcces;
using DTOs;
using Microsoft.EntityFrameworkCore;

[TestClass]
public class RecursoServiceTests
{
    private RecursoService _service;
    private IDataAccessRecurso _repoRecursos;
    private IDataAccessAsignacionRecursoTarea _repoAsignaciones;

    private Recurso _recurso1;
    private Recurso _recurso2;
    private Tarea _tarea1;

    private static readonly TimeSpan VALID_TIMESPAN = new TimeSpan(6, 5, 0, 0);

    [TestInitialize]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new SqlContext(options);
        _repoRecursos = new RecursoDataAccess(context);
        _repoAsignaciones = new AsignacionRecursoTareaDataAccess();

        _service = new RecursoService(_repoRecursos, _repoAsignaciones);

        _recurso1 = new Recurso("Recurso1", "Tipo1", "Desc1", false, 10);
        _recurso2 = new Recurso("Recurso2", "Tipo2", "Desc2", false, 5);

        _tarea1 = new Tarea("Tarea1", "DescTarea1", DateTime.Today, VALID_TIMESPAN, true);
        
        _repoRecursos.Add(_recurso1);
        _repoRecursos.Add(_recurso2);
    }

    [TestMethod]
    public void Crear_NuevoRecurso_SeCreaCorrectamente()
    {
        Recurso _recursoNuevo = new Recurso("Recurso", "Tipo", "Desc", false, 10);

        RecursoDTO recursoNuevo = Convertidor.ARecursoDTO(_recursoNuevo);

        RecursoDTO resultado = _service.Add(recursoNuevo);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(_recursoNuevo.Nombre, resultado.Nombre);

        Recurso recursoEnRepo = _repoRecursos.GetById(resultado.Id);
        
        Assert.IsNotNull(recursoEnRepo);
        Assert.AreEqual(_recursoNuevo.Nombre, recursoEnRepo.Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Delete_RecursoAsignado_LanzaExcepcion()
    {
        _repoAsignaciones.Add(new AsignacionRecursoTarea(_recurso1, _tarea1, 2));
        _service.Delete(_recurso1.Id);
    }
    
    [TestMethod]
    public void GetById_CuandoExisteElRecurso_RetornaElDTOCorrecto()
    {
        RecursoDTO resultado = _service.GetById(_recurso1.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(_recurso1.Id, resultado.Id);
        Assert.AreEqual(_recurso1.Nombre, resultado.Nombre);
        Assert.AreEqual(_recurso1.Tipo, resultado.Tipo);
        Assert.AreEqual(_recurso1.Descripcion, resultado.Descripcion);
        Assert.AreEqual(_recurso1.CantidadDelRecurso, resultado.CantidadDelRecurso);
        Assert.AreEqual(_recurso1.SePuedeCompartir, resultado.SePuedeCompartir);
    }


    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void Delete_RecursoNoAsignado_SeElimina()
    {
        Assert.IsNotNull(_repoRecursos.GetById(_recurso2.Id));
        
        _service.Delete(_recurso2.Id);

        _service.GetById(_recurso2.Id);
    }

    [TestMethod]
    public void ModificarRecurso_SeModifica()
    {
        var dtoModificado = new RecursoDTO
        {
            Id = _recurso1.Id,
            Nombre = "Modificado",
            Tipo = _recurso1.Tipo,
            Descripcion = "Desc Modificada",
            CantidadDelRecurso = 20,
            SePuedeCompartir = true
        };

        _service.ModificarRecurso(dtoModificado);

        var recursoModificado = _repoRecursos.GetById(_recurso1.Id);

        Assert.AreEqual("Modificado", recursoModificado.Nombre);
    }
    
    [TestMethod]
    public void EstaEnUso_CuandoRecursoNoFueConsumido_RetornaFalse()
    {
        bool enUso = _service.EstaEnUso(_recurso1.Id);

        Assert.IsFalse(enUso);
    }

    [TestMethod]
    public void EstaEnUso_CuandoRecursoFueConsumido_RetornaTrue()
    {
        _service.ConsumirRecurso(_recurso1.Id, 3);

        bool enUso = _service.EstaEnUso(_recurso1.Id);

        Assert.IsTrue(enUso);
    }

    [TestMethod]
    public void ConsumirRecurso_DisminuyeCantidadDisponible()
    {
        _service.ConsumirRecurso(_recurso1.Id, 4);

        Assert.AreEqual(4, _recurso1.CantidadEnUso);
    }

    [TestMethod]
    public void LiberarRecurso_AumentaCantidadDisponible()
    {
        _service.ConsumirRecurso(_recurso1.Id, 5);
        
        _service.LiberarRecurso(_recurso1.Id, 2);

        Assert.AreEqual(3, _recurso1.CantidadEnUso);
    }

    [TestMethod]
    public void EstaDisponible_CuandoHaySuficienteCantidad_RetornaTrue()
    {
        bool disponible = _service.EstaDisponible(_recurso1.Id, 5);

        Assert.IsTrue(disponible);
    }

    [TestMethod]
    public void EstaDisponible_CuandoNoHaySuficienteCantidad_RetornaFalse()
    {
        _service.ConsumirRecurso(_recurso1.Id, 10);

        bool disponible = _service.EstaDisponible(_recurso1.Id, 1);

        Assert.IsFalse(disponible);
    }
    
    [TestMethod]
    public void GetAll_CuandoHayRecursos_RetornaListaDeDTOsCorrecta()
    {
        List<RecursoDTO> resultado = _service.GetAll();

        Assert.AreEqual(2, resultado.Count);

        Assert.IsTrue(resultado.Any(r => r.Nombre == _recurso1.Nombre && r.Tipo == _recurso1.Tipo));
        Assert.IsTrue(resultado.Any(r => r.Nombre == _recurso2.Nombre && r.Tipo == _recurso2.Tipo));
    }

    public class StubRecursoObserver : IRecursoObserver
    {
        public bool FueNotificado { get; private set; } = false;
        public Recurso RecursoNotificado { get; private set; }

        public void ActualizarTareasDeRecurso(Recurso recurso)
        {
            FueNotificado = true;
            RecursoNotificado = recurso;
        }
    }
    
    [TestMethod]
    public void NotificarObservadores_ConObservadorAgregado_LlamaActualizarTareasDeRecurso()
    {
        StubRecursoObserver observador = new StubRecursoObserver();
        _service.AgregarObservador(observador);

        _service.ConsumirRecurso(_recurso1.Id, 1); 

        Assert.IsTrue(observador.FueNotificado);
        Assert.IsNotNull(observador.RecursoNotificado);
        Assert.AreEqual(_recurso1.Id, observador.RecursoNotificado.Id);
    }

    
}
