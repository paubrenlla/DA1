using Domain;
using DataAccess;

namespace DataAccess_Tests;

[TestClass]
public class RecursoDataAccess_Tests
{
    private RecursoDataAccess recursoRepo;

    [TestInitialize]
    public void SetUp()
    {
        recursoRepo = new RecursoDataAccess();
    }
    
    [TestMethod]
    public void AgregarRecurso()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion, false, 5);
        recursoRepo.Add(recurso);
        Assert.AreEqual(1, recursoRepo.GetAll().Count);
        Assert.AreSame(recurso, recursoRepo.GetAll()[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarRecursoYaExistenteEnElSistema()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion, false, 5);
        recursoRepo.Add(recurso);
        recursoRepo.Add(recurso);
    }
    
    [TestMethod]
    public void EliminarRecurso()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion, false, 5);
        recursoRepo.Add(recurso);
        Assert.AreEqual(1, recursoRepo.GetAll().Count);
        Assert.AreSame(recurso, recursoRepo.GetAll()[0]);
        
        recursoRepo.Remove(recurso);
        Assert.AreEqual(0, recursoRepo.GetAll().Count);
        Assert.IsFalse(recursoRepo.GetAll().Contains(recurso));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void NoSePuedeEliminarRecursoEnUso()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion, false, 5);
        recursoRepo.Add(recurso);
        Proyecto proyecto = new Proyecto("Proyecto","descripcion", DateTime.Today);
        Usuario usuario = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        Tarea tarea = new Tarea("Tarea", "Tarea prueba", DateTime.Now, TimeSpan.FromDays(1), false);
        proyecto.agregarTarea(tarea);
        tarea.AgregarUsuario(usuario);
        tarea.AgregarRecurso(recurso, 2);
        recurso.CantidadEnUso = 1;
        recursoRepo.Remove(recurso);
    }
    
    [TestMethod]
    public void BuscarRecursoPorIdDevuelveRecursoCorrecto()
    {
        Recurso recurso = new Recurso("Auto", "Vehiculo", "Transporte", false, 5);
        Recurso recurso2 = new Recurso("Auto2", "Vehiculo", "Transporte", false, 5);
        recursoRepo.Add(recurso);
        recursoRepo.Add(recurso2);
        Recurso resultado = recursoRepo.GetById(recurso2.Id);
        Assert.IsNotNull(resultado);
        Assert.AreEqual(recurso2.Nombre, resultado.Nombre);
    }
}
