using DTOs;
using Domain;
using Domain.Enums;
using DataAccess;
using IDataAcces;
using Services;

namespace Services_Tests;

[TestClass]
public class TareaServiceTests
{
    private IDataAccessTarea _repoTareas;
    private IDataAccessProyecto _repoProyectos;
    private IDataAccessUsuario _repoUsuarios;
    private TareaService _service;

    private Proyecto _proyectoEjemplo;
    private Tarea _tareaEjemplo;
    private Usuario _usuarioEjemplo;

    [TestInitialize]
    public void SetUp()
    {
        _repoTareas = new TareaDataAccess();
        _repoProyectos = new ProyectoDataAccess();
        _repoUsuarios = new UsuarioDataAccess();

        _service = new TareaService(_repoTareas, _repoProyectos, _repoUsuarios);

        _proyectoEjemplo = new Proyecto(
            "Proyecto Test",
            "Descripción Test",
            DateTime.Today.AddDays(1));
        _repoProyectos.Add(_proyectoEjemplo);

        _usuarioEjemplo = new Usuario(
            "user@test.com",
            "User",
            "Test",
            "Password1!",
            DateTime.Today.AddYears(-20));
        _repoUsuarios.Add(_usuarioEjemplo);

        _tareaEjemplo = new Tarea(
            "Tarea Test",
            "Descripción Tarea",
            DateTime.Today.AddHours(9),
            TimeSpan.FromHours(4),
            false);
        _proyectoEjemplo.TareasAsociadas.Add(_tareaEjemplo);
        _repoTareas.Add(_tareaEjemplo);
    }

    [TestMethod]
    public void BuscarTareaPorIdDevuelveDTO()
    {
        var resultado = _service.BuscarTareaPorId(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(_tareaEjemplo.Id, resultado.Id);
        Assert.AreEqual(_tareaEjemplo.Titulo, resultado.Titulo);
        Assert.AreEqual(_tareaEjemplo.Descripcion, resultado.Descripcion);
    }

    [TestMethod]
    public void ListarTareasPorProyectoDevuelveTodas()
    {
        var tarea2 = new Tarea(
            "Tarea 2",
            "Desc 2",
            DateTime.Today.AddDays(2),
            TimeSpan.FromHours(3),
            true);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        var lista = _service.ListarTareasPorProyecto(_proyectoEjemplo.Id);

        Assert.IsNotNull(lista);
        Assert.AreEqual(2, lista.Count);

        Assert.IsTrue(lista.Exists(t => t.Id == _tareaEjemplo.Id));
        Assert.IsTrue(lista.Exists(t => t.Id == tarea2.Id));
    }


    [TestMethod]
    public void CrearTareaAgregaCorrectamente()
    {
        var dtoNueva = new TareaDTO
        {
            Titulo = "Nueva Tarea",
            Descripcion = "Descripción nueva",
            FechaInicio = DateTime.Today.AddDays(3),
            Duracion = TimeSpan.FromHours(4)
        };

        var creada = _service.CrearTarea(_proyectoEjemplo.Id, dtoNueva);

        Assert.IsNotNull(creada);
        Assert.AreEqual(dtoNueva.Titulo, creada.Titulo);
        Assert.AreEqual(dtoNueva.Descripcion, creada.Descripcion);

        var tareasDelProyecto = _service.ListarTareasPorProyecto(_proyectoEjemplo.Id);
        var contiene = tareasDelProyecto.Any(t => t.Id == creada.Id);
        Assert.IsTrue(contiene);
    }

    [TestMethod]
    public void ModificarTareaCambiaDatosCorrectamente()
    {
        var dtoModificada = new TareaDTO
        {
            Titulo = "Modificado",
            Descripcion = "Nueva descripción",
            FechaInicio = DateTime.Today.AddDays(5),
            Duracion = TimeSpan.FromHours(8)
        };

        _service.ModificarTarea(_tareaEjemplo.Id, dtoModificada);

        var tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(dtoModificada.Titulo, tareaGuardada.Titulo);
        Assert.AreEqual(dtoModificada.Descripcion, tareaGuardada.Descripcion);
    }

    [TestMethod]
    public void MarcarComoEjecutandoseCambiaEstado()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);

        var tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.IsNotNull(tareaGuardada.EstadoActual);
        Assert.AreEqual(TipoEstadoTarea.Ejecutandose, tareaGuardada.EstadoActual.Valor);
    }

    [TestMethod]
    public void MarcarComoCompletadaCambiaBien()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);
        _service.MarcarComoCompletada(_tareaEjemplo.Id);

        var tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.IsNotNull(tareaGuardada.EstadoActual);
        Assert.AreEqual(TipoEstadoTarea.Efectuada, tareaGuardada.EstadoActual.Valor);
    }

    [TestMethod]
    public void AgregarDependenciaAgregaCorrectamente()
    {
        var tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddHours(1), TimeSpan.FromHours(3), true);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _service.AgregarDependencia(_tareaEjemplo.Id, tarea2.Id);

        var tareaOriginal = _repoTareas.GetById(_tareaEjemplo.Id);
        var tareaDependencia = _repoTareas.GetById(tarea2.Id);

        Assert.IsTrue(tareaOriginal.TareasDependencia.Contains(tareaDependencia));
        Assert.IsTrue(tareaDependencia.TareasSucesoras.Contains(tareaOriginal));
    }

    [TestMethod]
    public void AgregarUsuarioAgregaUsuarioCorrectamente()
    {
        _service.AgregarUsuario(_tareaEjemplo.Id, _usuarioEjemplo.Id);

        var tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.IsNotNull(tareaGuardada);
        Assert.IsTrue(tareaGuardada.UsuariosAsignados.Contains(_usuarioEjemplo));
    }

    [TestMethod]
    public void TieneSucesorasDevuelveFalseCuandoNoHaySucesoras()
    {
        var dtoSinSucesoras = Convertidor.ATareaDTO(_tareaEjemplo);
        var resultado = _service.TieneSucesoras(dtoSinSucesoras);
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void TieneSucesorasDevuelveTrueCuandoHaySucesoras()
    {
        var tarea2 = new Tarea(
            "Tarea 2",
            "Desc 2",
            DateTime.Today.AddHours(1),
            TimeSpan.FromHours(3),
            true);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _service.AgregarDependencia(_tareaEjemplo.Id, tarea2.Id);

        var dtoConSucesoras = Convertidor.ATareaDTO(tarea2);

        var resultado = _service.TieneSucesoras(dtoConSucesoras);

        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void UsuarioPerteneceALaTareaDevuelveTrueCuandoUsuarioAsignado()
    {
        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        var resultado = _service.UsuarioPerteneceALaTarea(_usuarioEjemplo.Id, _tareaEjemplo.Id);
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void EliminarTareaSinSucesorasNiDependenciasEliminaCorrectamente()
    {
        // Act
        _service.EliminarTarea(_proyectoEjemplo.Id, _tareaEjemplo.Id);

        // Assert: la tarea ya no está en el proyecto
        Assert.IsFalse(_proyectoEjemplo.TareasAsociadas.Contains(_tareaEjemplo));

        // Y al intentar buscarla, lanzará ArgumentException
        Assert.ThrowsException<ArgumentException>(() =>
            _service.BuscarTareaPorId(_tareaEjemplo.Id)
        );
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EliminarTarea_LanzaExcepcionSiTieneSucesoras()
    {
        // Preparamos una segunda tarea que dependa de la primera
        var tarea2 = new Tarea(
            "Tarea 2",
            "Desc",
            DateTime.Today.AddHours(9),
            TimeSpan.FromHours(4),
            false
        );
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        // tarea2 depende de tareaEjemplo → tareaEjemplo tiene sucesoras
        tarea2.AgregarDependencia(_tareaEjemplo);

        // Act: debe lanzar InvalidOperationException
        _service.EliminarTarea(_proyectoEjemplo.Id, _tareaEjemplo.Id);

        // (No llega aquí)
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EliminarTarea_LanzaExcepcionSiTieneDependencias()
    {
        // Preparamos una segunda tarea de la que dependa la original
        var tarea2 = new Tarea(
            "Tarea 2",
            "Desc",
            DateTime.Today.AddHours(9),
            TimeSpan.FromHours(4),
            false
        );
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        // tareaEjemplo depende de tarea2 → tareaEjemplo tiene dependencias
        _tareaEjemplo.AgregarDependencia(tarea2);

        // Act: debe lanzar InvalidOperationException
        _service.EliminarTarea(_proyectoEjemplo.Id, _tareaEjemplo.Id);
    }

    [TestMethod]
    public void TieneDependenciasReturnTrueCuandoTiene()
    {
        var _tarea2 = new Tarea("Tarea Test",
            "Descripción Tarea",
            DateTime.Today.AddHours(9),
            TimeSpan.FromHours(4),
            false);
        _proyectoEjemplo.TareasAsociadas.Add(_tarea2);
        _repoTareas.Add(_tarea2);
        _tareaEjemplo.AgregarDependencia(_tarea2);
        var resultado = _service.TieneDependencias(Convertidor.ATareaDTO(_tareaEjemplo));
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void GetEstadoTarea_DevuelveEstadoInicial()
    {
        var estado = _service.GetEstadoTarea(_tareaEjemplo.Id);

        Assert.AreEqual(TipoEstadoTarea.Pendiente, estado);
    }

    [TestMethod]
    public void GetEstadoTarea_DevuelveRealizandose_CuandoTareaEstaEjecutandose()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);

        var estado = _service.GetEstadoTarea(_tareaEjemplo.Id);

        Assert.AreEqual(TipoEstadoTarea.Ejecutandose, estado);
    }

    [TestMethod]
    public void GetEstadoTarea_DevuelveEfectuada_CuandoTareaEstaCompletada()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);
        _service.MarcarComoCompletada(_tareaEjemplo.Id);

        var estado = _service.GetEstadoTarea(_tareaEjemplo.Id);

        Assert.AreEqual(TipoEstadoTarea.Efectuada, estado);
    }
    
    [TestMethod]
    public void GetEstadoTarea_DevuelveRealizada_SiLaTareaFueCompletada()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);
        _service.MarcarComoCompletada(_tareaEjemplo.Id);

        var estado = _service.GetEstadoTarea(_tareaEjemplo.Id);
        Assert.AreEqual(TipoEstadoTarea.Efectuada, estado);
    }


    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetEstadoTarea_LanzaExcepcion_CuandoTareaNoExiste()
    {
        _service.GetEstadoTarea(-1);
    }

    [TestMethod]
    public void ListarUsuariosDeTarea_ConTareaExistenteYUsuarios_DevuelveListaUsuarios()
    {
        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);

        var resultado = _service.ListarUsuariosDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(_usuarioEjemplo.Email, resultado[0].Email);
    }

    [TestMethod]
    public void ListarUsuariosDeTarea_ConTareaExistenteSinUsuarios_DevuelveListaVacia()
    {
        var resultado = _service.ListarUsuariosDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ListarUsuariosDeTarea_ConTareaInexistente_DevuelveNull()
    {
        var idInexistente = 9999;
        var resultado = _service.ListarUsuariosDeTarea(idInexistente);
    }

    [TestMethod]
    public void ListarUsuariosDeTarea_ConMultiplesUsuarios_DevuelveTodosCorrectamente()
    {
        var usuario2 = new Usuario(
            "user2@test.com",
            "User2",
            "Test2",
            "Password2!",
            DateTime.Today.AddYears(-25));
        _repoUsuarios.Add(usuario2);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        _tareaEjemplo.AgregarUsuario(usuario2);

        var resultado = _service.ListarUsuariosDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(2, resultado.Count);
        Assert.IsTrue(resultado.Any(u => u.Email == _usuarioEjemplo.Email));
        Assert.IsTrue(resultado.Any(u => u.Email == usuario2.Email));
    }

    [TestMethod]
    public void EliminarUsuarioDeTarea_ConUsuarioAsignado_EliminaCorrectamente()
    {
        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);

        _service.EliminarUsuarioDeTarea(_usuarioEjemplo.Id, _tareaEjemplo.Id);

        var tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.IsFalse(tareaGuardada.UsuariosAsignados.Contains(_usuarioEjemplo));
    }

    [TestMethod]
    public void EliminarUsuarioDeTarea_ConUsuarioNoAsignado_NoAfectaLaTarea()
    {
        var usuario2 = new Usuario(
            "user2@test.com",
            "User2",
            "Test2",
            "Password2!",
            DateTime.Today.AddYears(-25));
        _repoUsuarios.Add(usuario2);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        var cantidadInicial = _tareaEjemplo.UsuariosAsignados.Count;

        _service.EliminarUsuarioDeTarea(usuario2.Id, _tareaEjemplo.Id);

        var tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(cantidadInicial, tareaGuardada.UsuariosAsignados.Count);
        Assert.IsTrue(tareaGuardada.UsuariosAsignados.Contains(_usuarioEjemplo));
    }

    [TestMethod]
    public void EliminarUsuarioDeTareasDeProyecto_ConUsuarioAsignadoAVariasTareas_EliminaDeTodasLasTareas()
    {
        var tarea2 = new Tarea(
            "Tarea 2",
            "Descripción 2",
            DateTime.Today.AddDays(2),
            TimeSpan.FromHours(3),
            false);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        tarea2.AgregarUsuario(_usuarioEjemplo);

        _service.EliminarUsuarioDeTareasDeProyecto(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        var tareaGuardada1 = _repoTareas.GetById(_tareaEjemplo.Id);
        var tareaGuardada2 = _repoTareas.GetById(tarea2.Id);

        Assert.IsFalse(tareaGuardada1.UsuariosAsignados.Contains(_usuarioEjemplo));
        Assert.IsFalse(tareaGuardada2.UsuariosAsignados.Contains(_usuarioEjemplo));
    }

    [TestMethod]
    public void EliminarUsuarioDeTareasDeProyecto_ConUsuarioNoAsignadoANingunaTarea_NoAfectaNingunaTarea()
    {
        var usuarioNoAsignado = new Usuario(
            "noasignado@test.com",
            "No",
            "Asignado",
            "Password1!",
            DateTime.Today.AddYears(-30));
        _repoUsuarios.Add(usuarioNoAsignado);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        var cantidadInicial = _tareaEjemplo.UsuariosAsignados.Count;

        _service.EliminarUsuarioDeTareasDeProyecto(usuarioNoAsignado.Id, _proyectoEjemplo.Id);

        var tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(cantidadInicial, tareaGuardada.UsuariosAsignados.Count);
        Assert.IsTrue(tareaGuardada.UsuariosAsignados.Contains(_usuarioEjemplo));
    }

    [TestMethod]
    public void EliminarUsuarioDeTareasDeProyecto_ConUsuarioAsignadoSoloAAlgunasTareas_EliminaSoloDeEsasTareas()
    {
        var usuario2 = new Usuario(
            "user2@test.com",
            "User2",
            "Test2",
            "Password2!",
            DateTime.Today.AddYears(-25));
        _repoUsuarios.Add(usuario2);

        var tarea2 = new Tarea(
            "Tarea 2",
            "Descripción 2",
            DateTime.Today.AddDays(2),
            TimeSpan.FromHours(3),
            false);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        _tareaEjemplo.AgregarUsuario(usuario2);
        tarea2.AgregarUsuario(usuario2);

        _service.EliminarUsuarioDeTareasDeProyecto(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        var tareaGuardada1 = _repoTareas.GetById(_tareaEjemplo.Id);
        var tareaGuardada2 = _repoTareas.GetById(tarea2.Id);

        Assert.IsFalse(tareaGuardada1.UsuariosAsignados.Contains(_usuarioEjemplo));
        Assert.IsTrue(tareaGuardada1.UsuariosAsignados.Contains(usuario2));
        Assert.IsTrue(tareaGuardada2.UsuariosAsignados.Contains(usuario2));
        Assert.IsFalse(tareaGuardada2.UsuariosAsignados.Contains(_usuarioEjemplo));
    }

    [TestMethod]
    public void EliminarUsuarioDeTareasDeProyecto_ConProyectoSinTareas_NoGeneraError()
    {
        var proyectoVacio = new Proyecto(
            "Proyecto Vacío",
            "Sin tareas",
            DateTime.Today.AddDays(10));
        _repoProyectos.Add(proyectoVacio);

        _service.EliminarUsuarioDeTareasDeProyecto(_usuarioEjemplo.Id, proyectoVacio.Id);

        Assert.IsTrue(true);
    }

    [TestMethod]
    public void EliminarUsuarioDeTareasDeProyecto_ConMultiplesUsuariosEnTarea_EliminaSoloElUsuarioEspecifico()
    {
        var usuario2 = new Usuario(
            "user2@test.com",
            "User2",
            "Test2",
            "Password2!",
            DateTime.Today.AddYears(-25));
        _repoUsuarios.Add(usuario2);

        var usuario3 = new Usuario(
            "user3@test.com",
            "User3",
            "Test3",
            "Password3!",
            DateTime.Today.AddYears(-28));
        _repoUsuarios.Add(usuario3);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        _tareaEjemplo.AgregarUsuario(usuario2);
        _tareaEjemplo.AgregarUsuario(usuario3);

        var cantidadInicial = _tareaEjemplo.UsuariosAsignados.Count;

        _service.EliminarUsuarioDeTareasDeProyecto(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        var tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);

        Assert.AreEqual(cantidadInicial - 1, tareaGuardada.UsuariosAsignados.Count);
        Assert.IsFalse(tareaGuardada.UsuariosAsignados.Contains(_usuarioEjemplo));
        Assert.IsTrue(tareaGuardada.UsuariosAsignados.Contains(usuario2));
        Assert.IsTrue(tareaGuardada.UsuariosAsignados.Contains(usuario3));
    }
    
    [TestMethod]
    public void ObtenerDependenciasDeTarea_ConTareaSinDependencias_DevuelveListaVacia()
    {
        var resultado = _service.ObtenerDependenciasDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void ObtenerDependenciasDeTarea_ConTareaConDependencias_DevuelveListaConDependencias()
    {
        var tarea2 = new Tarea(
            "Tarea Dependencia",
            "Descripción dependencia",
            DateTime.Today.AddHours(1),
            TimeSpan.FromHours(3),
            false);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _tareaEjemplo.AgregarDependencia(tarea2);

        var resultado = _service.ObtenerDependenciasDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(tarea2.Id, resultado[0].Id);
        Assert.AreEqual(tarea2.Titulo, resultado[0].Titulo);
    }

    [TestMethod]
    public void ObtenerDependenciasDeTarea_ConMultiplesDependencias_DevuelveTodasLasDependencias()
    {
        var tarea2 = new Tarea("Dependencia 1", "Desc 1", DateTime.Today.AddHours(1), TimeSpan.FromHours(2), false);
        var tarea3 = new Tarea("Dependencia 2", "Desc 2", DateTime.Today.AddHours(2), TimeSpan.FromHours(3), false);
        
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _proyectoEjemplo.TareasAsociadas.Add(tarea3);
        _repoTareas.Add(tarea2);
        _repoTareas.Add(tarea3);

        _tareaEjemplo.AgregarDependencia(tarea2);
        _tareaEjemplo.AgregarDependencia(tarea3);

        var resultado = _service.ObtenerDependenciasDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(2, resultado.Count);
        Assert.IsTrue(resultado.Any(t => t.Id == tarea2.Id));
        Assert.IsTrue(resultado.Any(t => t.Id == tarea3.Id));
    }

    [TestMethod]
    public void EliminarDependencia_ConDependenciaExistente_EliminaCorrectamente()
    {
        var tarea2 = new Tarea(
            "Tarea Dependencia",
            "Descripción dependencia",
            DateTime.Today.AddHours(1),
            TimeSpan.FromHours(3),
            false);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _tareaEjemplo.AgregarDependencia(tarea2);

        _service.EliminarDependencia(_tareaEjemplo.Id, tarea2.Id);

        var tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        var dependenciaGuardada = _repoTareas.GetById(tarea2.Id);

        Assert.IsFalse(tareaGuardada.TareasDependencia.Contains(dependenciaGuardada));
        Assert.IsFalse(dependenciaGuardada.TareasSucesoras.Contains(tareaGuardada));
    }

    [TestMethod]
    public void EliminarDependencia_ConDependenciaNoExistente_NoAfectaLaTarea()
    {
        var tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddHours(1), TimeSpan.FromHours(3), false);
        var tarea3 = new Tarea("Tarea 3", "Desc 3", DateTime.Today.AddHours(2), TimeSpan.FromHours(2), false);
        
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _proyectoEjemplo.TareasAsociadas.Add(tarea3);
        _repoTareas.Add(tarea2);
        _repoTareas.Add(tarea3);

        _tareaEjemplo.AgregarDependencia(tarea2);
        var cantidadInicial = _tareaEjemplo.TareasDependencia.Count;

        _service.EliminarDependencia(_tareaEjemplo.Id, tarea3.Id);

        var tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(cantidadInicial, tareaGuardada.TareasDependencia.Count);
        Assert.IsTrue(tareaGuardada.TareasDependencia.Contains(tarea2));
    }

    [TestMethod]
    public void ListarTareasDelUsuario_ConUsuarioSinTareasAsignadas_DevuelveListaVacia()
    {
        var resultado = _service.ListarTareasDelUsuario(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void ListarTareasDelUsuario_ConUsuarioConTareasAsignadas_DevuelveTareasDelUsuario()
    {
        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);

        var resultado = _service.ListarTareasDelUsuario(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(_tareaEjemplo.Id, resultado[0].Id);
        Assert.AreEqual(_tareaEjemplo.Titulo, resultado[0].Titulo);
    }

    [TestMethod]
    public void ListarTareasDelUsuario_ConMultiplesTareasAsignadas_DevuelveTodasLasTareasDelUsuario()
    {
        var tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddDays(1), TimeSpan.FromHours(2), false);
        var tarea3 = new Tarea("Tarea 3", "Desc 3", DateTime.Today.AddDays(2), TimeSpan.FromHours(1), false);
        
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _proyectoEjemplo.TareasAsociadas.Add(tarea3);
        _repoTareas.Add(tarea2);
        _repoTareas.Add(tarea3);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        tarea2.AgregarUsuario(_usuarioEjemplo);

        var resultado = _service.ListarTareasDelUsuario(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(2, resultado.Count);
        Assert.IsTrue(resultado.Any(t => t.Id == _tareaEjemplo.Id));
        Assert.IsTrue(resultado.Any(t => t.Id == tarea2.Id));
        Assert.IsFalse(resultado.Any(t => t.Id == tarea3.Id));
    }

    [TestMethod]
    public void ListarTareasDelUsuario_ConUsuarioAsignadoSoloAAlgunasTareas_DevuelveSoloEsasTareas()
    {
        var usuario2 = new Usuario(
            "user2@test.com",
            "User2",
            "Test2",
            "Password2!",
            DateTime.Today.AddYears(-25));
        _repoUsuarios.Add(usuario2);

        var tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddDays(1), TimeSpan.FromHours(2), false);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        _tareaEjemplo.AgregarUsuario(usuario2);
        tarea2.AgregarUsuario(usuario2);

        var resultado = _service.ListarTareasDelUsuario(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(_tareaEjemplo.Id, resultado[0].Id);
    }

    [TestMethod]
    public void PuedeCambiarDeEstado_ConTareaEnEstadoPendiente_DevuelveTrue()
    {
        var resultado = _service.PuedeCambiarDeEstado(_tareaEjemplo.Id);

        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void PuedeCambiarDeEstado_ConTareaEjecutandose_DevuelveTrue()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);

        var resultado = _service.PuedeCambiarDeEstado(_tareaEjemplo.Id);

        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void PuedeCambiarDeEstado_ConTareaCompletada_DevuelveFalse()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);
        _service.MarcarComoCompletada(_tareaEjemplo.Id);

        var resultado = _service.PuedeCambiarDeEstado(_tareaEjemplo.Id);

        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void ObtenerTareasParaAgregarDependencia_ConProyectoSinOtrasTareas_DevuelveListaVacia()
    {
        var resultado = _service.ObtenerTareasParaAgregarDependencia(_tareaEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void ObtenerTareasParaAgregarDependencia_ConOtrasTareasDisponibles_DevuelveTareasDisponibles()
    {
        var tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddDays(1), TimeSpan.FromHours(2), false);
        var tarea3 = new Tarea("Tarea 3", "Desc 3", DateTime.Today.AddDays(2), TimeSpan.FromHours(1), false);
        
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _proyectoEjemplo.TareasAsociadas.Add(tarea3);
        _repoTareas.Add(tarea2);
        _repoTareas.Add(tarea3);

        var resultado = _service.ObtenerTareasParaAgregarDependencia(_tareaEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(2, resultado.Count);
        Assert.IsTrue(resultado.Any(t => t.Id == tarea2.Id));
        Assert.IsTrue(resultado.Any(t => t.Id == tarea3.Id));
        Assert.IsFalse(resultado.Any(t => t.Id == _tareaEjemplo.Id));
    }

    [TestMethod]
    public void ObtenerTareasParaAgregarDependencia_ExcluyeSucesoras_DevuelveSoloTareasDisponibles()
    {
        var tarea2 = new Tarea("Sucesora", "Desc 2", DateTime.Today.AddDays(1), TimeSpan.FromHours(2), false);
        var tarea3 = new Tarea("Disponible", "Desc 3", DateTime.Today.AddDays(2), TimeSpan.FromHours(1), false);
        
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _proyectoEjemplo.TareasAsociadas.Add(tarea3);
        _repoTareas.Add(tarea2);
        _repoTareas.Add(tarea3);

        tarea2.AgregarDependencia(_tareaEjemplo);

        var resultado = _service.ObtenerTareasParaAgregarDependencia(_tareaEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(tarea3.Id, resultado[0].Id);
        Assert.IsFalse(resultado.Any(t => t.Id == tarea2.Id));
    }

    [TestMethod]
    public void PuedeAgregarDependencias_ConTareaEnEstadoPendiente_DevuelveTrue()
    {
        var resultado = _service.PuedeAgregarDependencias(_tareaEjemplo.Id);

        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void PuedeAgregarDependencias_ConTareaEjecutandose_DevuelveFalse()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);

        var resultado = _service.PuedeAgregarDependencias(_tareaEjemplo.Id);

        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void PuedeAgregarDependencias_ConTareaCompletada_DevuelveFalse()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);
        _service.MarcarComoCompletada(_tareaEjemplo.Id);

        var resultado = _service.PuedeAgregarDependencias(_tareaEjemplo.Id);

        Assert.IsFalse(resultado);
    }
    
    [TestMethod]
    public void PuedeEliminarTarea_ConTareaSinDependenciasNiSucesoras_DevuelveTrue()
    {
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);
        
        bool resultado = _service.PuedeEliminarTarea(tareaDTO);
        
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void PuedeEliminarTarea_ConTareaConDependencias_DevuelveFalse()
    {
        Tarea tarea2 = new Tarea(
            "Tarea Dependencia",
            "Descripción dependencia",
            DateTime.Today.AddHours(1),
            TimeSpan.FromHours(3),
            false);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _tareaEjemplo.AgregarDependencia(tarea2);
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);

        bool resultado = _service.PuedeEliminarTarea(tareaDTO);

        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void ActualizarEstadoTarea_ConEstadoPendiente_MarcaComoEjecutandose()
    {
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);

        _service.ActualizarEstadoTarea(TipoEstadoTarea.Pendiente, tareaDTO);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.IsNotNull(tareaGuardada.EstadoActual);
        Assert.AreEqual(TipoEstadoTarea.Ejecutandose, tareaGuardada.EstadoActual.Valor);
    }

    [TestMethod]
    public void ActualizarEstadoTarea_ConEstadoEjecutandose_MarcaComoCompletada()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);

        _service.ActualizarEstadoTarea(TipoEstadoTarea.Ejecutandose, tareaDTO);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.IsNotNull(tareaGuardada.EstadoActual);
        Assert.AreEqual(TipoEstadoTarea.Efectuada, tareaGuardada.EstadoActual.Valor);
    }

    [TestMethod]
    public void ActualizarEstadoTarea_ConEstadoEfectuada_NoHaceNada()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);
        _service.MarcarComoCompletada(_tareaEjemplo.Id);
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);

        _service.ActualizarEstadoTarea(TipoEstadoTarea.Efectuada, tareaDTO);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.IsNotNull(tareaGuardada.EstadoActual);
        Assert.AreEqual(TipoEstadoTarea.Efectuada, tareaGuardada.EstadoActual.Valor);
    }
}