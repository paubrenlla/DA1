using BusinessLogic;

namespace BusinessLogic_Tests;

[TestClass]
public class GeneradorContraseñaTests
{
    [TestMethod] public void GenerarContraseña_DatosValidos()
    {
        string password = GeneradorContraseña.GenerarContraseña(12);
        
        Usuario.ValidarContraseña(password);
        
        Assert.IsNotNull(password);
        Assert.IsTrue(password.Length >= 8);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GenerarContraseña_DebeFallar_MenorCantidadCaracteres()
    {
        GeneradorContraseña.GenerarContraseña(3);
    }

}