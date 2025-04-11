using BusinessLogic;

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

        [TestMethod]
        public void ConstructorConCantidadInvalida_LanzaExcepcion()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var duracion = new Duracion(-5, TipoDuracion.Horas);
            });
        }
        [TestMethod]
        public void ConstructorConTipoInvalido_LanzaArgumentException()
        {
            TipoDuracion tipoInvalido = (TipoDuracion)99; // un valor que no está definido en el enum
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var duracion = new Duracion(5, tipoInvalido);
            });
        }
        
        
    }
}





