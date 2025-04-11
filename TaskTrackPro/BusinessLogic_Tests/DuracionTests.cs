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

public enum TipoDuracion
{
    Dias,
    Horas
}

public class Duracion
{
    private int Cantidad { get; set; }
    private TipoDuracion Tipo { get; set; }

    public Duracion(int cantidad, TipoDuracion tipo)
    {
        Cantidad = cantidad;
        Tipo = tipo;
    }
}


