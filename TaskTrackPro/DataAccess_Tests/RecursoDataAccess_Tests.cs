using Domain;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Tests;

[TestClass]
public class RecursoDataAccess_Tests
{
    private RecursoDataAccess recursoRepo;
    private SqlContext _context;


    [TestInitialize]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new SqlContext(options);
        recursoRepo = new RecursoDataAccess(_context);
    }
    
    [TestMethod]
    public void AgregarRecurso()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion, false, 5);
        recursoRepo.Add(recurso);
        List<Recurso> repo = recursoRepo.GetAll();
        Assert.AreEqual(1, repo.Count);
        Assert.AreEqual(recurso.Id, repo[0].Id);
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
        List<Recurso> repo = recursoRepo.GetAll();
        Assert.AreEqual(1, repo.Count);
        Assert.AreEqual(recurso.Id, repo[0].Id);
        
        recursoRepo.Remove(recurso);
        Assert.AreEqual(0, recursoRepo.GetAll().Count);
        Assert.IsFalse(recursoRepo.GetAll().Contains(recurso));
    }
    
    
    [TestMethod]
    public void BuscarRecursoPorIdDevuelveRecursoCorrecto()
    {
        Recurso recurso = new Recurso("Auto", "Vehiculo", "Transporte", false, 5);
        Recurso recurso2 = new Recurso("Auto2", "Vehiculo", "Transporte", false, 2);
        recursoRepo.Add(recurso);
        recursoRepo.Add(recurso2);
        Recurso resultado = recursoRepo.GetById(recurso2.Id);
        
        Assert.IsNotNull(resultado);
        Assert.AreEqual(recurso2.Nombre, resultado.Nombre);
    }
    
    [TestMethod]
    public void ModificarUsuarioActualizaCorrectamente()
    {
        Recurso recurso = new Recurso("Auto", "Vehiculo", "Transporte", false, 5);
        recursoRepo.Add(recurso);

        recurso.Modificar("Auto nuevo", "Vehiculo", "Transporte", 3,false);
        recursoRepo.Update(recurso);

        Recurso modificado = recursoRepo.GetById(recurso.Id);
        Assert.AreEqual("Auto nuevo", modificado.Nombre);
        Assert.AreEqual(3, modificado.CantidadDelRecurso);
    }
}
