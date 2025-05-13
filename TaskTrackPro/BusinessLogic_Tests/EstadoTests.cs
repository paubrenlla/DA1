using BusinessLogic;
using BusinessLogic.Enums;
using System;

namespace BusinessLogic_Tests
{
    [TestClass]
    public class EstadoTests
    {
        private static readonly TipoEstadoTarea TIPO_ESTADO_VALIDO = TipoEstadoTarea.Pendiente;

        [TestMethod]
        public void Constructor_ValorEfectuada_SeAsignaFechaCorrecta()
        {
            var estado = new Estado(TipoEstadoTarea.Efectuada);
            Assert.AreEqual(TipoEstadoTarea.Efectuada, estado.Valor);
            Assert.IsNotNull(estado.Fecha);
        }

        [TestMethod]
        public void Constructor_ValorPendiente_SeAsignaValorCorrectoYFechaNula()
        {
            var estado = new Estado(TipoEstadoTarea.Pendiente);
            Assert.AreEqual(TipoEstadoTarea.Pendiente, estado.Valor);
            Assert.IsNull(estado.Fecha);
        }

        [TestMethod]
        public void MarcarComoEfectuada_SeCambiaValorYFecha()
        {
            var estado = new Estado(TipoEstadoTarea.Pendiente);
            DateTime fechaEsperada = new DateTime(2025, 5, 13);
            estado.MarcarComoEfectuada(fechaEsperada);
            Assert.AreEqual(TipoEstadoTarea.Efectuada, estado.Valor);
            Assert.AreEqual(fechaEsperada, estado.Fecha);
        }

        [TestMethod]
        public void ToString_ValorEfectuada_DevuelveCadenaCorrecta()
        {
            var estado = new Estado(TipoEstadoTarea.Efectuada);
            string resultado = estado.ToString();
            Assert.IsTrue(resultado.Contains("Estado: Efectuada"));
            Assert.IsTrue(resultado.Contains("Fecha"));
        }

        [TestMethod]
        public void ToString_ValorPendiente_DevuelveCadenaCorrecta()
        {
            var estado = new Estado(TipoEstadoTarea.Pendiente);
            string resultado = estado.ToString();
            Assert.AreEqual("Estado: Pendiente", resultado);
        }
    }
}