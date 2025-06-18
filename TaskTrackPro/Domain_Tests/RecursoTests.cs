using Domain;
using System.Reflection;

namespace Domain_Tests;

[TestClass]
public class RecursoTests
{
    private static readonly TimeSpan VALID_TIMESPAN = new TimeSpan(6, 5, 0, 0);

    [TestInitialize]
    public void Setup()
    {
        typeof(Recurso)
            .GetField("_contadorId", BindingFlags.Static | BindingFlags.NonPublic)
            ?.SetValue(null, 1);
    }
    
    [TestMethod]
    public void CreoRecursoConParametrosCorrectosSinProyecto()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        bool sePuedeCompartir = false;
        int cantidadDelRecurso = 5; 
        Proyecto? proyecto = null;

        Recurso recurso = new Recurso(nombre, tipo, descripcion, sePuedeCompartir, cantidadDelRecurso);

        Assert.AreEqual(nombre, recurso.Nombre);
        Assert.AreEqual(tipo, recurso.Tipo);
        Assert.AreEqual(descripcion, recurso.Descripcion);
        Assert.AreEqual(sePuedeCompartir, recurso.SePuedeCompartir);
        Assert.AreEqual(cantidadDelRecurso, recurso.CantidadDelRecurso);
        Assert.AreEqual(0, recurso.CantidadEnUso); 
    }
    
    [TestMethod]
    public void CreoRecursoConParametrosCorrectosSinProyectoSobrecargado()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        bool sePuedeCompartir = false;
        int cantidadDelRecurso = 5;

        Recurso recurso = new Recurso(nombre, tipo, descripcion, sePuedeCompartir, cantidadDelRecurso);

        Assert.AreEqual(nombre, recurso.Nombre);
        Assert.AreEqual(tipo, recurso.Tipo);
        Assert.AreEqual(descripcion, recurso.Descripcion);
        Assert.AreEqual(sePuedeCompartir, recurso.SePuedeCompartir);
        Assert.AreEqual(cantidadDelRecurso, recurso.CantidadDelRecurso);
        Assert.AreEqual(0, recurso.CantidadEnUso); 
    }
    
    [TestMethod]
    public void CreoRecursoConParametrosCorrectosConProyecto()
    {
        string nombre = "Auto";
        string tipo = "Vehiculo";
        string descripcion = "Auto de la empresa";
        bool sePuedeCompartir = false;
        int cantidadDelRecurso = 5; 
        Proyecto? proyecto = new Proyecto("Proyecto", "Descripcion", DateTime.Now);

        Recurso recurso = new Recurso(nombre, tipo, descripcion, sePuedeCompartir, cantidadDelRecurso);

        Assert.AreEqual(nombre, recurso.Nombre);
        Assert.AreEqual(tipo, recurso.Tipo);
        Assert.AreEqual(descripcion, recurso.Descripcion);
        Assert.AreEqual(sePuedeCompartir, recurso.SePuedeCompartir);
        Assert.AreEqual(cantidadDelRecurso, recurso.CantidadDelRecurso);
        Assert.AreEqual(0, recurso.CantidadEnUso); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreoRecursoConNombreVacío()
    {
        Recurso recurso = new Recurso("", "Vehiculo", "Auto de la empresa", false, 5);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreoRecursoSinTipo()
    {
        Recurso recurso = new Recurso("Auto", "", "Auto de la empresa", false, 5);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreoRecursoConCantidadDelRecursoIgualA0()
    {
        Recurso recurso = new Recurso("Auto", "Vechiculo", "Auto de la empresa", false, 0);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreoRecursoConCantidadDelRecursoMenorA0()
    {
        Recurso recurso = new Recurso("Auto", "Vechiculo", "Auto de la empresa", false, -8);
    }
    
    [TestMethod]
    public void EstaDisponible_DevuelveTrueSiSePuedeCompartir()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", true, 10);
        Assert.IsTrue(recurso.EstaDisponible(5));
    }

    [TestMethod]
    public void EstaDisponible_DevuelveTrueSiHayCantidadSuficiente()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", false, 10);
        recurso.CantidadEnUso = 3;
        Assert.IsTrue(recurso.EstaDisponible(5));
    }

    [TestMethod]
    public void EstaDisponible_DevuelveFalseSiNoHayCantidadSuficiente()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", false, 10);
        recurso.CantidadEnUso = 8;
        Assert.IsFalse(recurso.EstaDisponible(5)); 
    }
    
    [TestMethod]
    public void ConsumirRecurso_ActualizaCantidadEnUso()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", false, 10);
        recurso.CantidadEnUso = 3;

        recurso.ConsumirRecurso(5);
        Assert.AreEqual(8, recurso.CantidadEnUso);
    }

    [TestMethod]
    public void ConsumirRecurso_NoActualizaSiNoDisponible()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", false, 10);
        recurso.CantidadEnUso = 8;

        recurso.ConsumirRecurso(5); 
        Assert.AreEqual(8, recurso.CantidadEnUso);
    }
    
    [TestMethod]
    public void LiberarRecurso_ActualizaCantidadEnUso()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", false, 10);
        recurso.CantidadEnUso = 8;

        recurso.LiberarRecurso(3);
        Assert.AreEqual(5, recurso.CantidadEnUso);
    }
    
    [TestMethod]
    public void Modificar_RecursoActualizaTodosLosCamposCorrectamente()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", true, 10);

        recurso.Modificar("Notebook", "Tecnología", "Notebook Dell", 7, false);

        Assert.AreEqual("Notebook", recurso.Nombre);
        Assert.AreEqual("Tecnología", recurso.Tipo);
        Assert.AreEqual("Notebook Dell", recurso.Descripcion);
        Assert.IsFalse(recurso.SePuedeCompartir);
        Assert.AreEqual(7, recurso.CantidadDelRecurso);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Modificar_LanzaExcepcionSiNombreEsVacio()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", true, 10);

        recurso.Modificar("", "Tecnología", "Notebook Dell", 7, false);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Modificar_LanzaExcepcionSiTipoEsVacio()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", true, 10);

        recurso.Modificar("Notebook", "", "Notebook Dell", 7, false);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Modificar_LanzaExcepcionSiDescripcionEsVacio()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", true, 10);

        recurso.Modificar("Notebook", "Equipo", "", 7, false);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Modificar_LanzaExcepcionSiCantidadEsMenorOIgualACero()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", true, 10);

        recurso.Modificar("Notebook", "Tecnología", "Notebook Dell", 0, false);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Modificar_LanzaExcepcionSiCantidadEsMenorQueEnUso()
    {
        Recurso recurso = new Recurso("Proyector", "Equipo", "Proyector HD", true, 10);
        recurso.CantidadEnUso = 6;

        recurso.Modificar("Notebook", "Tecnología", "Notebook Dell", 5, false);
    }
    
    [TestMethod]
    public void EstaEnUso_DeberiaRetornarFalse_CuandoCantidadEnUsoEsCero()
    {
        Recurso recurso = new Recurso("Proyector", "Tecnología", "Proyector HD", true, 5);
        bool resultado = recurso.EstaEnUso();
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void EstaEnUso_DeberiaRetornarTrue_CuandoCantidadEnUsoEsMayorACero()
    {
        Recurso recurso = new Recurso("Notebook", "Tecnología", "Notebook Dell", true, 5);
        recurso.ConsumirRecurso(2);
        bool resultado = recurso.EstaEnUso();
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void EstaEnUso_DeberiaRetornarFalse_SiSeLiberaTodoElRecurso()
    {
        Recurso recurso = new Recurso("Monitor", "Tecnología", "Monitor LED", true, 3);
        recurso.ConsumirRecurso(3);
        recurso.LiberarRecurso(3);
        bool resultado = recurso.EstaEnUso();
        Assert.IsFalse(resultado);
    }

}