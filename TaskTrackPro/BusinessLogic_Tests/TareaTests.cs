using BusinessLogic;
using BusinessLogic.Enums;

namespace BusinessLogic_Tests
{
    [TestClass]
    public class TareaTests
    {
        private static readonly TimeSpan VALID_TIMESPAN = new TimeSpan(6, 5, 0, 0);
        private static readonly Recurso RECURSO_VALIDO = new Recurso("Computadora", "tipo", "desripcion", false, 8);
        private static readonly DateTime VALID_DATETIME = DateTime.Today;
        private Proyecto PROYECTO_VALIDO;

        [TestInitialize]
        public void TestInitialize()
        {
            PROYECTO_VALIDO = new Proyecto("Proyecto valido", "descripcion", VALID_DATETIME);
        }
        
        [TestCleanup]
        public void TestCleanup()
        {
            PROYECTO_VALIDO = null;
        }
        
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
            Tarea tarea = new Tarea(
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
            Tarea tareaPrincipal = new Tarea(
                "Reinstalar los servidores de la ORT",
                "Reinstalar los servidores de la ORT para la clase de Sistemas Operativos",
                DateTime.Today.AddDays(20),
                VALID_TIMESPAN,
                true
            );

            Tarea tareaDependiente = new Tarea(
                "Configurar red",
                "Configurar la red antes de reinstalar los servidores",
                DateTime.Today.AddDays(10),
                VALID_TIMESPAN,
                false
            );

            PROYECTO_VALIDO.agregarTarea(tareaPrincipal);
            PROYECTO_VALIDO.agregarTarea(tareaDependiente);
            
            tareaPrincipal.AgregarDependencia(tareaDependiente);;
            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tareaPrincipal.EstadoActual.Valor);
            Assert.IsTrue(tareaPrincipal.EstadoActual.Fecha.Value.Date == DateTime.Today);
        }

        [TestMethod]
        public void NuevaTarea_TieneListaDeDependenciasVacia()
        {
            Tarea tarea = new Tarea("Tarea Principal", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
            PROYECTO_VALIDO.agregarTarea(tarea);
            Assert.IsNotNull(tarea.TareasDependencia);
            Assert.AreEqual(0, tarea.TareasDependencia.Count);
        }

        [TestMethod]
        public void AgregarDependencia_DeberiaPonerEstadoBloqueada()
        {
            Tarea tareaPrincipal = new Tarea("Tarea Principal", "Descripción principal", DateTime.Today, VALID_TIMESPAN,
                false);
            Tarea tareaDependencia = new Tarea("Tarea Dependiente", "Descripción dependencia", DateTime.Today,
                VALID_TIMESPAN, false);
            PROYECTO_VALIDO.agregarTarea(tareaPrincipal);
            PROYECTO_VALIDO.agregarTarea(tareaDependencia);

            tareaPrincipal.AgregarDependencia(tareaDependencia);

            Assert.AreEqual(1, tareaPrincipal.TareasDependencia.Count);
            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tareaPrincipal.EstadoActual.Valor);
        }

        [TestMethod]
        public void TodasLasDependenciasTerminadas_DeberiaCambiarAEstadoPendiente()
        {
            Tarea tareaPrincipal = new Tarea("Tarea Principal", "Descripción principal", DateTime.Today, VALID_TIMESPAN, false);
            Tarea tareaDependencia1 = new Tarea("Dependencia 1", "Desc 1", DateTime.Today, VALID_TIMESPAN, false);
            Tarea tareaDependencia2 = new Tarea("Dependencia 2", "Desc 2", DateTime.Today, VALID_TIMESPAN, false);

            PROYECTO_VALIDO.agregarTarea(tareaPrincipal);
            PROYECTO_VALIDO.agregarTarea(tareaDependencia1);
            PROYECTO_VALIDO.agregarTarea(tareaDependencia2);
            
            tareaPrincipal.AgregarDependencia(tareaDependencia1);
            tareaPrincipal.AgregarDependencia(tareaDependencia2);

            tareaDependencia1.MarcarTareaComoCompletada();
            tareaDependencia2.MarcarTareaComoCompletada();

            tareaPrincipal.ActualizarEstado();

            Assert.AreEqual(TipoEstadoTarea.Pendiente, tareaPrincipal.EstadoActual.Valor);
        }

        [TestMethod]
        public void ActualizarEstado_DependenciaAnidadaBloqueada_EstadoBloqueado()
        {
            Proyecto proyecto = new Proyecto("Proyecto valido", "descripcion", VALID_DATETIME);
            
            Tarea tareaPrincipal = new Tarea("Tarea Principal", "Descripción principal", DateTime.Today, VALID_TIMESPAN, false);
            Tarea tareaDependencia1 = new Tarea("Dependencia 1", "Desc 1", DateTime.Today, VALID_TIMESPAN, false);
            Tarea tareaDependencia2 = new Tarea("Dependencia 2", "Desc 2", DateTime.Today, VALID_TIMESPAN, false);
            Tarea tareaDependencia3 = new Tarea("Dependencia 3", "Desc 3", DateTime.Today, VALID_TIMESPAN, false);
            
            proyecto.agregarTarea(tareaPrincipal);
            proyecto.agregarTarea(tareaDependencia1);
            proyecto.agregarTarea(tareaDependencia2);
            proyecto.agregarTarea(tareaDependencia3);
            
            tareaDependencia1.MarcarTareaComoCompletada();

            tareaDependencia2.AgregarDependencia(tareaDependencia3);

            tareaPrincipal.AgregarDependencia(tareaDependencia1);
            tareaPrincipal.AgregarDependencia(tareaDependencia2);

            tareaPrincipal.ActualizarEstado();

            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tareaPrincipal.EstadoActual.Valor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Tarea_DuracionMenorAUnaHora_LanzaExcepcion()
        {
            TimeSpan duracionInvalida = TimeSpan.FromMinutes(59);
            Tarea tarea = new Tarea("Título", "Descripción", DateTime.Now, duracionInvalida, false);
        }

        [TestMethod]
        public void AgregarDependencia_AgregaSucesoraCorrectamente()
        {
            DateTime inicio = DateTime.Today.AddDays(1);
            TimeSpan duracion = TimeSpan.FromHours(2);

            Tarea tarea1 = new Tarea("Tarea 1", "Desc", inicio, duracion, false);
            Tarea tarea2 = new Tarea("Tarea 2", "Desc", inicio.AddHours(3), duracion, false);

            PROYECTO_VALIDO.agregarTarea(tarea1);
            PROYECTO_VALIDO.agregarTarea(tarea2);
            
            tarea2.AgregarDependencia(tarea1);

            Assert.IsTrue(tarea2.TareasDependencia.Contains(tarea1));
            Assert.IsTrue(tarea1.TareasSucesoras.Contains(tarea2));
        }

        [TestMethod]
        public void MarcarTareaComoCompletada_SinSucesoras_EstableceEstadoEfectuadaYFecha()
        {
            Tarea tarea = new Tarea("Tarea sin sucesoras", "Descripción", DateTime.Today, VALID_TIMESPAN, false);

            PROYECTO_VALIDO.agregarTarea(tarea);
            
            tarea.AgregarRecurso(RECURSO_VALIDO, 2);
            tarea.ConsumirRecursos();
            tarea.MarcarTareaComoCompletada();

            Assert.AreEqual(TipoEstadoTarea.Efectuada, tarea.EstadoActual.Valor);
            Assert.IsNotNull(tarea.EstadoActual.Fecha);
            Assert.AreEqual(DateTime.Today, tarea.EstadoActual.Fecha.Value.Date);
            Assert.AreEqual(0, RECURSO_VALIDO.CantidadEnUso);
        }

        [TestMethod]
        public void MarcarTareaComoCompletada_ConSucesoras_ActualizaEstadoDeSucesoras()
        {
            Tarea tareaPrincipal = new Tarea("Tarea principal", "Descripción principal", DateTime.Today, VALID_TIMESPAN,
                false);
            Tarea tareaSucesora = new Tarea("Tarea sucesora", "Descripción sucesora", DateTime.Today, VALID_TIMESPAN,
                false);

            PROYECTO_VALIDO.agregarTarea(tareaPrincipal);
            PROYECTO_VALIDO.agregarTarea(tareaSucesora);
            
            tareaSucesora.AgregarDependencia(tareaPrincipal);
            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tareaSucesora.EstadoActual.Valor);

            tareaPrincipal.MarcarTareaComoCompletada();

            Assert.AreEqual(TipoEstadoTarea.Efectuada, tareaPrincipal.EstadoActual.Valor);
            Assert.AreEqual(TipoEstadoTarea.Pendiente, tareaSucesora.EstadoActual.Valor);
        }
        
        [TestMethod]
        public void MarcarTareaComoCompletada_ConSucesoras_LiberaRecursosYActualizaEstado()
        {
            Tarea tareaPrincipal = new Tarea("Tarea principal", "Descripción principal", DateTime.Today, TimeSpan.FromHours(2), false);
            Tarea tareaSucesora = new Tarea("Tarea sucesora", "Descripción sucesora", DateTime.Today, TimeSpan.FromHours(2), false);
    
            PROYECTO_VALIDO.agregarTarea(tareaPrincipal);
            PROYECTO_VALIDO.agregarTarea(tareaSucesora);
            
            tareaPrincipal.AgregarRecurso(RECURSO_VALIDO, 3);
            tareaPrincipal.ConsumirRecursos();
            tareaSucesora.AgregarDependencia(tareaPrincipal);
    
            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tareaSucesora.EstadoActual.Valor);

            tareaPrincipal.MarcarTareaComoCompletada();

            Assert.AreEqual(TipoEstadoTarea.Efectuada, tareaPrincipal.EstadoActual.Valor);
            Assert.AreEqual(TipoEstadoTarea.Pendiente, tareaSucesora.EstadoActual.Valor);
            Assert.AreEqual(0, RECURSO_VALIDO.CantidadEnUso);
        }
        
        //TODO HACER TEST DE QUE TAREA NO PUEDE TENER FECHA DE INICIO MENOR A FECH DE INICIO DE PROYECTO
        
    
        [TestMethod]
        public void AgregarRecurso_NuevoRecurso_SeAgregaALaLista()
        {
            Tarea tarea = new Tarea("Tarea Test", "Descripción Test", DateTime.Today, TimeSpan.FromHours(2), false);
            
            PROYECTO_VALIDO.agregarTarea(tarea);
            
            tarea.AgregarRecurso(RECURSO_VALIDO, 2);

            Assert.AreEqual(1, tarea.RecursosNecesarios.Count);
            Assert.AreEqual(RECURSO_VALIDO, tarea.RecursosNecesarios[0].Recurso);
            Assert.AreEqual(2, tarea.RecursosNecesarios[0].CantidadNecesaria);
        }

        [TestMethod]
        public void AgregarRecurso_RecursoExistente_IncrementaCantidad()
        {
            Tarea tarea = new Tarea("Tarea Test", "Descripción Test", DateTime.Today, TimeSpan.FromHours(2), false);

            PROYECTO_VALIDO.agregarTarea(tarea);
            
            tarea.AgregarRecurso(RECURSO_VALIDO, 2);
            tarea.AgregarRecurso(RECURSO_VALIDO, 3);

            Assert.AreEqual(1, tarea.RecursosNecesarios.Count);
            Assert.AreEqual(5, tarea.RecursosNecesarios[0].CantidadNecesaria);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AgregarRecurso_CantidadMenorQue1_LanzaExcepcion()
        {
            Tarea tarea = new Tarea("Tarea Test", "Descripción Test", DateTime.Today, TimeSpan.FromHours(2), false);

            tarea.AgregarRecurso(RECURSO_VALIDO, 0);
        }

        [TestMethod]
        public void VerificarRecursosDisponibles_TodosDisponibles_DeberiaRetornarTrue()
        {
            Tarea tarea = new Tarea("Test", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);
            
            PROYECTO_VALIDO.agregarTarea(tarea);
            
            Recurso recurso1 = new Recurso("Computadora", "tipo", "descripcion", false, 5);
            Recurso recurso2 = new Recurso("Proyector", "tipo", "descripcion", false, 2);

            tarea.AgregarRecurso(recurso1, 3);
            tarea.AgregarRecurso(recurso2, 2);

            Console.WriteLine($"Recurso1 disponible: {recurso1.EstaDisponible(3)}");
            Console.WriteLine($"Recurso2 disponible: {recurso2.EstaDisponible(2)}");

            Assert.IsTrue(tarea.VerificarRecursosDisponibles());
        }

        [TestMethod]
        public void VerificarRecursosDisponibles_UnRecursoInsuficiente_DeberiaRetornarFalse()
        {
            Tarea tarea = new Tarea("Test", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);
            
            PROYECTO_VALIDO.agregarTarea(tarea);
            
            Recurso recurso1 = new Recurso("Computadora", "tipo", "descripcion", false, 2);
            Recurso recurso2 = new Recurso("Proyector", "tipo", "descripcion", false, 1);

            tarea.AgregarRecurso(recurso1, 2);
            tarea.AgregarRecurso(recurso2, 2);

            Assert.IsFalse(tarea.VerificarRecursosDisponibles());
        }

        [TestMethod]
        public void VerificarRecursosDisponibles_NingunRecursoAgregado_DeberiaRetornarTrue()
        {
            Tarea tarea = new Tarea("Test", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);

            Assert.IsTrue(tarea.VerificarRecursosDisponibles()); // No hay restricciones, debería ser válido
        }

        [TestMethod]
        public void VerificarRecursosDisponibles_CantidadExacta_DeberiaRetornarTrue()
        {
            Tarea tarea = new Tarea("Test", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);

            PROYECTO_VALIDO.agregarTarea(tarea);
            
            tarea.AgregarRecurso(RECURSO_VALIDO, 3); // Disponible exactamente la misma cantidad requerida

            Assert.IsTrue(tarea.VerificarRecursosDisponibles());
        }

        [TestMethod]
        public void VerificarRecursosDisponibles_CantidadMayorALaDisponible_DeberiaRetornarFalse()
        {
            Tarea tarea = new Tarea("Test", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);

            tarea.AgregarRecurso(RECURSO_VALIDO, 10);

            Assert.IsFalse(tarea.VerificarRecursosDisponibles());
        }
        
        [TestMethod]
        public void ConsumirRecursos_RecursosDisponibles_SeReducenCorrectamente()
        {
            Tarea tarea = new Tarea("Test", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);
            
            PROYECTO_VALIDO.agregarTarea(tarea);
            
            Recurso recurso1 = new Recurso("Computadora", "tipo", "descripcion", false, 5);
            Recurso recurso2 = new Recurso("Proyector", "tipo", "descripcion", false, 3);

            tarea.AgregarRecurso(recurso1, 2);
            tarea.AgregarRecurso(recurso2, 3);

            tarea.ConsumirRecursos();

            Assert.AreEqual(2, recurso1.CantidadEnUso);
            Assert.AreEqual(3, recurso2.CantidadEnUso);
        }

        [TestMethod]
        public void LiberarRecursos_RecursosConsumidos_SeRestauranCorrectamente()
        {
            Tarea tarea = new Tarea("Test", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);

            PROYECTO_VALIDO.agregarTarea(tarea);
            
            Recurso recurso1 = new Recurso("Computadora", "tipo", "descripcion", false, 5);
            Recurso recurso2 = new Recurso("Proyector", "tipo", "descripcion", false, 3);

            tarea.AgregarRecurso(recurso1, 2);
            tarea.AgregarRecurso(recurso2, 3);

            tarea.ConsumirRecursos();
            tarea.LiberarRecursos();

            Assert.AreEqual(0, recurso1.CantidadEnUso);
            Assert.AreEqual(0, recurso2.CantidadEnUso);
        }

        [TestMethod]
        public void ConsumirRecursos_SinRecursosAsignados_NoCambiaNada()
        {
            Tarea tarea = new Tarea("Test", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);
            tarea.ConsumirRecursos();

            Assert.IsTrue(tarea.VerificarRecursosDisponibles());
        }
        
        [TestMethod]
        public void MarcarTareaComoEjecutandose_ConDependenciasYRecursos_ActualizaEstadoYConsumeRecursos()
        {
            Tarea tarea = new Tarea("Tarea Ejecutada", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);
            Tarea tareaDependencia = new Tarea("Dependencia", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);
            
            PROYECTO_VALIDO.agregarTarea(tarea);
            PROYECTO_VALIDO.agregarTarea(tareaDependencia);
            
            Recurso recurso = new Recurso("Servidor", "tipo", "descripcion", false, 5);

            tarea.AgregarDependencia(tareaDependencia);
            tareaDependencia.MarcarTareaComoCompletada();
            tarea.AgregarRecurso(recurso, 3);

            tarea.MarcarTareaComoEjecutandose();

            Assert.AreEqual(TipoEstadoTarea.Ejecutandose, tarea.EstadoActual.Valor);
            Assert.AreEqual(3, recurso.CantidadEnUso);
        }

        [TestMethod]
        public void MarcarTareaComoEjecutandose_ConDependenciasIncompletas_NoCambiaEstadoNiConsumeRecursos()
        {
            Tarea tarea = new Tarea("Tarea", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);
            Tarea tareaDependencia = new Tarea("Dependencia", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);
            
            PROYECTO_VALIDO.agregarTarea(tarea);
            PROYECTO_VALIDO.agregarTarea(tareaDependencia);
            
            Recurso recurso = new Recurso("Laptop", "tipo", "descripcion", false, 5);

            tarea.AgregarDependencia(tareaDependencia);
            tarea.AgregarRecurso(recurso, 3);

            tarea.MarcarTareaComoEjecutandose();

            Assert.AreNotEqual(TipoEstadoTarea.Ejecutandose, tarea.EstadoActual.Valor);
            Assert.AreEqual(0, recurso.CantidadEnUso);
        }

        [TestMethod]
        public void MarcarTareaComoEjecutandose_SinRecursosDisponibles_NoCambiaEstadoNiConsumeRecursos()
        {
            Tarea tarea = new Tarea("Tarea", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);
            
            PROYECTO_VALIDO.agregarTarea(tarea);

            Recurso recurso = new Recurso("Laptop", "tipo", "descripcion", false, 2);

            tarea.AgregarRecurso(recurso, 5);

            tarea.MarcarTareaComoEjecutandose();

            Assert.AreNotEqual(TipoEstadoTarea.Ejecutandose, tarea.EstadoActual.Valor);
            Assert.AreEqual(0, recurso.CantidadEnUso);
        }

        [TestMethod]
        public void MarcarTareaComoEjecutandose_SinDependenciasNiRecursos_CambiaEstadoPeroNoConsumeNada()
        {
            Tarea tarea = new Tarea("Tarea", "Desc", DateTime.Today, TimeSpan.FromHours(2), false);

            PROYECTO_VALIDO.agregarTarea(tarea);

            tarea.MarcarTareaComoEjecutandose();

            Assert.AreEqual(TipoEstadoTarea.Ejecutandose, tarea.EstadoActual.Valor);
        }

        [TestMethod]
        public void ReevaluarTareasPosteriores_ConSucesorasBloqueadas_DeberiaActualizarEstadoAPendiente()
        {
            Tarea tareaPrincipal = new Tarea("Tarea Principal", "Descripción", DateTime.Today, TimeSpan.FromHours(2), false);
            Tarea tareaSucesora = new Tarea("Tarea Sucesora", "Descripción sucesora", DateTime.Today, TimeSpan.FromHours(2), false);

            PROYECTO_VALIDO.agregarTarea(tareaPrincipal);
            PROYECTO_VALIDO.agregarTarea(tareaSucesora);
            
            tareaSucesora.AgregarDependencia(tareaPrincipal); 

            tareaPrincipal.MarcarTareaComoCompletada();
            
            Assert.AreEqual(TipoEstadoTarea.Pendiente, tareaSucesora.EstadoActual.Valor);
        }

        [TestMethod]
        public void ReevaluarTareasPosteriores_ConSucesorasPendientes_NoCambiaEstado()
        {
            Tarea tareaPrincipal = new Tarea("Tarea Principal", "Descripción", DateTime.Today, TimeSpan.FromHours(2), false);
            Tarea tareaSucesora = new Tarea("Tarea Sucesora", "Descripción sucesora", DateTime.Today, TimeSpan.FromHours(2), false);

            PROYECTO_VALIDO.agregarTarea(tareaPrincipal);
            PROYECTO_VALIDO.agregarTarea(tareaSucesora);
            
            tareaSucesora.AgregarDependencia(tareaPrincipal);
            
            Assert.AreEqual(TipoEstadoTarea.Pendiente, tareaPrincipal.EstadoActual.Valor);
            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tareaSucesora.EstadoActual.Valor);
        }

        [TestMethod]
        public void ReevaluarTareasPosteriores_ConSucesorasConDependenciasNoCompletadas_NoCambiaEstadoSucesora()
        {
            Tarea tareaPrincipal = new Tarea("Tarea Principal", "Descripción", DateTime.Today, TimeSpan.FromHours(2), false);
            Tarea tareaDependencia = new Tarea("Dependencia Intermedia", "Descripción dependencia", DateTime.Today, TimeSpan.FromHours(2), false);
            Tarea tareaSucesora = new Tarea("Tarea Sucesora", "Descripción sucesora", DateTime.Today, TimeSpan.FromHours(2), false);

            PROYECTO_VALIDO.agregarTarea(tareaPrincipal);
            PROYECTO_VALIDO.agregarTarea(tareaSucesora);
            PROYECTO_VALIDO.agregarTarea(tareaDependencia);

            tareaSucesora.AgregarDependencia(tareaPrincipal);
            tareaPrincipal.AgregarDependencia(tareaDependencia);

            tareaDependencia.MarcarTareaComoCompletada();

            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tareaSucesora.EstadoActual.Valor);
            Assert.AreEqual(TipoEstadoTarea.Pendiente, tareaPrincipal.EstadoActual.Valor);
        }
        
        [TestMethod]
        public void TareasSeDesbloqueanAlCompletarDependencias()
        {
            Tarea tarea1 = new Tarea("Tarea 1", "Inicio del flujo", DateTime.Now, TimeSpan.FromHours(2), false);
            Tarea tarea2 = new Tarea("Tarea 2", "Segunda etapa", DateTime.Now, TimeSpan.FromHours(2), false);
            Tarea tarea3 = new Tarea("Tarea 3", "Final del flujo", DateTime.Now, TimeSpan.FromHours(2), false);

            PROYECTO_VALIDO.agregarTarea(tarea1);
            PROYECTO_VALIDO.agregarTarea(tarea2);
            PROYECTO_VALIDO.agregarTarea(tarea3);
            
            tarea2.AgregarDependencia(tarea1);
            tarea3.AgregarDependencia(tarea2);

            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tarea2.EstadoActual.Valor);
            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tarea3.EstadoActual.Valor);

            tarea1.MarcarTareaComoCompletada();

            Assert.AreEqual(TipoEstadoTarea.Pendiente, tarea2.EstadoActual.Valor);
            Assert.AreEqual(TipoEstadoTarea.Bloqueada, tarea3.EstadoActual.Valor);

            tarea2.MarcarTareaComoCompletada();

            Assert.AreEqual(TipoEstadoTarea.Pendiente, tarea3.EstadoActual.Valor);
        }
        
        [TestMethod]
        public void Modificar_CambiaTitulo()
        {
            Tarea tarea = new Tarea("Título original", "Desc", VALID_DATETIME, VALID_TIMESPAN, false);
            
            PROYECTO_VALIDO.agregarTarea(tarea);
            
            string nuevoTitulo = "Título nuevo";

            tarea.Modificar(nuevoTitulo, tarea.Descripcion, tarea.FechaInicio, tarea.Duracion);

            Assert.AreEqual(nuevoTitulo, tarea.Titulo);
        }

        [TestMethod]
        public void Modificar_CambiaDescripcion()
        {
            Tarea tarea = new Tarea("Título", "Descripción original", VALID_DATETIME, VALID_TIMESPAN, false);
            
            PROYECTO_VALIDO.agregarTarea(tarea);

            string nuevaDescripcion = "Descripción nueva";

            tarea.Modificar(tarea.Titulo, nuevaDescripcion, tarea.FechaInicio, tarea.Duracion);

            Assert.AreEqual(nuevaDescripcion, tarea.Descripcion);
        }

        [TestMethod]
        public void Modificar_CambiaFechaInicio()
        {
            DateTime nuevaFecha = VALID_DATETIME.AddDays(5);
            Tarea tarea = new Tarea("Título", "Descripción", VALID_DATETIME, VALID_TIMESPAN, false);

            PROYECTO_VALIDO.agregarTarea(tarea);
            
            tarea.Modificar(tarea.Titulo, tarea.Descripcion, nuevaFecha, tarea.Duracion);

            Assert.AreEqual(nuevaFecha, tarea.FechaInicio);
        }

        [TestMethod]
        public void Modificar_CambiaDuracion()
        {
            TimeSpan nuevaDuracion = TimeSpan.FromHours(12);
            Tarea tarea = new Tarea("Título", "Descripción", VALID_DATETIME, VALID_TIMESPAN, false);

            PROYECTO_VALIDO.agregarTarea(tarea);
            
            tarea.Modificar(tarea.Titulo, tarea.Descripcion, tarea.FechaInicio, nuevaDuracion);

            Assert.AreEqual(nuevaDuracion, tarea.Duracion);
        }

        [TestMethod]
        public void Modificar_CambiaTodosLosValores()
        {
            string nuevoTitulo = "Título actualizado";
            string nuevaDescripcion = "Descripción actualizada";
            DateTime nuevaFecha = VALID_DATETIME.AddDays(3);
            TimeSpan nuevaDuracion = TimeSpan.FromDays(2);

            Tarea tarea = new Tarea("Original", "Original", VALID_DATETIME, VALID_TIMESPAN, false);

            PROYECTO_VALIDO.agregarTarea(tarea);
            
            tarea.Modificar(nuevoTitulo, nuevaDescripcion, nuevaFecha, nuevaDuracion);

            Assert.AreEqual(nuevoTitulo, tarea.Titulo);
            Assert.AreEqual(nuevaDescripcion, tarea.Descripcion);
            Assert.AreEqual(nuevaFecha, tarea.FechaInicio);
            Assert.AreEqual(nuevaDuracion, tarea.Duracion);
        }
    }
}