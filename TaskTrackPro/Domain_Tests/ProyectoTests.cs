﻿using Domain;
using System.Reflection;
using Domain.Enums;


namespace Domain_Tests;

[TestClass]
public class ProyectoTests
{
    private static Recurso recurso = new Recurso("Recurso", "Tipo", "Descripcion", true, 5);
    private static readonly TimeSpan VALID_TIMESPAN = new TimeSpan(6, 5, 0, 0);
    private static  IEnumerable<AsignacionRecursoTarea> _asignacionRecursoTareas;
       
    [TestInitialize]
    public void Setup()
    {
        typeof(Proyecto)
            .GetField("_contadorId", BindingFlags.Static | BindingFlags.NonPublic)
            ?.SetValue(null, 1);
        typeof(Tarea)
            .GetField("_contadorId", BindingFlags.Static | BindingFlags.NonPublic)
            ?.SetValue(null, 1);
       _asignacionRecursoTareas  = Enumerable.Empty<AsignacionRecursoTarea>();
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
    public void AgregarTareaAlProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);

        Tarea tarea = new Tarea("Tarea de prueba", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        
        proyecto.AgregarTarea(tarea);
        
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

        Tarea tarea = new Tarea("Tarea de prueba", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        
        proyecto.AgregarTarea(tarea);
        proyecto.AgregarTarea(tarea);
    }
    
    [TestMethod]
    public void AgregarTarea_TituloUnico_DeberiaAgregarTarea()
    {
        Proyecto proyecto = new Proyecto("Proyecto Test", "Descripción", DateTime.Now.AddDays(1));
        Tarea tarea1 = new Tarea("Tarea 1", "Descripcion",DateTime.Now.AddDays(1), VALID_TIMESPAN, false);
        Tarea tarea2 = new Tarea("Tarea 2", "Descripcion",DateTime.Now.AddDays(1), VALID_TIMESPAN, false);

        
        proyecto.AgregarTarea(tarea1);
        proyecto.AgregarTarea(tarea2);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarTarea_TituloRepetido_DeberiaLanzarExcepcion()
    {
        Proyecto proyecto = new Proyecto("Proyecto Test", "Descripción", DateTime.Now.AddDays(1));
        Tarea tarea1 = new Tarea("Tarea Duplicada", "descripcion 1", DateTime.Now.AddDays(1), VALID_TIMESPAN, false);
        Tarea tarea2 = new Tarea("Tarea Duplicada", "descripcion 2" ,DateTime.Now.AddDays(2), VALID_TIMESPAN, false);

        proyecto.AgregarTarea(tarea1);

        proyecto.AgregarTarea(tarea2);
    }
    
    [TestMethod]
    
    public void EliminarTareaDelProyectoConVariasTareas()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        Tarea tarea = new Tarea("Tarea a eliminar", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        Tarea tarea2 = new Tarea("Otra tarea a eliminar", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        Tarea tarea3 = new Tarea("Ootra tarea a eliminar", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        Tarea tarea4 = new Tarea("Oootra tarea a eliminar", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        
        proyecto.AgregarTarea(tarea);
        proyecto.AgregarTarea(tarea2);
        proyecto.AgregarTarea(tarea3);
        proyecto.AgregarTarea(tarea4);
        tarea.AgregarDependencia(tarea2);
        Assert.AreEqual(4, proyecto.TareasAsociadas.Count);

        proyecto.EliminarTarea(tarea);

        Assert.AreEqual(3, proyecto.TareasAsociadas.Count);
        Assert.IsFalse(proyecto.TareasAsociadas.Contains(tarea));
        Assert.IsFalse(tarea2.TareasSucesoras.Contains(tarea));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarTareaQueNoExisteEnElProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        Tarea tarea = new Tarea("Tarea a eliminar", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        
        proyecto.EliminarTarea(tarea);
    }
    
    [TestMethod]
    public void AgregarMiembroAProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        Usuario user = new Usuario("gandalf@gmail.com", "Gandalf", "El Gris", "ganadlfSape123!", new DateTime(2000, 01, 01));

        AsignacionProyecto asignacionProyecto = new AsignacionProyecto(proyecto, user, Rol.Miembro);

        proyecto.agregarMiembro(asignacionProyecto);
        
        Assert.AreEqual(1, proyecto.AsignacionesDelProyecto.Count);
        Assert.AreSame(user, proyecto.AsignacionesDelProyecto[0].Usuario);
        
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
        AsignacionProyecto asignacionProyecto = new AsignacionProyecto(proyecto, user, Rol.Miembro);

        proyecto.agregarMiembro(asignacionProyecto);
        proyecto.agregarMiembro(asignacionProyecto);
    }

    [TestMethod]
    public void EliminarMiembroDeProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        Usuario user = new Usuario("gandalf@gmail.com", "Gandalf", "El Gris", "ganadlfSape123!", new DateTime(2000, 01, 01));
        AsignacionProyecto asignacionProyecto = new AsignacionProyecto(proyecto, user, Rol.Miembro);

        proyecto.agregarMiembro(asignacionProyecto);

        Assert.AreEqual(1, proyecto.AsignacionesDelProyecto.Count);
        Assert.AreSame(user, proyecto.AsignacionesDelProyecto[0].Usuario);
        
        proyecto.EliminarMiembro(asignacionProyecto);
        Assert.AreEqual(0, proyecto.AsignacionesDelProyecto.Count);
        Assert.IsFalse(proyecto.AsignacionesDelProyecto.Contains(asignacionProyecto));
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
        AsignacionProyecto asignacionProyecto = new AsignacionProyecto(proyecto, user, Rol.Miembro);
        
        proyecto.EliminarMiembro(asignacionProyecto);
    }

    [TestMethod]
    public void ListarTareasSinDependencias()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jeje";
        DateTime fechaInicio = DateTime.Today;

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        Tarea tarea = new Tarea("Tarea ", "Descripción", DateTime.Today, VALID_TIMESPAN, false);
        Tarea tareaDependencia = new Tarea("Dependencia", "Desc", DateTime.Today, VALID_TIMESPAN, false);
        
        proyecto.AgregarTarea(tarea);
        proyecto.AgregarTarea(tareaDependencia);
        
        tarea.AgregarDependencia(tareaDependencia);

        List<Tarea> tareasSinDependencia = proyecto.TareasSinDependencia();
        Assert.AreEqual(1, tareasSinDependencia.Count);
        Assert.AreSame(tareaDependencia, tareasSinDependencia[0]);
    }
    
    [TestMethod]
    public void CalcularEarlyTimes_TareaSinPredecesores()
    {
        DateTime hoy = DateTime.Today;
        Proyecto proyecto = new Proyecto("Test", "Descripción", hoy);

        Tarea tarea = new Tarea("T1", "Desc", hoy, TimeSpan.FromDays(3), false);

        proyecto.AgregarTarea(tarea);

        proyecto.CalcularTiemposTempranos(_asignacionRecursoTareas);

        Assert.AreEqual(hoy, tarea.EarlyStart);
        Assert.AreEqual(hoy.AddDays(3), tarea.EarlyFinish);
    }
    
    [TestMethod]
    public void CalcularEarlyTimes_TareaConPredecesor()
    {
        DateTime hoy = DateTime.Today;
        Proyecto proyecto = new Proyecto("Test", "Desc", hoy);

        Tarea t1 = new Tarea("T1", "Desc", hoy, TimeSpan.FromDays(3), false);
        Tarea t2 = new Tarea("T2", "Desc", hoy, TimeSpan.FromDays(2), false);

        proyecto.AgregarTarea(t1);
        proyecto.AgregarTarea(t2);

        t2.AgregarDependencia(t1);

        proyecto.CalcularTiemposTempranos(_asignacionRecursoTareas);

        Assert.AreEqual(hoy, t1.EarlyStart);
        Assert.AreEqual(hoy.AddDays(3), t1.EarlyFinish);
    
        Assert.AreEqual(hoy.AddDays(3), t2.EarlyStart);
        Assert.AreEqual(hoy.AddDays(5), t2.EarlyFinish);
    }
    
    [TestMethod]
    public void CalcularTiemposTardios_ConTareasLineales_CalculaCorrectamente()
    {
        DateTime inicio = DateTime.Today.AddDays(1);
        TimeSpan duracion = TimeSpan.FromHours(2);

        Tarea t1 = new Tarea("T1", "desc", inicio, duracion, false);
        Tarea t2 = new Tarea("T2", "desc", inicio.AddHours(3), duracion, false);
        Tarea t3 = new Tarea("T3", "desc", inicio.AddHours(6), duracion, false);

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.AgregarTarea(t1);
        proyecto.AgregarTarea(t2);
        proyecto.AgregarTarea(t3);
        
        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t2);

        proyecto.CalcularTiemposTempranos(_asignacionRecursoTareas);
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
        
        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.AgregarTarea(t1);
        proyecto.AgregarTarea(t2);
        proyecto.AgregarTarea(t3);
        proyecto.AgregarTarea(t4);

        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t1);
        t4.AgregarDependencia(t2);
        t4.AgregarDependencia(t3);

        List<Tarea> rutaCritica = proyecto.CalcularRutaCritica(_asignacionRecursoTareas);

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

        Proyecto proyecto = new Proyecto("Proyecto Test", "Descripción", DateTime.Now);
        Usuario usuario = new Usuario("test@test.com", "Test", "Usuario", "Contra*seña123", DateTime.Now);
        Tarea tarea = new Tarea("Tarea Test", "Descripción", DateTime.Now, TimeSpan.FromHours(5), false);
        AsignacionProyecto asignacionProyecto = new AsignacionProyecto(proyecto, usuario, Rol.Miembro);

        proyecto.agregarMiembro(asignacionProyecto);
        proyecto.AgregarTarea(tarea);
        proyecto.AsignarUsuarioATarea(usuario, tarea);
        CollectionAssert.Contains(tarea.UsuariosAsignados.ToList(), usuario);
        
    }
    
    [TestMethod]
    public void AsignarUsuarioATarea_UsuarioNoMiembro_LanzaExcepcion()
    {
        Proyecto proyecto = new Proyecto("Proyecto Test", "Descripción", DateTime.Now);
        Usuario usuario = new Usuario("test@test.com", "Test", "Usuario", "Contr*aseña123", DateTime.Now);
        Tarea tarea = new Tarea("Tarea Test", "Descripción", DateTime.Now, TimeSpan.FromHours(5), false);
    
        proyecto.AgregarTarea(tarea);

        Assert.ThrowsException<ArgumentException>(() => proyecto.AsignarUsuarioATarea(usuario, tarea));
    }

    [TestMethod]
    public void AsignarUsuarioATarea_UsuarioYaAsignado_LanzaExcepcion()
    {
        Proyecto proyecto = new Proyecto("Proyecto Test", "Descripción", DateTime.Now);
        Usuario usuario = new Usuario("test@test.com", "Test", "Usuario", "Contr*aseña123", DateTime.Now);
        Tarea tarea = new Tarea("Tarea Test", "Descripción", DateTime.Now, TimeSpan.FromHours(5), false);
        AsignacionProyecto asignacionProyecto = new AsignacionProyecto(proyecto, usuario, Rol.Miembro);

        proyecto.agregarMiembro(asignacionProyecto);
        proyecto.AgregarTarea(tarea);
        proyecto.AsignarUsuarioATarea(usuario, tarea);

        Assert.ThrowsException<ArgumentException>(() => proyecto.AsignarUsuarioATarea(usuario, tarea));
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
    public void BuscarTareaPorId_TareaNoExiste_ReturnsNull()
    {
        Proyecto proyecto = new Proyecto("Proyecto de prueba", "Descripción del proyecto", DateTime.Now);
        Tarea tarea1 = new Tarea("Tarea 1", "descripcion", DateTime.Now,VALID_TIMESPAN, false);
        Tarea tarea2 = new Tarea("Tarea 2", "descripcion", DateTime.Now,VALID_TIMESPAN, false);

        proyecto.AgregarTarea(tarea1);
        proyecto.AgregarTarea(tarea2);
        
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

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.AgregarTarea(t1);
        proyecto.AgregarTarea(t2);
        proyecto.AgregarTarea(t3);
        proyecto.AgregarTarea(t4);

        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t1);
        t4.AgregarDependencia(t2);
        t4.AgregarDependencia(t3);

        List<Tarea> rutaCritica = proyecto.CalcularRutaCritica(_asignacionRecursoTareas);
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
        
        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.AgregarTarea(t1);
        proyecto.AgregarTarea(t2);
        proyecto.AgregarTarea(t3);
        proyecto.AgregarTarea(t4);

        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t1);

        proyecto.CalcularRutaCritica(_asignacionRecursoTareas);
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

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.AgregarTarea(t1);
        proyecto.AgregarTarea(t2);
        proyecto.AgregarTarea(t3);
        proyecto.AgregarTarea(t4);

        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t1);

        proyecto.CalcularRutaCritica(_asignacionRecursoTareas);
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

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.AgregarTarea(t1);
        proyecto.AgregarTarea(t2);
        proyecto.AgregarTarea(t3);
        proyecto.AgregarTarea(t4);

        t2.AgregarDependencia(t1);
        t3.AgregarDependencia(t1);

        proyecto.CalcularRutaCritica(_asignacionRecursoTareas);
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
        Tarea t4 = new Tarea("T4", "desc", inicio.AddHours(2), duracion, false);

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.AgregarTarea(t1);
        proyecto.AgregarTarea(t2);
        proyecto.AgregarTarea(t3);
        proyecto.AgregarTarea(t4);

        t2.AgregarDependencia(t1);
        t4.AgregarDependencia(t3);

        proyecto.CalcularRutaCritica(_asignacionRecursoTareas);
        List<Tarea> tareasOrdenadas = proyecto.TareasAsociadasPorInicio();

        Assert.AreEqual(t1, tareasOrdenadas[0]);
        Assert.AreEqual(t3, tareasOrdenadas[1]);
        Assert.AreEqual(t2, tareasOrdenadas[2]);
        Assert.AreEqual(t4, tareasOrdenadas[3]);
    }
    
    [TestMethod]
    public void DevolverTareasDelUsuario()
    {
        DateTime inicio = DateTime.Today;
        TimeSpan duracion = TimeSpan.FromDays(2);

        Tarea t1 = new Tarea("T1", "desc", inicio, duracion, false);
        Tarea t2 = new Tarea("T2", "desc", inicio, duracion, false);
        Tarea t3 = new Tarea("T3", "desc", inicio.AddHours(1), duracion, false);
        Tarea t4 = new Tarea("T4", "desc", inicio, duracion, false);

        Proyecto proyecto = new Proyecto("Proyecto", "desc", inicio);
        proyecto.AgregarTarea(t1);
        proyecto.AgregarTarea(t2);
        proyecto.AgregarTarea(t3);
        proyecto.AgregarTarea(t4);

        Usuario usuario = new Usuario("test@test.com", "Test", "Usuario", "Contra*seña123", DateTime.Now);
        t1.AgregarUsuario(usuario);
        t2.AgregarUsuario(usuario);

        List<Tarea> tareasDelUsuario = proyecto.TareasDelUsuario(usuario);
        
        Assert.AreEqual(2, tareasDelUsuario.Count);
        Assert.AreEqual(t1, tareasDelUsuario[0]);
        Assert.AreEqual(t2, tareasDelUsuario[1]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void EliminarMiembro_DeberiaLanzarExcepcion_SiUsuarioNoEsMiembro()
    {
        Proyecto proyecto = new Proyecto("Proyecto", "descripcion", DateTime.Now);
        Usuario usuario = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        AsignacionProyecto asignacionProyecto = new AsignacionProyecto(proyecto, usuario, Rol.Miembro);

        proyecto.EliminarMiembro(asignacionProyecto);
    }

    [TestMethod]
    public void EliminarMiembroDeUnaTareaCorrectamente()
    {
        Proyecto proyecto = new Proyecto("Proyecto", "descripcion", DateTime.Now);
        Usuario usuario = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        Tarea tarea = new Tarea("Tarea", "Desc", DateTime.Now, TimeSpan.FromDays(1), false);
        AsignacionProyecto asignacionProyecto = new AsignacionProyecto(proyecto, usuario, Rol.Miembro);
        
        proyecto.AgregarTarea(tarea);
        tarea.UsuariosAsignados.Add(usuario);

        proyecto.EliminarMiembroTarea(usuario,tarea);
        
        Assert.IsTrue(tarea.UsuariosAsignados.Count == 0);
        Assert.IsFalse(tarea.UsuariosAsignados.Contains(usuario));
    }
    
    [TestMethod]
    public void CalcularRutaCritica_ConsideraDisponibilidadEntreProyectos()
    {
        var hoy = DateTime.Today;
        var auto = new Recurso("Auto", "Vehiculo", "Coche", sePuedeCompartir: false, cantidadDelRecurso: 1);

        var t1 = new Tarea("T1", "Desc1", hoy, TimeSpan.FromDays(2), esCritica: false);
        var t2 = new Tarea("T2", "Desc2", hoy, TimeSpan.FromDays(2), esCritica: false);

        t1.Id = 1;
        t2.Id = 2;
        var proyecto1 = new Proyecto("P1", "Desc1", hoy);
        var proyecto2 = new Proyecto("P2", "Desc2", hoy);

        proyecto1.AgregarTarea(t1);
        proyecto2.AgregarTarea(t2);

        var a1 = new AsignacionRecursoTarea(auto, t1, 1);
        var a2 = new AsignacionRecursoTarea(auto, t2, 1);
        var allAsign = new[] { a1, a2 };

        proyecto1.CalcularTiemposTempranos(allAsign);
        proyecto1.CalcularTiemposTardios();

        t1.EstadoActual.Valor = TipoEstadoTarea.Ejecutandose;
        auto.CantidadEnUso = 1;

        t2.RecursosForzados = true;
        t2.EstadoActual.Valor = TipoEstadoTarea.Bloqueada;

        proyecto2.CalcularTiemposTempranos(allAsign);

        Assert.AreEqual(hoy.AddDays(2), t2.EarlyStart);
        Assert.AreEqual(hoy.AddDays(4), t2.EarlyFinish);
    }
    
    [TestMethod]
    public void CalcularRutaCritica_ConDependenciasEfectuadasYRecursoCompartido()
    {
        var hoy = DateTime.Today;
        var auto = new Recurso("Auto", "Vehiculo", "Coche", sePuedeCompartir: false, cantidadDelRecurso: 1);

        var pre1 = new Tarea("Pre1", "Pre1Desc", hoy, TimeSpan.FromDays(1), esCritica: false);
        var pre2 = new Tarea("Pre2", "Pre2Desc", hoy, TimeSpan.FromDays(1), esCritica: false);
        pre1.EstadoActual.Valor = TipoEstadoTarea.Efectuada;
        pre2.EstadoActual.Valor = TipoEstadoTarea.Efectuada;
        pre1.Id = 1;
        pre2.Id = 2;

        var t1 = new Tarea("T1", "Desc1", hoy.AddDays(1), TimeSpan.FromDays(2), esCritica: false);
        var t2 = new Tarea("T2", "Desc2", hoy.AddDays(1), TimeSpan.FromDays(2), esCritica: false);
        t1.Id = 3;
        t2.Id = 4;
        
        pre1.EarlyStart = hoy;
        pre1.EarlyFinish = hoy.AddDays(1);

        pre2.EarlyStart = hoy;
        pre2.EarlyFinish = hoy.AddDays(1);
        
        t1.AgregarDependencia(pre1);
        t2.AgregarDependencia(pre2);

        var proyecto1 = new Proyecto("P1", "Desc1", hoy);
        var proyecto2 = new Proyecto("P2", "Desc2", hoy);

        proyecto1.AgregarTarea(pre1);
        proyecto1.AgregarTarea(t1);

        proyecto2.AgregarTarea(pre2);
        proyecto2.AgregarTarea(t2);

        var a1 = new AsignacionRecursoTarea(auto, t1, 1);
        var a2 = new AsignacionRecursoTarea(auto, t2, 1);
        var allAsign = new[] { a1, a2 };

        proyecto1.CalcularTiemposTempranos(allAsign);
        proyecto1.CalcularTiemposTardios();
        
        t1.EstadoActual.Valor = TipoEstadoTarea.Ejecutandose;
        auto.CantidadEnUso = 1;

        t2.RecursosForzados = true;
        t2.EstadoActual.Valor = TipoEstadoTarea.Bloqueada;

        proyecto2.CalcularTiemposTempranos(allAsign);

        Assert.AreEqual(hoy.AddDays(3), t2.EarlyStart);
        Assert.AreEqual(hoy.AddDays(5), t2.EarlyFinish);
    }
}