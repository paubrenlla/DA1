using BusinessLogic;
using System.Reflection;


namespace BusinessLogic_Tests;

[TestClass]
public class ProyectoTests
{
    private static readonly TimeSpan VALID_TIMESPAN = new TimeSpan(6, 5, 0, 0);
    
    [TestInitialize]
    public void Setup()
    {
        typeof(Proyecto)
            .GetField("_contadorId", BindingFlags.Static | BindingFlags.NonPublic)
            ?.SetValue(null, 1);
    }
    
    [TestMethod]
    public void ProyectoConstructorConDatosCorrectos()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jej";
        DateTime fechaInicio = DateTime.Today;
        

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);

        Assert.AreEqual(nombre, proyecto.Nombre);
        Assert.AreEqual(descripcion, proyecto.Descripcion);
        Assert.AreEqual(fechaInicio, proyecto.FechaInicio);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoConstructorConDescripcionLarga()
    {
        string nombre = "Proyecto A";
        string descripcionLarga = new string('a', 401);
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcionLarga, fechaInicio);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoConstructorSinNombre()
    {
        string nombre = "";
        string descripcion = "Este proyecto no tiene nombre omg";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoConstructorConFechaInicioIncorrecta()
    {
        string nombre = "Proyecto A";
        string descripcion = "En esta fecha salió el Xenoblade";
        DateTime fechaInicio = new DateTime(2010, 6, 10);

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
    }

    [TestMethod]
    public void ProyectosConsecutivosConIDCorrecta()
    {
        string nombre = "Proyecto A";
        string descripcion = "este proyecto deberia tener ID 1";
        DateTime fechaInicio = DateTime.Today;
        
        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        Assert.AreEqual(1, proyecto.Id);
        
        nombre = "Proyecto B";
        descripcion = "este proyecto deberia tener ID 2";
        
        Proyecto proyecto2 = new Proyecto(nombre, descripcion, fechaInicio);
        
        Assert.AreEqual(2,proyecto2.Id);
    }

    [TestMethod]
    public void AgregarRecursoAlProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        Recurso recurso = new Recurso("Auto","Vehiculo","Auto de la empresa",false,1);
        proyecto.agregarRecurso(recurso);
        Assert.AreEqual(1, proyecto.RecursosAsociados.Count);
        Assert.AreEqual(recurso, proyecto.RecursosAsociados[0]);
    }    
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarRecursoQueYaExisteEnElProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);

        Recurso recurso = new Recurso("Auto","Vehiculo","Auto de la empresa",false,1);
        
        proyecto.agregarRecurso(recurso);
        proyecto.agregarRecurso(recurso);
    }
    
    [TestMethod]
    public void AgregarTareaAlProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);

        var tarea = new Tarea("Tarea de prueba", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        
        proyecto.agregarTarea(tarea);
        
        Assert.AreEqual(1, proyecto.TareasAsociadas.Count);
        Assert.AreSame(tarea, proyecto.TareasAsociadas[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarTareaQueYaExisteEnElProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);

        var tarea = new Tarea("Tarea de prueba", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        
        proyecto.agregarTarea(tarea);
        proyecto.agregarTarea(tarea);
    }
    
    [TestMethod]
    
    public void EliminarTareaDelProyectoConVariasTareas()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        var tarea = new Tarea("Tarea a eliminar", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        var tarea2 = new Tarea("Otra tarea a eliminar", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        var tarea3 = new Tarea("Ootra tarea a eliminar", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        var tarea4 = new Tarea("Oootra tarea a eliminar", "Descripción", DateTime.Today, VALID_TIMESPAN, false);

        proyecto.agregarTarea(tarea);
        proyecto.agregarTarea(tarea2);
        proyecto.agregarTarea(tarea3);
        proyecto.agregarTarea(tarea4);
        Assert.AreEqual(4, proyecto.TareasAsociadas.Count);

        proyecto.eliminarTarea(tarea);

        Assert.AreEqual(3, proyecto.TareasAsociadas.Count);
        Assert.IsFalse(proyecto.TareasAsociadas.Contains(tarea));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarTareaQueNoExisteEnElProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        var tarea = new Tarea("Tarea a eliminar", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        
        proyecto.eliminarTarea(tarea);
    }
    
    [TestMethod]
    public void AgregarMiembroAProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        Usuario user = new Usuario("gandalf@gmail.com", "Gandalf", "El Gris", "ganadlfSape123!", new DateTime(2000, 01, 01));

        proyecto.agregarMiembro(user);
        
        Assert.AreEqual(1, proyecto.Miembros.Count);
        Assert.AreSame(user, proyecto.Miembros[0]);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarMiembroYaExisteEnProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        Usuario user = new Usuario("gandalf@gmail.com", "Gandalf", "El Gris", "ganadlfsape123", DateTime.Today);

        proyecto.agregarMiembro(user);
        proyecto.agregarMiembro(user);
    }

    [TestMethod]
    public void EliminarMiembroDeProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        Usuario user = new Usuario("gandalf@gmail.com", "Gandalf", "El Gris", "ganadlfSape123!", new DateTime(2000, 01, 01));
        
        proyecto.agregarMiembro(user);
        Assert.AreEqual(1, proyecto.Miembros.Count);
        Assert.AreSame(user, proyecto.Miembros[0]);
        
        proyecto.eliminarMiembro(user);
        Assert.AreEqual(0, proyecto.Miembros.Count);
        Assert.IsFalse(proyecto.Miembros.Contains(user));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarMiembroQueNoExisteEnElProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        Usuario user = new Usuario("gandalf@gmail.com", "Gandalf", "El Gris", "ganadlfsape123", DateTime.Today);

        proyecto.eliminarMiembro(user);
    }

}


