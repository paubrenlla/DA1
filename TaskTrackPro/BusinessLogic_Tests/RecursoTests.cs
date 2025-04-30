using BusinessLogic;
using System.Reflection;

namespace BusinessLogic_Tests;

[TestClass]
public class RecursoTests
{
    [TestInitialize]
    public void Setup()
    {
        typeof(Recurso)
            .GetField("_contadorId", BindingFlags.Static | BindingFlags.NonPublic)
            ?.SetValue(null, 1);
    }
    
    [TestMethod]
    public void CreoRecursoConParametrosCorrectosSinProyecto()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        bool sePuedeCompartir = false;
        int cantidadDelRecurso = 5; 
        Proyecto? proyecto = null;

        Recurso recurso = new Recurso(nombre, tipo, descripcion, sePuedeCompartir, cantidadDelRecurso, proyecto);

        Assert.AreEqual(nombre, recurso.Nombre);
        Assert.AreEqual(tipo, recurso.Tipo);
        Assert.AreEqual(descripcion, recurso.Descripcion);
        Assert.AreEqual(sePuedeCompartir, recurso.SePuedeCompartir);
        Assert.AreEqual(cantidadDelRecurso, recurso.CantidadDelRecurso);
        Assert.AreEqual(0, recurso.CantidadEnUso); 
        Assert.AreEqual(proyecto, recurso.ProyectoAlQuePertenece);
    }
    
    [TestMethod]
    public void CreoRecursoConParametrosCorrectosSinProyectoSobrecargado()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        bool sePuedeCompartir = false;
        int cantidadDelRecurso = 5;

        Recurso recurso = new Recurso(nombre, tipo, descripcion, sePuedeCompartir, cantidadDelRecurso);

        Assert.AreEqual(nombre, recurso.Nombre);
        Assert.AreEqual(tipo, recurso.Tipo);
        Assert.AreEqual(descripcion, recurso.Descripcion);
        Assert.AreEqual(sePuedeCompartir, recurso.SePuedeCompartir);
        Assert.AreEqual(cantidadDelRecurso, recurso.CantidadDelRecurso);
        Assert.AreEqual(0, recurso.CantidadEnUso); 
        Assert.AreEqual(null, recurso.ProyectoAlQuePertenece);
    }
    
    [TestMethod]
    public void CreoRecursoConParametrosCorrectosConProyecto()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        bool sePuedeCompartir = false;
        int cantidadDelRecurso = 5; 
        Proyecto? proyecto = new Proyecto("Proyecto", "Descripcion", DateTime.Now);

        Recurso recurso = new Recurso(nombre, tipo, descripcion, sePuedeCompartir, cantidadDelRecurso, proyecto);

        Assert.AreEqual(nombre, recurso.Nombre);
        Assert.AreEqual(tipo, recurso.Tipo);
        Assert.AreEqual(descripcion, recurso.Descripcion);
        Assert.AreEqual(sePuedeCompartir, recurso.SePuedeCompartir);
        Assert.AreEqual(cantidadDelRecurso, recurso.CantidadDelRecurso);
        Assert.AreEqual(0, recurso.CantidadEnUso); 
        Assert.AreEqual(proyecto, recurso.ProyectoAlQuePertenece);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreoRecursoConNombreVacío()
    {
        Recurso recurso = new Recurso("", "Vehiculo", "Auto de la empresa", false, 5, null);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreoRecursoSinTipo()
    {
        Recurso recurso = new Recurso("Auto", "", "Auto de la empresa", false, 5, null);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreoRecursoConCantidadDelRecursoIgualA0()
    {
        Recurso recurso = new Recurso("Auto", "Vechiculo", "Auto de la empresa", false, 0, null);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreoRecursoConCantidadDelRecursoMenorA0()
    {
        Recurso recurso = new Recurso("Auto", "Vechiculo", "Auto de la empresa", false, -8, null);
    }
    
    [TestMethod]
    public void RecursosConsecutivosConIDCorrecta()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion, false, 1, null);
        Assert.AreEqual(1, recurso.Id);
        
        string nombre2 = "Auto2";
        string tipo2 = "Vehiculo";
        string descripcion2 = "Segundo auto de la empresa";
        Recurso recurso2 = new Recurso(nombre2, tipo2, descripcion2, false, 2, null);
        Assert.AreEqual(2,recurso2.Id);
    }
   
    [TestMethod]
    public void RecursoPasaASerExclusivo()
    {
        //TODO
    }
    
    [TestMethod]
    public void RecursoPasaAEstarEnUso()
    {
        //TODO
    }
}