using BusinessLogic;

namespace BusinessLogic_Tests;

[TestClass]
public class RecursoTests
{
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
    
}