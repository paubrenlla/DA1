using Domain;
using DataAccess;

namespace DataAccess_Tests;

[TestClass]
public class TareaDataAccess_Tests
{
    private TareaDataAccess tareaRepo;

    [TestInitialize]
    public void SetUp()
    {
        tareaRepo = new TareaDataAccess();
    }
    
    [TestMethod]
    public void CrearNuevaTarea()
    {
        Tarea tarea = new Tarea("Tarea", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        tareaRepo.Add(tarea);
        
        Assert.AreEqual(1, tareaRepo.GetAll().Count);
        Assert.AreEqual(tarea, tareaRepo.GetAll()[0]);
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
        Assert.IsFalse(tareaRepo.GetAll().Contains(tarea));
    }
}