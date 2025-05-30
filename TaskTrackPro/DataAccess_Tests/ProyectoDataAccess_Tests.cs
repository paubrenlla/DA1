using Domain;
using DataAccess;

namespace DataAccess_Tests;

[TestClass]
public class ProyectoDataAccess_Tests
{
    private ProyectoDataAccess proyectoRepo;

    [TestInitialize]
    public void SetUp()
    {
        proyectoRepo = new ProyectoDataAccess();
    }
    
    [TestMethod]
    public void AgregarProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jej";
        DateTime fechaInicio = DateTime.Today;
        

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        proyectoRepo.Add(proyecto);
        Assert.AreEqual(1, proyectoRepo.GetAll().Count);
        Assert.AreSame(proyecto, proyectoRepo.GetAll()[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarProyectoYaExistenteEnElSistema()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jej";
        DateTime fechaInicio = DateTime.Today;
        

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        proyectoRepo.Add(proyecto);
        proyectoRepo.Add(proyecto);
    }
    
    [TestMethod]
    public void EliminarProyecto()
    {
        
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jej";
        DateTime fechaInicio = DateTime.Today;
        

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        proyectoRepo.Add(proyecto);
        
        Assert.AreEqual(1, proyectoRepo.GetAll().Count);
        Assert.AreSame(proyecto, proyectoRepo.GetAll()[0]);
        
        proyectoRepo.Remove(proyecto);
        Assert.AreEqual(0, proyectoRepo.GetAll().Count);
        Assert.IsFalse(proyectoRepo.GetAll().Contains(proyecto));
    }
    
    [TestMethod]
    public void BuscarProyectoPorIdDevuelveProyectoCorrecto()
    { 
        DB db = new DB();
        Proyecto p1= new Proyecto("Proyecto 1", "desc", DateTime.Today);
        Proyecto p2= new Proyecto("Proyecto 1", "desc", DateTime.Today);
        
        proyectoRepo.Add(p1);
        proyectoRepo.Add(p2);
        
        Proyecto resultado = proyectoRepo.GetById(p2.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(p2.Id, resultado.Id);
        Assert.AreEqual(p2.Nombre, resultado.Nombre);
    }
}