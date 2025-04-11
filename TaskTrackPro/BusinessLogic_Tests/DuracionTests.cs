namespace BusinessLogic_Tests
{
    [TestClass]
    public class DuracionTests
    {
        [TestMethod]
        public void ConstructorConDatos()
        {
            Duracion duracion = new Duracion(5, TipoDuracion.Dias);
            Assert.IsNotNull(duracion);
        }
    }
}


