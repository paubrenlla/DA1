using BusinessLogic;
using System.Reflection;


namespace BusinessLogic_Tests;

[TestClass]
public class ProyectoTests
{
    private static Recurso recurso = new Recurso("Recurso", "Tipo", "Descripcion", true, 5);
    private static readonly TimeSpan VALID_TIMESPAN = new TimeSpan(6, 5, 0, 0);
    
    [TestInitialize]
    public void Setup()
    {
        typeof(Proyecto)
            .GetField("_contadorId", BindingFlags.Static | BindingFlags.NonPublic)
            ?.SetValue(null, 1);
        typeof(Tarea)
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

    [TestMethod]
    public void ListarTareasSinDependencias()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        var tarea = new Tarea("Tarea ", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        var tareaDependencia = new Tarea("Dependencia", "Desc", DateTime.Today, VALID_TIMESPAN, false);
        tarea.AgregarDependencia(tareaDependencia);
        
        proyecto.agregarTarea(tarea);
        proyecto.agregarTarea(tareaDependencia);

        List<Tarea> tareasSinDependencia = proyecto.TareasSinDependencia();
        Assert.AreEqual(1, tareasSinDependencia.Count);
        Assert.AreSame(tareaDependencia, tareasSinDependencia[0]);
        
    }
    
    [TestMethod]
    public void CalcularEarlyTimes_TareaSinPredecesores()
    {
        var hoy = DateTime.Today;
        var proyecto = new Proyecto("Test", "Descripción", hoy);

        var tarea = new Tarea("T1", "Desc", hoy, TimeSpan.FromDays(3), false);

        proyecto.agregarTarea(tarea);

        proyecto.CalcularTiemposTempranos();

        Assert.AreEqual(hoy, tarea.EarlyStart);
        Assert.AreEqual(hoy.AddDays(3), tarea.EarlyFinish);
    }
    
    [TestMethod]
    public void CalcularEarlyTimes_TareaConPredecesor()
    {
        var hoy = DateTime.Today;
        var proyecto = new Proyecto("Test", "Desc", hoy);

        var t1 = new Tarea("T1", "Desc", hoy, TimeSpan.FromDays(3), false);
        var t2 = new Tarea("T2", "Desc", hoy, TimeSpan.FromDays(2), false);

        t2.AgregarDependencia(t1);

        proyecto.agregarTarea(t1);
        proyecto.agregarTarea(t2);

        proyecto.CalcularTiemposTempranos();

        Assert.AreEqual(hoy, t1.EarlyStart);
        Assert.AreEqual(hoy.AddDays(3), t1.EarlyFinish);
    
        Assert.AreEqual(hoy.AddDays(3), t2.EarlyStart);
        Assert.AreEqual(hoy.AddDays(5), t2.EarlyFinish);
    }
    
    [TestMethod]
    public void CalcularTiemposTardios_ConTareasLineales_CalculaCorrectamente()
    {
        var inicio = DateTime.Today.AddDays(1);
        var duracion = TimeSpan.FromHours(2);

        var t1 = new Tarea("T1", "desc", inicio, duracion, false);
        var t2 = new Tarea("T2", "desc", inicio.AddHours(3), duracion, false);
        var t3 = new Tarea("T3", "desc", inicio.AddHours(6), duracion, false);

        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t2);

        var proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.agregarTarea(t1);
        proyecto.agregarTarea(t2);
        proyecto.agregarTarea(t3);

        proyecto.CalcularTiemposTempranos();
        proyecto.CalcularTiemposTardios();

        Assert.AreEqual(t1.EarlyStart, t1.LateStart);
        Assert.AreEqual(t2.EarlyStart, t2.LateStart);
        Assert.AreEqual(t3.EarlyStart, t3.LateStart);
    }
    
    [TestMethod]
    public void CalcularRutaCriticaSoloDevuelvaTareasConHolguraCero()
    {
        DateTime inicio = DateTime.Today.AddDays(1);
        TimeSpan duracion = TimeSpan.FromHours(2);

        Tarea t1 = new Tarea("T1", "desc", inicio, duracion, false);
        Tarea t2 = new Tarea("T2", "desc", inicio, duracion, false);
        Tarea t3 = new Tarea("T3", "desc", inicio, duracion, false);
        Tarea t4 = new Tarea("T4", "desc", inicio, duracion, false);

        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t1);
        t4.AgregarDependencia(t2);
        t4.AgregarDependencia(t3);

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.agregarTarea(t1);
        proyecto.agregarTarea(t2);
        proyecto.agregarTarea(t3);
        proyecto.agregarTarea(t4);

        List<Tarea> rutaCritica = proyecto.CalcularRutaCritica();

        Assert.AreEqual(4, rutaCritica.Count);
        Assert.AreEqual(TimeSpan.Zero, t1.Holgura);
        Assert.AreEqual(TimeSpan.Zero, t2.Holgura);
        Assert.AreEqual(TimeSpan.Zero, t3.Holgura);
        Assert.AreEqual(TimeSpan.Zero, t4.Holgura);
        Assert.IsTrue(t1.EsCritica);
        Assert.IsTrue(t2.EsCritica);
        Assert.IsTrue(t3.EsCritica);
        Assert.IsTrue(t4.EsCritica);
        CollectionAssert.Contains(rutaCritica, t1);
        CollectionAssert.Contains(rutaCritica, t2);
        CollectionAssert.Contains(rutaCritica, t3);
        CollectionAssert.Contains(rutaCritica, t4);
        
    }
    [TestMethod] 
    public void AsignarUsuarioATarea_UsuarioMiembro_TareaEnProyecto_AsignaCorrectamente()
    {

        var proyecto = new Proyecto("Proyecto Test", "Descripción", DateTime.Now);
        var usuario = new Usuario("test@test.com", "Test", "Usuario", "Contra*seña123", DateTime.Now);
        var tarea = new Tarea("Tarea Test", "Descripción", DateTime.Now, TimeSpan.FromHours(5), false);
        proyecto.agregarMiembro(usuario);
        proyecto.agregarTarea(tarea);
        proyecto.AsignarUsuarioATarea(usuario, tarea);
        CollectionAssert.Contains(tarea.UsuariosAsignados.ToList(), usuario);
        
    }
    
    [TestMethod]
    public void AsignarUsuarioATarea_UsuarioNoMiembro_LanzaExcepcion()
    {
        var proyecto = new Proyecto("Proyecto Test", "Descripción", DateTime.Now);
        var usuario = new Usuario("test@test.com", "Test", "Usuario", "Contr*aseña123", DateTime.Now);
        var tarea = new Tarea("Tarea Test", "Descripción", DateTime.Now, TimeSpan.FromHours(5), false);
    
        proyecto.agregarTarea(tarea);

        Assert.ThrowsException<ArgumentException>(() => proyecto.AsignarUsuarioATarea(usuario, tarea));
    }

    [TestMethod]
    public void AsignarUsuarioATarea_UsuarioYaAsignado_LanzaExcepcion()
    {
        var proyecto = new Proyecto("Proyecto Test", "Descripción", DateTime.Now);
        var usuario = new Usuario("test@test.com", "Test", "Usuario", "Contr*aseña123", DateTime.Now);
        var tarea = new Tarea("Tarea Test", "Descripción", DateTime.Now, TimeSpan.FromHours(5), false);
    
        proyecto.agregarMiembro(usuario);
        proyecto.agregarTarea(tarea);
        proyecto.AsignarUsuarioATarea(usuario, tarea);

        Assert.ThrowsException<ArgumentException>(() => proyecto.AsignarUsuarioATarea(usuario, tarea));
    }

    [TestMethod]
    public void AsignarAdminAlProyecto()
    {

        Proyecto proyecto = new Proyecto("Proyecto Test", "Descripción", DateTime.Now);
        Usuario usuario = new Usuario("admin@test.com", "Admin", "User", "paASD*ss1", DateTime.Now);
        proyecto.AsignarAdmin(usuario); 
        Assert.AreEqual("admin@test.com",usuario.Email);
        Assert.IsTrue(proyecto.EsAdmin(usuario));
        Assert.AreEqual(0, proyecto.Miembros.Count);
    }
    
    [TestMethod]
    public void ModificarUnProyectoCorrectamente()
    {
        Proyecto proyecto = new Proyecto("Proyecto Test", "Descripción", DateTime.Now);
        string descNueva = "Nueva DESCRIPCION";
        DateTime fechaNueva = DateTime.Now.AddDays(3);
        proyecto.Modificar(descNueva, fechaNueva);
        
        Assert.AreEqual(descNueva, proyecto.Descripcion);
        Assert.AreEqual(fechaNueva, proyecto.FechaInicio);
    }
    
    [TestMethod]
    public void BuscarTareaPorId_TareaExiste_ReturnsTarea()
    {
        Proyecto proyecto = new Proyecto("Proyecto de prueba", "Descripción del proyecto", DateTime.Now);
        Tarea tarea1 = new Tarea("Tarea 1", "descripcion", DateTime.Now,VALID_TIMESPAN, false);
        Tarea tarea2 = new Tarea("Tarea 2", "descripcion", DateTime.Now,VALID_TIMESPAN, false);

        proyecto.agregarTarea(tarea1);
        proyecto.agregarTarea(tarea2);

        Tarea resultado = proyecto.BuscarTareaPorId(1);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(tarea1, resultado);
    }

    [TestMethod]
    public void BuscarTareaPorId_TareaNoExiste_ReturnsNull()
    {
        Proyecto proyecto = new Proyecto("Proyecto de prueba", "Descripción del proyecto", DateTime.Now);
        Tarea tarea1 = new Tarea("Tarea 1", "descripcion", DateTime.Now,VALID_TIMESPAN, false);
        Tarea tarea2 = new Tarea("Tarea 2", "descripcion", DateTime.Now,VALID_TIMESPAN, false);

        proyecto.agregarTarea(tarea1);
        proyecto.agregarTarea(tarea2);

        proyecto.agregarRecurso(recurso);

        Tarea resultado = proyecto.BuscarTareaPorId(8);

        Assert.IsNull(resultado);
    }
    
    [TestMethod]
    public void CalcularFinEstimadoDelProyecto()
    {
        DateTime inicio = DateTime.Today.AddDays(1);
        TimeSpan duracion = TimeSpan.FromHours(2);

        Tarea t1 = new Tarea("T1", "desc", inicio, duracion, false);
        Tarea t2 = new Tarea("T2", "desc", inicio, duracion, false);
        Tarea t3 = new Tarea("T3", "desc", inicio, duracion, false);
        Tarea t4 = new Tarea("T4", "desc", inicio, duracion, false);

        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t1);
        t4.AgregarDependencia(t2);
        t4.AgregarDependencia(t3);

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.agregarTarea(t1);
        proyecto.agregarTarea(t2);
        proyecto.agregarTarea(t3);
        proyecto.agregarTarea(t4);

        List<Tarea> rutaCritica = proyecto.CalcularRutaCritica();
        proyecto.CalcularFinEstimado();
        Assert.AreEqual(DateTime.Today.AddDays(1).AddHours(6), proyecto.FinEstimado);
    }
    
    [TestMethod]
    public void DevolverTareasNoCriticas()
    {
        DateTime inicio = DateTime.Today.AddDays(1);
        TimeSpan duracion = TimeSpan.FromHours(2);

        Tarea t1 = new Tarea("T1", "desc", inicio, duracion, false);
        Tarea t2 = new Tarea("T2", "desc", inicio, duracion, false);
        Tarea t3 = new Tarea("T3", "desc", inicio, duracion, false);
        Tarea t4 = new Tarea("T4", "desc", inicio, duracion, false);

        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t1);

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.agregarTarea(t1);
        proyecto.agregarTarea(t2);
        proyecto.agregarTarea(t3);
        proyecto.agregarTarea(t4);

        proyecto.CalcularRutaCritica();
        List<Tarea> noCriticas = proyecto.TareasNoCriticas();
        Assert.AreEqual(noCriticas.Count,1);
        Assert.IsTrue(noCriticas.Contains(t4));
    }
    
    [TestMethod]
    public void CalcularElInicioVerdaderoDeLproyecto()
    {
        DateTime inicio = DateTime.Today.AddDays(1);
        TimeSpan duracion = TimeSpan.FromHours(2);

        Tarea t1 = new Tarea("T1", "desc", inicio, duracion, false);
        Tarea t2 = new Tarea("T2", "desc", inicio, duracion, false);
        Tarea t3 = new Tarea("T3", "desc", inicio, duracion, false);
        Tarea t4 = new Tarea("T4", "desc", inicio, duracion, false);

        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t1);

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.agregarTarea(t1);
        proyecto.agregarTarea(t2);
        proyecto.agregarTarea(t3);
        proyecto.agregarTarea(t4);

        proyecto.CalcularRutaCritica();
        proyecto.CalcularFinEstimado();
        DateTime inicioVerdadero = proyecto.InicioVerdadero();
        
        Assert.AreEqual(inicioVerdadero, inicio);
        
    }
    
    [TestMethod]
    public void CalcularLosDiasTotalesDelProyecto()
    {
        DateTime inicio = DateTime.Today.AddDays(1);
        TimeSpan duracion = TimeSpan.FromHours(2);

        Tarea t1 = new Tarea("T1", "desc", inicio, duracion, false);
        Tarea t2 = new Tarea("T2", "desc", inicio, duracion, false);
        Tarea t3 = new Tarea("T3", "desc", inicio, duracion, false);
        Tarea t4 = new Tarea("T4", "desc", inicio, duracion, false);

        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t1);

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.agregarTarea(t1);
        proyecto.agregarTarea(t2);
        proyecto.agregarTarea(t3);
        proyecto.agregarTarea(t4);

        proyecto.CalcularRutaCritica();
        proyecto.CalcularFinEstimado();
        int diasTotales = proyecto.CalcularDiasTotales();
        
        Assert.AreEqual(diasTotales, 1);
        
    }
    
    [TestMethod]
    public void DevolverTareasOrdenadasPorEarlyStart()
    {
        DateTime inicio = DateTime.Today;
        TimeSpan duracion = TimeSpan.FromDays(2);

        Tarea t1 = new Tarea("T1", "desc", inicio, duracion, false);
        Tarea t2 = new Tarea("T2", "desc", inicio, duracion, false);
        Tarea t3 = new Tarea("T3", "desc", inicio.AddHours(1), duracion, false);
        Tarea t4 = new Tarea("T4", "desc", inicio, duracion, false);

        t2.AgregarDependencia(t1);
        t4.AgregarDependencia(t3);

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.agregarTarea(t1);
        proyecto.agregarTarea(t2);
        proyecto.agregarTarea(t3);
        proyecto.agregarTarea(t4);

        proyecto.CalcularRutaCritica();
        List<Tarea> tareasOrdenadas = proyecto.TareasAsociadasPorInicio();
        
        Assert.AreEqual(t1, tareasOrdenadas[0]);
        Assert.AreEqual(t3, tareasOrdenadas[1]);
        Assert.AreEqual(t2, tareasOrdenadas[2]);
        Assert.AreEqual(t4, tareasOrdenadas[3]);
        
    }
    
}


