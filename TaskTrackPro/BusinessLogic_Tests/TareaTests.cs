using BusinessLogic;

namespace BusinessLogic_Tests
{
    [TestClass]
    public class TareaTests
    {
        [TestMethod]
        public void ConstructorConDatosValidos_CreaTareaCorrectamente()
        {
            string titulo = "Leer artículo";
            string descripcion = "Leer artículo de investigación para clase";
            DateTime? fechaInicio = DateTime.Today;
            Duracion duracion = new Duracion(2, TipoDuracion.Dias);
            bool esCritica = false;

            Tarea tarea = new Tarea(titulo, descripcion, fechaInicio, duracion, esCritica);

            Assert.AreEqual(titulo, tarea.Titulo);
            Assert.AreEqual(descripcion, tarea.Descripcion);
            Assert.AreEqual(fechaInicio, tarea.FechaInicio);
            Assert.AreEqual(duracion, tarea.Duracion);
            Assert.AreEqual(esCritica, tarea.EsCritica);
            Assert.AreEqual(EstadoTarea.Pendiente, tarea.Estado.Valor);
        }
    }
}
