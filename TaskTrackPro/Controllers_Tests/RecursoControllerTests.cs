using Controllers;
using DTOs;
using Moq;
using Services;

namespace Controllers_Tests;

[TestClass]
public class RecursoControllerTests
{
    private Mock<IRecursoService> _mockService;
    private RecursoController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<IRecursoService>();
        _controller = new RecursoController(_mockService.Object);
    }
    
    [TestMethod]
    public void GetById_ConIdValido_RetornaRecursoDTO()
    {
        int idRecurso = 1;
        RecursoDTO recursoEsperado = new RecursoDTO
        {
            Id = idRecurso,
            Nombre = "Recurso Test",
            Tipo = "Tipo Test",
            Descripcion = "Descripción Test",
            CantidadDelRecurso = 10,
            SePuedeCompartir = true
        };

        _mockService.Setup(s => s.GetById(idRecurso)).Returns(recursoEsperado);

        RecursoDTO resultado = _controller.GetById(idRecurso);

        Assert.AreEqual(recursoEsperado, resultado);
        _mockService.Verify(s => s.GetById(idRecurso), Times.Once);
    }

    [TestMethod]
    public void GetById_CuandoServiceLanzaExcepcion_PropagaExcepcion()
    {
        int idRecurso = 999;
        _mockService.Setup(s => s.GetById(idRecurso))
                   .Throws(new NullReferenceException("Recurso"));

        Assert.ThrowsException<NullReferenceException>(() => _controller.GetById(idRecurso));
        _mockService.Verify(s => s.GetById(idRecurso), Times.Once);
    }

    [TestMethod]
    public void GetAll_RetornaListaDeRecursos()
    {
        List<RecursoDTO> recursosEsperados = new List<RecursoDTO>
        {
            new RecursoDTO { Id = 1, Nombre = "Recurso 1", Tipo = "Tipo 1" },
            new RecursoDTO { Id = 2, Nombre = "Recurso 2", Tipo = "Tipo 2" },
            new RecursoDTO { Id = 3, Nombre = "Recurso 3", Tipo = "Tipo 3" }
        };

        _mockService.Setup(s => s.GetAll()).Returns(recursosEsperados);

        List<RecursoDTO> resultado = _controller.GetAll();

        Assert.AreEqual(recursosEsperados.Count, resultado.Count);
        CollectionAssert.AreEqual(recursosEsperados, resultado);
        _mockService.Verify(s => s.GetAll(), Times.Once);
    }

    [TestMethod]
    public void GetAll_SinRecursos_RetornaListaVacia()
    {
        List<RecursoDTO> listaVacia = new List<RecursoDTO>();
        _mockService.Setup(s => s.GetAll()).Returns(listaVacia);

        List<RecursoDTO> resultado = _controller.GetAll();

        Assert.AreEqual(0, resultado.Count);
        _mockService.Verify(s => s.GetAll(), Times.Once);
    }

    [TestMethod]
    public void Add_ConDTOValido_RetornaRecursoCreado()
    {
        RecursoDTO nuevoRecursoDTO = new RecursoDTO
        {
            Nombre = "Nuevo Recurso",
            Tipo = "Tipo Nuevo",
            Descripcion = "Descripción Nueva",
            CantidadDelRecurso = 5,
            SePuedeCompartir = false
        };

        RecursoDTO recursoCreado = new RecursoDTO
        {
            Id = 1,
            Nombre = "Nuevo Recurso",
            Tipo = "Tipo Nuevo",
            Descripcion = "Descripción Nueva",
            CantidadDelRecurso = 5,
            SePuedeCompartir = false
        };

        _mockService.Setup(s => s.Add(nuevoRecursoDTO)).Returns(recursoCreado);

        RecursoDTO resultado = _controller.Add(nuevoRecursoDTO);

        Assert.AreEqual(recursoCreado, resultado);
        Assert.AreEqual(1, resultado.Id);
        _mockService.Verify(s => s.Add(nuevoRecursoDTO), Times.Once);
    }

    [TestMethod]
    public void Add_CuandoServiceLanzaExcepcion_PropagaExcepcion()
    {
        RecursoDTO recursoInvalido = new RecursoDTO();
        _mockService.Setup(s => s.Add(recursoInvalido))
                   .Throws(new ArgumentException("Datos inválidos"));

        Assert.ThrowsException<ArgumentException>(() => _controller.Add(recursoInvalido));
        _mockService.Verify(s => s.Add(recursoInvalido), Times.Once);
    }

    [TestMethod]
    public void Delete_ConIdValido_LlamaServiceDelete()
    {
        int idRecurso = 1;
        _mockService.Setup(s => s.Delete(idRecurso));

        _controller.Delete(idRecurso);

        _mockService.Verify(s => s.Delete(idRecurso), Times.Once);
    }

    [TestMethod]
    public void Delete_CuandoRecursoEstaEnUso_PropagaExcepcion()
    {
        int idRecurso = 1;
        _mockService.Setup(s => s.Delete(idRecurso))
                   .Throws(new ArgumentOutOfRangeException("El recurso esta asignado a alguna tarea."));

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => _controller.Delete(idRecurso));
        _mockService.Verify(s => s.Delete(idRecurso), Times.Once);
    }
    
    [TestMethod]
    public void ModificarRecurso_ConDTOValido_LlamaServiceModificar()
    {
        RecursoDTO recursoModificado = new RecursoDTO
        {
            Id = 1,
            Nombre = "Recurso Modificado",
            Tipo = "Tipo Modificado",
            Descripcion = "Descripción Modificada",
            CantidadDelRecurso = 15,
            SePuedeCompartir = true
        };

        _mockService.Setup(s => s.ModificarRecurso(recursoModificado));

        _controller.ModificarRecurso(recursoModificado);

        _mockService.Verify(s => s.ModificarRecurso(recursoModificado), Times.Once);
    }

    [TestMethod]
    public void ModificarRecurso_CuandoRecursoNoExiste_PropagaExcepcion()
    {
        RecursoDTO recursoInexistente = new RecursoDTO { Id = 999 };
        _mockService.Setup(s => s.ModificarRecurso(recursoInexistente))
                   .Throws(new NullReferenceException("Recurso no encontrado"));

        Assert.ThrowsException<NullReferenceException>(() => _controller.ModificarRecurso(recursoInexistente));
        _mockService.Verify(s => s.ModificarRecurso(recursoInexistente), Times.Once);
    }

    [TestMethod]
    public void EstaEnUso_CuandoRecursoEstaEnUso_RetornaTrue()
    {
        int idRecurso = 1;
        _mockService.Setup(s => s.EstaEnUso(idRecurso)).Returns(true);

        bool resultado = _controller.EstaEnUso(idRecurso);

        Assert.IsTrue(resultado);
        _mockService.Verify(s => s.EstaEnUso(idRecurso), Times.Once);
    }

    [TestMethod]
    public void EstaEnUso_CuandoRecursoNoEstaEnUso_RetornaFalse()
    {
        int idRecurso = 1;
        _mockService.Setup(s => s.EstaEnUso(idRecurso)).Returns(false);

        bool resultado = _controller.EstaEnUso(idRecurso);

        Assert.IsFalse(resultado);
        _mockService.Verify(s => s.EstaEnUso(idRecurso), Times.Once);
    }

    [TestMethod]
    public void EstaEnUso_CuandoRecursoNoExiste_PropagaExcepcion()
    {
        int idRecursoInexistente = 999;
        _mockService.Setup(s => s.EstaEnUso(idRecursoInexistente))
                   .Throws(new NullReferenceException("Recurso no encontrado"));

        Assert.ThrowsException<NullReferenceException>(() => _controller.EstaEnUso(idRecursoInexistente));
        _mockService.Verify(s => s.EstaEnUso(idRecursoInexistente), Times.Once);
    }

    [TestMethod]
    public void EstaDisponible_ConCantidadSuficiente_RetornaTrue()
    {
        int idRecurso = 1;
        int cantidad = 5;
        _mockService.Setup(s => s.EstaDisponible(idRecurso, cantidad)).Returns(true);

        bool resultado = _controller.EstaDisponible(idRecurso, cantidad);

        Assert.IsTrue(resultado);
        _mockService.Verify(s => s.EstaDisponible(idRecurso, cantidad), Times.Once);
    }

    [TestMethod]
    public void EstaDisponible_ConCantidadInsuficiente_RetornaFalse()
    {
        int idRecurso = 1;
        int cantidad = 100;
        _mockService.Setup(s => s.EstaDisponible(idRecurso, cantidad)).Returns(false);

        bool resultado = _controller.EstaDisponible(idRecurso, cantidad);

        Assert.IsFalse(resultado);
        _mockService.Verify(s => s.EstaDisponible(idRecurso, cantidad), Times.Once);
    }

    [TestMethod]
    public void EstaDisponible_ConCantidadCero_RetornaTrue()
    {
        // Arrange
        int idRecurso = 1;
        int cantidad = 0;
        _mockService.Setup(s => s.EstaDisponible(idRecurso, cantidad)).Returns(true);

        // Act
        bool resultado = _controller.EstaDisponible(idRecurso, cantidad);

        // Assert
        Assert.IsTrue(resultado);
        _mockService.Verify(s => s.EstaDisponible(idRecurso, cantidad), Times.Once);
    }

    [TestMethod]
    public void EstaDisponible_ConCantidadNegativa_PropagaExcepcion()
    {
        int idRecurso = 1;
        int cantidadNegativa = -5;
        _mockService.Setup(s => s.EstaDisponible(idRecurso, cantidadNegativa))
                   .Throws(new ArgumentException("La cantidad no puede ser negativa"));

        Assert.ThrowsException<ArgumentException>(() => _controller.EstaDisponible(idRecurso, cantidadNegativa));
        _mockService.Verify(s => s.EstaDisponible(idRecurso, cantidadNegativa), Times.Once);
    }

    [TestMethod]
    public void Constructor_ConServiceValido_CreaControllerCorrectamente()
    {
        Mock<IRecursoService> mockService = new Mock<IRecursoService>();

        RecursoController controller = new RecursoController(mockService.Object);

        Assert.IsNotNull(controller);
    }
}