using BusinessLogic;

namespace BusinessLogic_Tests
{
    [TestClass]
    public class TareaTests
    {
        private static readonly TimeSpan VALID_TIMESPAN = new TimeSpan(6, 5, 0, 0);

        [TestMethod]
        public void ConstructorConDatosValidos_CreaTareaCorrectamente()
        {
            string titulo = "Leer artículo";
            string descripcion = "Leer artículo de investigación para clase";
            DateTime fechaInicio = DateTime.Today;
            TimeSpan duracion = VALID_TIMESPAN;
            bool esCritica = false;

            Tarea tarea1 = new Tarea(titulo, descripcion, fechaInicio, duracion, esCritica);

            Assert.AreEqual(titulo, tarea1.Titulo);
            Assert.AreEqual(descripcion, tarea1.Descripcion);
            Assert.AreEqual(fechaInicio, tarea1.FechaInicio);
            Assert.AreEqual(duracion, tarea1.Duracion);
            Assert.AreEqual(esCritica, tarea1.EsCritica);
            Assert.AreEqual(TipoEstadoTarea.Pendiente, tarea1.EstadoActual.Valor);
        }

        [TestMethod]
        public void ConstructorConTituloNulo_DeberiaLanzarExcepcion()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                Tarea tarea = new Tarea(null, "Descripción", DateTime.Today, VALID_TIMESPAN, true);
            });
        }

        [TestMethod]
        public void ConstructorConTituloVacio_DeberiaLanzarExcepcion()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                Tarea tarea = new Tarea("", "Descripción", DateTime.Today, VALID_TIMESPAN, true);
            });
        }

        [TestMethod]
        public void ConstructorConDescripcionNula_DeberiaLanzarExcepcion()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                Tarea tarea = new Tarea("Tarea1", null, DateTime.Today, VALID_TIMESPAN, true);
            });
        }

        [TestMethod]
        public void ConstructorConDescripcionVacio_DeberiaLanzarExcepcion()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                Tarea tarea = new Tarea("Tarea1", "", DateTime.Today, VALID_TIMESPAN, true);
            });
        }

        [TestMethod]
        public void MarcarComoEfectuada_DeberiaCambiarEstadoYAsignarFecha()
        {
            var tarea = new Tarea(
                "Leer artículo",
                "Leer artículo de investigación para clase",
                DateTime.Today,
                VALID_TIMESPAN,
                false
            );
            tarea.EstadoActual.MarcarComoEfectuada(DateTime.Today);
            Assert.AreEqual(TipoEstadoTarea.Efectuada, tarea.EstadoActual.Valor);
            Assert.IsNotNull(tarea.EstadoActual.Fecha);
            Assert.IsTrue(tarea.EstadoActual.Fecha.Value.Date == DateTime.Today);
        }
        
        [TestMethod]
        public void ModificarElEstadoDeTarea()
            // Se modifica el estado de Pendiente a Bloqueada y se valida que la fecha es correcta
        {
            var tarea = new Tarea(
                "Reinstalar los servidores de la ORT",
                "Reinstalar los servidores de la ORT para la clase de Sistemas Operativos",
                DateTime.Today.AddDays(20),
                VALID_TIMESPAN,
                true
            );
            tarea.modificarEstado(TipoEstadoTarea.Bloqueada, DateTime.Today);
            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tarea.EstadoActual.Valor);
            Assert.IsTrue(tarea.EstadoActual.Fecha.Value.Date == DateTime.Today);
        }
        
        [TestMethod]
        public void NuevaTarea_TieneListaDeDependenciasVacia()
        {
            var tarea = new Tarea("Tarea Principal", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
            Assert.IsNotNull(tarea.TareasDependencia);
            Assert.AreEqual(0, tarea.TareasDependencia.Count);
        }
        
        [TestMethod]
        public void AgregarDependencia_DeberiaPonerEstadoBloqueada()
        {
            var tareaPrincipal = new Tarea("Tarea Principal", "Descripción principal", DateTime.Today, VALID_TIMESPAN, false);
            var tareaDependencia = new Tarea("Tarea Dependiente", "Descripción dependencia", DateTime.Today, VALID_TIMESPAN, false);

            tareaPrincipal.AgregarDependencia(tareaDependencia);

            Assert.AreEqual(1, tareaPrincipal.TareasDependencia.Count);
            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tareaPrincipal.EstadoActual.Valor);
        }
        
        [TestMethod]
        public void TodasLasDependenciasTerminadas_DeberiaCambiarAEstadoPendiente()
        {
            var tareaPrincipal = new Tarea("Tarea Principal", "Descripción principal", DateTime.Today, VALID_TIMESPAN, false);
            var tareaDependencia1 = new Tarea("Dependencia 1", "Desc 1", DateTime.Today, VALID_TIMESPAN, false);
            var tareaDependencia2 = new Tarea("Dependencia 2", "Desc 2", DateTime.Today, VALID_TIMESPAN, false);

            tareaPrincipal.AgregarDependencia(tareaDependencia1);
            tareaPrincipal.AgregarDependencia(tareaDependencia2);

            // Simulamos que las dependencias directas se terminan
            tareaDependencia1.modificarEstado(TipoEstadoTarea.Efectuada, DateTime.Today);
            tareaDependencia2.modificarEstado(TipoEstadoTarea.Efectuada, DateTime.Today);

            tareaPrincipal.ActualizarEstadoSegunDependencias();

            Assert.AreEqual(TipoEstadoTarea.Pendiente, tareaPrincipal.EstadoActual.Valor);
        }
        
        [TestMethod]
        public void ActualizarEstado_DependenciaAnidadaBloqueada_EstadoBloqueado()
        {
            var tareaPrincipal = new Tarea("Tarea Principal", "Descripción principal", DateTime.Today, VALID_TIMESPAN, false);
            var tareaDependencia1 = new Tarea("Dependencia 1", "Desc 1", DateTime.Today, VALID_TIMESPAN, false);
            var tareaDependencia2 = new Tarea("Dependencia 2", "Desc 2", DateTime.Today, VALID_TIMESPAN, false);
            var tareaDependencia3 = new Tarea("Dependencia 2", "Desc 2", DateTime.Today, VALID_TIMESPAN, false);
           
            tareaDependencia1.modificarEstado(TipoEstadoTarea.Efectuada, DateTime.Today);
            tareaDependencia2.modificarEstado(TipoEstadoTarea.Bloqueada, DateTime.Today);
         
            tareaDependencia2.AgregarDependencia(tareaDependencia3); // Dependencia anidada no efectuada
            tareaPrincipal.AgregarDependencia(tareaDependencia1);
            tareaPrincipal.AgregarDependencia(tareaDependencia2);

            tareaPrincipal.ActualizarEstadoSegunDependencias();
            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tareaPrincipal.EstadoActual.Valor);
        }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Tarea_DuracionMenorAUnaHora_LanzaExcepcion()
    {
    var duracionInvalida = TimeSpan.FromMinutes(59);
    var tarea = new Tarea("Título", "Descripción", DateTime.Now, duracionInvalida, false);
    }

    }
    
    







}
    




