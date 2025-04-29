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
    public void CreoRecursoConParametrosCorrectos()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion, false);
        Assert.AreEqual(nombre,recurso.Nombre);
        Assert.AreEqual(tipo, recurso.Tipo);
        Assert.AreEqual(descripcion, recurso.Descripcion);
        Assert.AreEqual(false, recurso.sePuedeCompartir);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreoRecursoConNombreVacío()
    {
        Recurso recurso = new Recurso("", "Vehiculo", "Auto de la empresa", false);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreoRecursoSinTipo()
    {
        Recurso recurso = new Recurso("Auto", "", "Auto de la empresa", false);
    }
    
    [TestMethod]
    public void RecursosConsecutivosConIDCorrecta()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion, false);
        Assert.AreEqual(1, recurso.Id);
        
        string nombre2 = "Auto2";
        string tipo2 = "Vehiculo";
        string descripcion2 = "Segundo auto de la empresa";
        Recurso recurso2 = new Recurso(nombre2, tipo2, descripcion2, false);
        Assert.AreEqual(2,recurso2.Id);
    }
    
    
}