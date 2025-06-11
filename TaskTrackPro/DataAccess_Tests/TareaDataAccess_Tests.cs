using Domain;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Tests;

[TestClass]
public class TareaDataAccess_Tests
{
    private SqlContext _context;
    private TareaDataAccess tareaRepo;

    [TestInitialize]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new SqlContext(options);
        tareaRepo = new TareaDataAccess(_context);
    }
    
    [TestMethod]
    public void CrearNuevaTarea()
    {
        Tarea tarea = new Tarea("Tarea", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        tareaRepo.Add(tarea);
        
        Assert.AreEqual(1, tareaRepo.GetAll().Count);
        Assert.AreEqual(tarea.Id, tareaRepo.GetAll()[0].Id);
    }
    
    [TestMethod]
    public void BuscarTareaPorIdDevuelveBien()
    {
        Tarea tarea = new Tarea("Tarea", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        Tarea tarea2 = new Tarea("Tarea2", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        tareaRepo.Add(tarea);
        tareaRepo.Add(tarea2);
        
        Tarea resultado = tareaRepo.GetById(tarea2.Id);
        Assert.AreEqual(resultado, tarea2);
    }
    
    [TestMethod]
    public void EliminarTarea()
    {
        Tarea tarea = new Tarea("Tarea", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        Tarea tarea2 = new Tarea("Tarea2", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        tareaRepo.Add(tarea);
        tareaRepo.Add(tarea2);
        
        Assert.AreEqual(2, tareaRepo.GetAll().Count);
        
        tareaRepo.Remove(tarea);
        
        Assert.AreEqual(1, tareaRepo.GetAll().Count);
    }

    [TestMethod]
    public void ActualizarTarea()
    {
        Tarea tarea = new Tarea("Tarea", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        tareaRepo.Add(tarea);
        
        Assert.AreEqual(1, tareaRepo.GetAll().Count);
        
        tarea.Descripcion = "Descripcion Cambiada";
        tarea.Titulo = "Tarea Nueva";
        tareaRepo.Update(tarea);
        Assert.AreEqual(tarea.Id, tareaRepo.GetAll()[0].Id);
        Assert.AreEqual("Tarea Nueva", tareaRepo.GetAll()[0].Titulo);
        Assert.AreEqual("Descripcion Cambiada", tareaRepo.GetAll()[0].Descripcion);
        
    }
}