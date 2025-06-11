using Controllers;
using DTOs;
using Domain;
using IDataAcces;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Controllers_Tests;

[TestClass]
public class RecursoControllerTests
{
    private RecursoController _controller;
    private IRecursoService _service;
    private IDataAccessRecurso _recursoRepo;
    private IDataAccessAsignacionRecursoTarea _asignacionRepo;
    private Recurso _recursoEjemplo;

    [TestInitialize]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        SqlContext context = new SqlContext(options);
        _recursoRepo = new RecursoDataAccess(context);
        _asignacionRepo = new AsignacionRecursoTareaDataAccess();
        _service = new RecursoService(_recursoRepo, _asignacionRepo);
        _controller = new RecursoController(_service);

        _recursoEjemplo = new Recurso("Proyector", "Audiovisual", "Proyector HD", false, 3);
        _recursoRepo.Add(_recursoEjemplo);
    }

    [TestMethod]
    public void GetById_FuncionaCorrectamente()
    {
        var dto = _controller.GetById(_recursoEjemplo.Id);

        Assert.IsNotNull(dto);
        Assert.AreEqual(_recursoEjemplo.Id, dto.Id);
        Assert.AreEqual(_recursoEjemplo.Nombre, dto.Nombre);
    }

    [TestMethod]
    public void GetAll_FuncionaCorrectamente()
    {
        var lista = _controller.GetAll();

        Assert.IsNotNull(lista);
        Assert.IsTrue(lista.Any(r => r.Id == _recursoEjemplo.Id));
    }

    [TestMethod]
    public void Add_FuncionaCorrectamente()
    {
        Recurso recurso = new Recurso("ProyectorGrande", "Audiovisual", "Proyector HD", false, 3);
        RecursoDTO nuevoDTO = Convertidor.ARecursoDTO(recurso);
        RecursoDTO agregado = _controller.Add(nuevoDTO);

        Assert.IsNotNull(agregado);
        Assert.AreEqual(nuevoDTO.Nombre, agregado.Nombre);
    }

    [TestMethod]
    public void Delete_FuncionaCorrectamente()
    {
        _controller.Delete(_recursoEjemplo.Id);

        List<RecursoDTO> lista = _controller.GetAll();
        Assert.IsFalse(lista.Any(r => r.Id == _recursoEjemplo.Id));
    }

    [TestMethod]
    public void ModificarRecurso_FuncionaCorrectamente()
    {
        RecursoDTO dto = _controller.GetById(_recursoEjemplo.Id);
        dto.CantidadDelRecurso = 10;

        _controller.ModificarRecurso(dto);

        RecursoDTO modificado = _controller.GetById(_recursoEjemplo.Id);
        Assert.AreEqual(10, modificado.CantidadDelRecurso);
    }

    [TestMethod]
    public void EstaEnUso_FuncionaCorrectamente()
    {
        bool enUso = _controller.EstaEnUso(_recursoEjemplo.Id);
        Assert.IsFalse(enUso);
    }

    [TestMethod]
    public void EstaDisponible_FuncionaCorrectamente()
    {
        bool disponible = _controller.EstaDisponible(_recursoEjemplo.Id, 2);
        Assert.IsTrue(disponible);
    }
}