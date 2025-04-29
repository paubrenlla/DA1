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
        Recurso recurso = new Recurso(nombre, tipo, descripcion);
        Assert.AreEqual(nombre,recurso.Nombre);
        Assert.AreEqual(tipo, recurso.Tipo);
        Assert.AreEqual(descripcion, recurso.Descripcion);
    }
    
    [TestMethod]
    public void RecursosConsecutivosConIDCorrecta()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        Recurso recurso = new Recurso(nombre, tipo, descripcion);
        Assert.AreEqual(1, recurso.Id);
        
        string nombre2 = "Auto2";
        string tipo2 = "Vehiculo";
        string descripcion2 = "Segundo auto de la empresa";
        Recurso recurso2 = new Recurso(nombre2, tipo2, descripcion2);
        Assert.AreEqual(2,recurso2.Id);
    }
    
    
}