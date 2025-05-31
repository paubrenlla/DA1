using Domain;

namespace Domain_Tests;


[TestClass]
public class AsignacionRecursoTareaTests
{
    private static readonly Recurso RECURSO_VALIDO = new Recurso("Computadora", "tipo", "desripcion", false, 8);
    private static readonly TimeSpan VALID_TIMESPAN = new TimeSpan(6, 5, 0, 0);
    private static readonly Tarea TAREA_VALIDA = new Tarea("Tarea Valida", "descripcion", DateTime.Now, VALID_TIMESPAN, false);

    [TestMethod]
    public void Constructor_ValoresValidos_SeAsignaCorrectamente()
    {
        AsignacionRecursoTarea asignacionRecursoTarea = new AsignacionRecursoTarea(RECURSO_VALIDO, TAREA_VALIDA , 2);

        Assert.AreEqual(RECURSO_VALIDO, asignacionRecursoTarea.Recurso);
        Assert.AreEqual(2, asignacionRecursoTarea.CantidadNecesaria);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_RecursoNulo_DeberiaLanzarExcepcion()
    {
        AsignacionRecursoTarea asignacionRecursoTarea = new AsignacionRecursoTarea(null, TAREA_VALIDA, 5);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_TareaNula_DeberiaLanzarExcepcion()
    {
        AsignacionRecursoTarea asignacionRecursoTarea = new AsignacionRecursoTarea(RECURSO_VALIDO, null, 5);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_CantidadMenorIgualA0_DeberiaLanzarExcepcion()
    {
        AsignacionRecursoTarea asignacionRecursoTarea = new AsignacionRecursoTarea(RECURSO_VALIDO, TAREA_VALIDA, 0);
    }

    [TestMethod]
    public void Modificar_CantidadNuevaCorrecta_ActualizaCantidad()
    {
        AsignacionRecursoTarea asignacion = new AsignacionRecursoTarea(RECURSO_VALIDO, TAREA_VALIDA, 5);
        asignacion.Modificar(7);
        Assert.AreEqual(7, asignacion.CantidadNecesaria);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Modificar_CantidadNuevaMenorOIgualACero_LanzaExcepcion()
    {
        AsignacionRecursoTarea asignacion = new AsignacionRecursoTarea(RECURSO_VALIDO, TAREA_VALIDA, 5);
        asignacion.Modificar(0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Modificar_CantidadNuevaMayorQueCantidadDisponible_LanzaExcepcion()
    {
        AsignacionRecursoTarea asignacion = new AsignacionRecursoTarea(RECURSO_VALIDO, TAREA_VALIDA, 5);
        asignacion.Modificar(15);
    }
}