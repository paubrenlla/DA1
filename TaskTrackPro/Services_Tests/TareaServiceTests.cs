using DTOs;
using Domain;
using Domain.Enums;
using DataAccess;
using Domain.Observers;
using IDataAcces;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Services_Tests;

[TestClass]
public class TareaServiceTests
{
    private IDataAccessTarea _repoTareas;
    private IDataAccessProyecto _repoProyectos;
    private IDataAccessUsuario _repoUsuarios;
    private IAsignacionRecursoTareaService _asignacionRecursoTareaService;
    private IRecursoService _recursoService;
    private TareaService _service;
    private List<IRecursoObserver> _observadores;

    private Proyecto _proyectoEjemplo;
    private Tarea _tareaEjemplo;
    private Usuario _usuarioEjemplo;
    private Recurso _recursoEjemplo;

    [TestInitialize]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        SqlContext context = new SqlContext(options);
    
        _repoTareas = new TareaDataAccess(context);
        _repoProyectos = new ProyectoDataAccess(context);
        _repoUsuarios = new UsuarioDataAccess(context);
        _observadores = new List<IRecursoObserver>();
        
        var repoAsignaciones = new AsignacionRecursoTareaDataAccess(context);
        _recursoService = new RecursoService(new RecursoDataAccess(context), repoAsignaciones, _observadores);
    
        _asignacionRecursoTareaService = new AsignacionRecursoTareaService(new RecursoDataAccess(context), _repoTareas,  repoAsignaciones);
    
        _service = new TareaService(_repoTareas, _repoProyectos, _repoUsuarios, _asignacionRecursoTareaService,  _recursoService);

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

        _recursoEjemplo = new Recurso(
            "Recurso Test",
            "Descripción Recurso",
            "desc",
            false,
            100);
        _recursoService.Add(Convertidor.ARecursoDTO(_recursoEjemplo));

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
        TareaDTO resultado = _service.BuscarTareaPorId(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(_tareaEjemplo.Id, resultado.Id);
        Assert.AreEqual(_tareaEjemplo.Titulo, resultado.Titulo);
        Assert.AreEqual(_tareaEjemplo.Descripcion, resultado.Descripcion);
    }

    [TestMethod]
    public void ListarTareasPorProyectoDevuelveTodas()
    {
        Tarea tarea2 = new Tarea(
            "Tarea 2",
            "Desc 2",
            DateTime.Today.AddDays(2),
            TimeSpan.FromHours(3),
            true);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        List<TareaDTO> lista = _service.ListarTareasPorProyecto(_proyectoEjemplo.Id);

        Assert.IsNotNull(lista);
        Assert.AreEqual(2, lista.Count);

        Assert.IsTrue(lista.Exists(t => t.Id == _tareaEjemplo.Id));
        Assert.IsTrue(lista.Exists(t => t.Id == tarea2.Id));
    }


    [TestMethod]
    public void CrearTareaAgregaCorrectamente()
    {
        TareaDTO dtoNueva = new TareaDTO
        {
            Titulo = "Nueva Tarea",
            Descripcion = "Descripción nueva",
            FechaInicio = DateTime.Today.AddDays(3),
            Duracion = TimeSpan.FromHours(4)
        };

        TareaDTO creada = _service.CrearTarea(_proyectoEjemplo.Id, dtoNueva);

        Assert.IsNotNull(creada);
        Assert.AreEqual(dtoNueva.Titulo, creada.Titulo);
        Assert.AreEqual(dtoNueva.Descripcion, creada.Descripcion);

        List<TareaDTO> tareasDelProyecto = _service.ListarTareasPorProyecto(_proyectoEjemplo.Id);
        bool contiene = tareasDelProyecto.Any(t => t.Id == creada.Id);
        Assert.IsTrue(contiene);
    }

    [TestMethod]
    public void ModificarTareaCambiaDatosCorrectamente()
    {
        TareaDTO dtoModificada = new TareaDTO
        {
            Titulo = "Modificado",
            Descripcion = "Nueva descripción",
            FechaInicio = DateTime.Today.AddDays(5),
            Duracion = TimeSpan.FromHours(8)
        };

        _service.ModificarTarea(_tareaEjemplo.Id, dtoModificada);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(dtoModificada.Titulo, tareaGuardada.Titulo);
        Assert.AreEqual(dtoModificada.Descripcion, tareaGuardada.Descripcion);
    }

    [TestMethod]
    public void AgregarDependenciaAgregaCorrectamente()
    {
        Tarea tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddHours(1), TimeSpan.FromHours(3), true);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _service.AgregarDependencia(_tareaEjemplo.Id, tarea2.Id);

        Tarea tareaOriginal = _repoTareas.GetById(_tareaEjemplo.Id);
        Tarea tareaDependencia = _repoTareas.GetById(tarea2.Id);

        Assert.IsTrue(tareaOriginal.TareasDependencia.Contains(tareaDependencia));
        Assert.IsTrue(tareaDependencia.TareasSucesoras.Contains(tareaOriginal));
    }

    [TestMethod]
    public void AgregarUsuarioAgregaUsuarioCorrectamente()
    {
        _service.AgregarUsuario(_tareaEjemplo.Id, _usuarioEjemplo.Id);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.IsNotNull(tareaGuardada);
        Assert.IsTrue(tareaGuardada.UsuariosAsignados.Contains(_usuarioEjemplo));
    }

    [TestMethod]
    public void TieneSucesorasDevuelveFalseCuandoNoHaySucesoras()
    {
        TareaDTO dtoSinSucesoras = Convertidor.ATareaDTO(_tareaEjemplo);
        bool resultado = _service.TieneSucesoras(dtoSinSucesoras);
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void TieneSucesorasDevuelveTrueCuandoHaySucesoras()
    {
        Tarea tarea2 = new Tarea(
            "Tarea 2",
            "Desc 2",
            DateTime.Today.AddHours(1),
            TimeSpan.FromHours(3),
            true);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _service.AgregarDependencia(_tareaEjemplo.Id, tarea2.Id);

        TareaDTO dtoConSucesoras = Convertidor.ATareaDTO(tarea2);

        bool resultado = _service.TieneSucesoras(dtoConSucesoras);

        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void UsuarioPerteneceALaTareaDevuelveTrueCuandoUsuarioAsignado()
    {
        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        bool resultado = _service.UsuarioPerteneceALaTarea(_usuarioEjemplo.Id, _tareaEjemplo.Id);
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void EliminarTareaSinSucesorasNiDependenciasEliminaCorrectamente()
    {
        _service.EliminarTarea(_proyectoEjemplo.Id, _tareaEjemplo.Id);

        Assert.IsFalse(_proyectoEjemplo.TareasAsociadas.Contains(_tareaEjemplo));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EliminarTarea_LanzaExcepcionSiTieneSucesoras()
    {
        Tarea tarea2 = new Tarea(
            "Tarea 2",
            "Desc",
            DateTime.Today.AddHours(9),
            TimeSpan.FromHours(4),
            false
        );
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        tarea2.AgregarDependencia(_tareaEjemplo);

        _service.EliminarTarea(_proyectoEjemplo.Id, _tareaEjemplo.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EliminarTarea_LanzaExcepcionSiTieneDependencias()
    {
        Tarea tarea2 = new Tarea(
            "Tarea 2",
            "Desc",
            DateTime.Today.AddHours(9),
            TimeSpan.FromHours(4),
            false
        );
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _tareaEjemplo.AgregarDependencia(tarea2);

        _service.EliminarTarea(_proyectoEjemplo.Id, _tareaEjemplo.Id);
    }

    [TestMethod]
    public void TieneDependenciasReturnTrueCuandoTiene()
    {
        Tarea _tarea2 = new Tarea("Tarea Test",
            "Descripción Tarea",
            DateTime.Today.AddHours(9),
            TimeSpan.FromHours(4),
            false);
        _proyectoEjemplo.TareasAsociadas.Add(_tarea2);
        _repoTareas.Add(_tarea2);
        _tareaEjemplo.AgregarDependencia(_tarea2);
        bool resultado = _service.TieneDependencias(Convertidor.ATareaDTO(_tareaEjemplo));
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void GetEstadoTarea_DevuelveEstadoInicial()
    {
        TipoEstadoTarea estado = _service.GetEstadoTarea(_tareaEjemplo.Id);

        Assert.AreEqual(TipoEstadoTarea.Pendiente, estado);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void GetEstadoTarea_LanzaExcepcion_CuandoTareaNoExiste()
    {
        _service.GetEstadoTarea(999);
    }

    [TestMethod]
    public void ListarUsuariosDeTarea_ConTareaExistenteYUsuarios_DevuelveListaUsuarios()
    {
        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);

        List<UsuarioDTO> resultado = _service.ListarUsuariosDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(_usuarioEjemplo.Email, resultado[0].Email);
    }

    [TestMethod]
    public void ListarUsuariosDeTarea_ConTareaExistenteSinUsuarios_DevuelveListaVacia()
    {
        List<UsuarioDTO> resultado = _service.ListarUsuariosDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ListarUsuariosDeTarea_ConTareaInexistente_DevuelveNull()
    {
        int idInexistente = 9999;
        List<UsuarioDTO> resultado = _service.ListarUsuariosDeTarea(idInexistente);
    }

    [TestMethod]
    public void ListarUsuariosDeTarea_ConMultiplesUsuarios_DevuelveTodosCorrectamente()
    {
        Usuario usuario2 = new Usuario(
            "user2@test.com",
            "User2",
            "Test2",
            "Password2!",
            DateTime.Today.AddYears(-25));
        _repoUsuarios.Add(usuario2);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        _tareaEjemplo.AgregarUsuario(usuario2);

        List<UsuarioDTO> resultado = _service.ListarUsuariosDeTarea(_tareaEjemplo.Id);

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
        Usuario usuario2 = new Usuario(
            "user2@test.com",
            "User2",
            "Test2",
            "Password2!",
            DateTime.Today.AddYears(-25));
        _repoUsuarios.Add(usuario2);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        int cantidadInicial = _tareaEjemplo.UsuariosAsignados.Count;

        _service.EliminarUsuarioDeTarea(usuario2.Id, _tareaEjemplo.Id);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(cantidadInicial, tareaGuardada.UsuariosAsignados.Count);
        Assert.IsTrue(tareaGuardada.UsuariosAsignados.Contains(_usuarioEjemplo));
    }

    [TestMethod]
    public void EliminarUsuarioDeTareasDeProyecto_ConUsuarioAsignadoAVariasTareas_EliminaDeTodasLasTareas()
    {
        Tarea tarea2 = new Tarea(
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

        Tarea tareaGuardada1 = _repoTareas.GetById(_tareaEjemplo.Id);
        Tarea tareaGuardada2 = _repoTareas.GetById(tarea2.Id);

        Assert.IsFalse(tareaGuardada1.UsuariosAsignados.Contains(_usuarioEjemplo));
        Assert.IsFalse(tareaGuardada2.UsuariosAsignados.Contains(_usuarioEjemplo));
    }

    [TestMethod]
    public void EliminarUsuarioDeTareasDeProyecto_ConUsuarioNoAsignadoANingunaTarea_NoAfectaNingunaTarea()
    {
        Usuario usuarioNoAsignado = new Usuario(
            "noasignado@test.com",
            "No",
            "Asignado",
            "Password1!",
            DateTime.Today.AddYears(-30));
        _repoUsuarios.Add(usuarioNoAsignado);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        int cantidadInicial = _tareaEjemplo.UsuariosAsignados.Count;

        _service.EliminarUsuarioDeTareasDeProyecto(usuarioNoAsignado.Id, _proyectoEjemplo.Id);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(cantidadInicial, tareaGuardada.UsuariosAsignados.Count);
        Assert.IsTrue(tareaGuardada.UsuariosAsignados.Contains(_usuarioEjemplo));
    }

    [TestMethod]
    public void EliminarUsuarioDeTareasDeProyecto_ConUsuarioAsignadoSoloAAlgunasTareas_EliminaSoloDeEsasTareas()
    {
        Usuario usuario2 = new Usuario(
            "user2@test.com",
            "User2",
            "Test2",
            "Password2!",
            DateTime.Today.AddYears(-25));
        _repoUsuarios.Add(usuario2);

        Tarea tarea2 = new Tarea(
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

        Tarea tareaGuardada1 = _repoTareas.GetById(_tareaEjemplo.Id);
        Tarea tareaGuardada2 = _repoTareas.GetById(tarea2.Id);

        Assert.IsFalse(tareaGuardada1.UsuariosAsignados.Contains(_usuarioEjemplo));
        Assert.IsTrue(tareaGuardada1.UsuariosAsignados.Contains(usuario2));
        Assert.IsTrue(tareaGuardada2.UsuariosAsignados.Contains(usuario2));
        Assert.IsFalse(tareaGuardada2.UsuariosAsignados.Contains(_usuarioEjemplo));
    }

    [TestMethod]
    public void EliminarUsuarioDeTareasDeProyecto_ConProyectoSinTareas_NoGeneraError()
    {
        Proyecto proyectoVacio = new Proyecto(
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
        Usuario usuario2 = new Usuario(
            "user2@test.com",
            "User2",
            "Test2",
            "Password2!",
            DateTime.Today.AddYears(-25));
        _repoUsuarios.Add(usuario2);

        Usuario usuario3 = new Usuario(
            "user3@test.com",
            "User3",
            "Test3",
            "Password3!",
            DateTime.Today.AddYears(-28));
        _repoUsuarios.Add(usuario3);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        _tareaEjemplo.AgregarUsuario(usuario2);
        _tareaEjemplo.AgregarUsuario(usuario3);

        int cantidadInicial = _tareaEjemplo.UsuariosAsignados.Count;

        _service.EliminarUsuarioDeTareasDeProyecto(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);

        Assert.AreEqual(cantidadInicial - 1, tareaGuardada.UsuariosAsignados.Count);
        Assert.IsFalse(tareaGuardada.UsuariosAsignados.Contains(_usuarioEjemplo));
        Assert.IsTrue(tareaGuardada.UsuariosAsignados.Contains(usuario2));
        Assert.IsTrue(tareaGuardada.UsuariosAsignados.Contains(usuario3));
    }
    
    [TestMethod]
    public void ObtenerDependenciasDeTarea_ConTareaSinDependencias_DevuelveListaVacia()
    {
        List<TareaDTO> resultado = _service.ObtenerDependenciasDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void ObtenerDependenciasDeTarea_ConTareaConDependencias_DevuelveListaConDependencias()
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

        List<TareaDTO> resultado = _service.ObtenerDependenciasDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(tarea2.Id, resultado[0].Id);
        Assert.AreEqual(tarea2.Titulo, resultado[0].Titulo);
    }

    [TestMethod]
    public void ObtenerDependenciasDeTarea_ConMultiplesDependencias_DevuelveTodasLasDependencias()
    {
        Tarea tarea2 = new Tarea("Dependencia 1", "Desc 1", DateTime.Today.AddHours(1), TimeSpan.FromHours(2), false);
        Tarea tarea3 = new Tarea("Dependencia 2", "Desc 2", DateTime.Today.AddHours(2), TimeSpan.FromHours(3), false);
        
        _repoTareas.Add(tarea2);
        _repoTareas.Add(tarea3);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _proyectoEjemplo.TareasAsociadas.Add(tarea3);

        _tareaEjemplo.AgregarDependencia(tarea2);
        _tareaEjemplo.AgregarDependencia(tarea3);

        List<TareaDTO> resultado = _service.ObtenerDependenciasDeTarea(_tareaEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(2, resultado.Count);
        Assert.IsTrue(resultado.Any(t => t.Id == tarea2.Id));
        Assert.IsTrue(resultado.Any(t => t.Id == tarea3.Id));
    }

    [TestMethod]
    public void EliminarDependencia_ConDependenciaExistente_EliminaCorrectamente()
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

        _service.EliminarDependencia(_tareaEjemplo.Id, tarea2.Id);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Tarea dependenciaGuardada = _repoTareas.GetById(tarea2.Id);

        Assert.IsFalse(tareaGuardada.TareasDependencia.Contains(dependenciaGuardada));
        Assert.IsFalse(dependenciaGuardada.TareasSucesoras.Contains(tareaGuardada));
    }

    [TestMethod]
    public void EliminarDependencia_ConDependenciaNoExistente_NoAfectaLaTarea()
    {
        Tarea tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddHours(1), TimeSpan.FromHours(3), false);
        Tarea tarea3 = new Tarea("Tarea 3", "Desc 3", DateTime.Today.AddHours(2), TimeSpan.FromHours(2), false);
        _repoTareas.Add(tarea2);
        _repoTareas.Add(tarea3);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _proyectoEjemplo.TareasAsociadas.Add(tarea3);

        _tareaEjemplo.AgregarDependencia(tarea2);
        int cantidadInicial = _tareaEjemplo.TareasDependencia.Count;

        _service.EliminarDependencia(_tareaEjemplo.Id, tarea3.Id);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(cantidadInicial, tareaGuardada.TareasDependencia.Count);
        Assert.IsTrue(tareaGuardada.TareasDependencia.Contains(tarea2));
    }

    [TestMethod]
    public void ListarTareasDelUsuario_ConUsuarioSinTareasAsignadas_DevuelveListaVacia()
    {
        List<TareaDTO> resultado = _service.ListarTareasDelUsuario(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void ListarTareasDelUsuario_ConUsuarioConTareasAsignadas_DevuelveTareasDelUsuario()
    {
        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);

        List<TareaDTO> resultado = _service.ListarTareasDelUsuario(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(_tareaEjemplo.Id, resultado[0].Id);
        Assert.AreEqual(_tareaEjemplo.Titulo, resultado[0].Titulo);
    }

    [TestMethod]
    public void ListarTareasDelUsuario_ConMultiplesTareasAsignadas_DevuelveTodasLasTareasDelUsuario()
    {
        Tarea tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddDays(1), TimeSpan.FromHours(2), false);
        Tarea tarea3 = new Tarea("Tarea 3", "Desc 3", DateTime.Today.AddDays(2), TimeSpan.FromHours(1), false);
        
        _repoTareas.Add(tarea2);
        _repoTareas.Add(tarea3);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _proyectoEjemplo.TareasAsociadas.Add(tarea3);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        tarea2.AgregarUsuario(_usuarioEjemplo);

        List<TareaDTO> resultado = _service.ListarTareasDelUsuario(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(2, resultado.Count);
        Assert.IsTrue(resultado.Any(t => t.Id == _tareaEjemplo.Id));
        Assert.IsTrue(resultado.Any(t => t.Id == tarea2.Id));
        Assert.IsFalse(resultado.Any(t => t.Id == tarea3.Id));
    }

    [TestMethod]
    public void ListarTareasDelUsuario_ConUsuarioAsignadoSoloAAlgunasTareas_DevuelveSoloEsasTareas()
    {
        Usuario usuario2 = new Usuario(
            "user2@test.com",
            "User2",
            "Test2",
            "Password2!",
            DateTime.Today.AddYears(-25));
        _repoUsuarios.Add(usuario2);

        Tarea tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddDays(1), TimeSpan.FromHours(2), false);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _repoTareas.Add(tarea2);

        _tareaEjemplo.AgregarUsuario(_usuarioEjemplo);
        _tareaEjemplo.AgregarUsuario(usuario2);
        tarea2.AgregarUsuario(usuario2);

        List<TareaDTO> resultado = _service.ListarTareasDelUsuario(_usuarioEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(_tareaEjemplo.Id, resultado[0].Id);
    }

    [TestMethod]
    public void PuedeCambiarDeEstado_ConTareaEnEstadoPendiente_DevuelveTrue()
    {
        bool resultado = _service.PuedeCambiarDeEstado(_tareaEjemplo.Id);

        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void ObtenerTareasParaAgregarDependencia_ConProyectoSinOtrasTareas_DevuelveListaVacia()
    {
        List<TareaDTO> resultado = _service.ObtenerTareasParaAgregarDependencia(_tareaEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void ObtenerTareasParaAgregarDependencia_ConOtrasTareasDisponibles_DevuelveTareasDisponibles()
    {
        Tarea tarea2 = new Tarea("Tarea 2", "Desc 2", DateTime.Today.AddDays(1), TimeSpan.FromHours(2), false);
        Tarea tarea3 = new Tarea("Tarea 3", "Desc 3", DateTime.Today.AddDays(2), TimeSpan.FromHours(1), false);
        
        _repoTareas.Add(tarea2);
        _repoTareas.Add(tarea3);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _proyectoEjemplo.TareasAsociadas.Add(tarea3);

        List<TareaDTO> resultado = _service.ObtenerTareasParaAgregarDependencia(_tareaEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(2, resultado.Count);
        Assert.IsTrue(resultado.Any(t => t.Id == tarea2.Id));
        Assert.IsTrue(resultado.Any(t => t.Id == tarea3.Id));
        Assert.IsFalse(resultado.Any(t => t.Id == _tareaEjemplo.Id));
    }

    [TestMethod]
    public void ObtenerTareasParaAgregarDependencia_ExcluyeSucesoras_DevuelveSoloTareasDisponibles()
    {
        Tarea tarea2 = new Tarea("Sucesora", "Desc 2", DateTime.Today.AddDays(1), TimeSpan.FromHours(2), false);
        Tarea tarea3 = new Tarea("Disponible", "Desc 3", DateTime.Today.AddDays(2), TimeSpan.FromHours(1), false);
        
        _repoTareas.Add(tarea2);
        _repoTareas.Add(tarea3);
        _proyectoEjemplo.TareasAsociadas.Add(tarea2);
        _proyectoEjemplo.TareasAsociadas.Add(tarea3);

        tarea2.AgregarDependencia(_tareaEjemplo);

        List<TareaDTO> resultado = _service.ObtenerTareasParaAgregarDependencia(_tareaEjemplo.Id, _proyectoEjemplo.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual(tarea3.Id, resultado[0].Id);
        Assert.IsFalse(resultado.Any(t => t.Id == tarea2.Id));
    }

    [TestMethod]
    public void PuedeAgregarDependencias_ConTareaEnEstadoPendiente_DevuelveTrue()
    {
        bool resultado = _service.PuedeAgregarDependencias(_tareaEjemplo.Id);

        Assert.IsTrue(resultado);
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
    public void MarcarComoEjecutandose_SinAsignacionesDeRecursos_CambiaEstado()
    {
        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(TipoEstadoTarea.Ejecutandose, tareaGuardada.EstadoActual.Valor);
    }

    [TestMethod]
    public void MarcarComoCompletada_SinAsignacionesDeRecursos_CambiaEstado()
    {
        _tareaEjemplo.MarcarTareaComoEjecutandose();
        _repoTareas.Update(_tareaEjemplo);

        _service.MarcarComoCompletada(_tareaEjemplo.Id);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(TipoEstadoTarea.Efectuada, tareaGuardada.EstadoActual.Valor);
    }

    [TestMethod]
    public void ActualizarEstadoTarea_ConEstadoCompletada_NoModificaEstado()
    {
        _tareaEjemplo.MarcarTareaComoCompletada();
        _repoTareas.Update(_tareaEjemplo);
        
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);

        _service.ActualizarEstadoTarea(TipoEstadoTarea.Efectuada, tareaDTO);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(TipoEstadoTarea.Efectuada, tareaGuardada.EstadoActual.Valor);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearTarea_ConFechaInicioMenorAlProyecto_LanzaExcepcion()
    {
        TareaDTO dtoNueva = new TareaDTO
        {
            Titulo = "Nueva Tarea",
            Descripcion = "Descripción nueva",
            FechaInicio = _proyectoEjemplo.FechaInicio.AddDays(-1), 
            Duracion = TimeSpan.FromHours(4)
        };

        _service.CrearTarea(_proyectoEjemplo.Id, dtoNueva);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearTarea_ConFechaInicioIgualAlProyecto_LanzaExcepcion()
    {
        TareaDTO dtoNueva = new TareaDTO
        {
            Titulo = "Nueva Tarea",
            Descripcion = "Descripción nueva",
            FechaInicio = _proyectoEjemplo.FechaInicio, // Fecha igual al proyecto
            Duracion = TimeSpan.FromHours(4)
        };

        _service.CrearTarea(_proyectoEjemplo.Id, dtoNueva);
    }

    [TestMethod]
    public void MarcarComoEjecutandose_ConDependenciasNoCompletadas_NoModificaEstado()
    {
        Tarea dependencia = new Tarea(
            "Dependencia",
            "Descripción dependencia",
            DateTime.Today.AddHours(1),
            TimeSpan.FromHours(2),
            false);
        _proyectoEjemplo.TareasAsociadas.Add(dependencia);
        _repoTareas.Add(dependencia);
        
        _tareaEjemplo.AgregarDependencia(dependencia);

        TipoEstadoTarea estadoInicial = _tareaEjemplo.EstadoActual.Valor;

        _service.MarcarComoEjecutandose(_tareaEjemplo.Id);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(estadoInicial, tareaGuardada.EstadoActual.Valor); 
    }

    [TestMethod]
    public void ActualizarEstadoTarea_ConEstadoPendiente_LlamaMarcarComoEjecutandose()
    {
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);

        _service.ActualizarEstadoTarea(TipoEstadoTarea.Pendiente, tareaDTO);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(TipoEstadoTarea.Ejecutandose, tareaGuardada.EstadoActual.Valor);
    }

    [TestMethod]
    public void ActualizarEstadoTarea_ConEstadoEjecutandose_LlamaMarcarComoCompletada()
    {
        _tareaEjemplo.MarcarTareaComoEjecutandose();
        _repoTareas.Update(_tareaEjemplo);
        
        TareaDTO tareaDTO = Convertidor.ATareaDTO(_tareaEjemplo);

        _service.ActualizarEstadoTarea(TipoEstadoTarea.Ejecutandose, tareaDTO);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(TipoEstadoTarea.Efectuada, tareaGuardada.EstadoActual.Valor);
    }

    [TestMethod]
    public void MarcarComoCompletada_VerificaLlamadaDuplicadaGetAsignaciones()
    {
        _tareaEjemplo.MarcarTareaComoEjecutandose();
        _repoTareas.Update(_tareaEjemplo);

        _service.MarcarComoCompletada(_tareaEjemplo.Id);

        Tarea tareaGuardada = _repoTareas.GetById(_tareaEjemplo.Id);
        Assert.AreEqual(TipoEstadoTarea.Efectuada, tareaGuardada.EstadoActual.Valor);
    }

    [TestMethod]
    public void PuedeCambiarDeEstado_ConTareaEnEstadoEjecutandose_DevuelveTrue()
    {
        _tareaEjemplo.MarcarTareaComoEjecutandose();
        _repoTareas.Update(_tareaEjemplo);

        bool resultado = _service.PuedeCambiarDeEstado(_tareaEjemplo.Id);

        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void PuedeCambiarDeEstado_ConTareaEnEstadoCompletada_DevuelveFalse()
    {
        _tareaEjemplo.MarcarTareaComoCompletada();
        _repoTareas.Update(_tareaEjemplo);

        bool resultado = _service.PuedeCambiarDeEstado(_tareaEjemplo.Id);

        Assert.IsFalse(resultado);
    }
    
    [TestMethod]
    public void PuedeAgregarDependencias_ConTareaEnEstadoEjecutandose_DevuelveFalse()
    {
        _tareaEjemplo.MarcarTareaComoEjecutandose();
        _repoTareas.Update(_tareaEjemplo);

        bool resultado = _service.PuedeAgregarDependencias(_tareaEjemplo.Id);

        Assert.IsFalse(resultado);
    }
}