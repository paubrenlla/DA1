using Domain;
using IDataAcces;
using Repositorios;
using Controllers;
using Repositorios.DTOs;

namespace Controllers_Tests;

[TestClass]
public class ProyectoControllerTests
{
    private ProyectoController _proyectoController;
    private UsuarioController _usuarioController;
    private IDataAccessProyecto _repoProyectos;
    private IDataAccessUsuario _repoUsuarios;

    private Proyecto proyectoEjemplo;
    private Usuario usuarioEjemplo;

    [TestInitialize]
    public void SetUp()
    {
        _repoProyectos = new ProyectoDataAccess();
        _repoUsuarios = new UsuarioDataAccess();
        _proyectoController = new ProyectoController(_repoProyectos, _repoUsuarios);

        proyectoEjemplo = new Proyecto("Proyecto 1", "Descripción del proyecto", DateTime.Today.AddDays(1));
        usuarioEjemplo = new Usuario("pepe@gmail.com", "Pepe", "Perez", "Contraseña1!",DateTime.Today);
        _repoProyectos.Add(proyectoEjemplo);
        _repoUsuarios.Add(usuarioEjemplo);
    }
    
    [TestMethod]
    public void BuscarProyectoPorIdDevuelveDTOCorrecto()
    {
        ProyectoDTO dto = _proyectoController.BuscarProyectoPorId(proyectoEjemplo.Id);

        Assert.AreEqual(proyectoEjemplo.Id, dto.Id);
        Assert.AreEqual(proyectoEjemplo.Nombre, dto.Nombre);
        Assert.AreEqual(proyectoEjemplo.Descripcion, dto.Descripcion);
    }
    
    [TestMethod]
    public void ListarTodosLosProyectosExistentes()
    {
        Proyecto otroProyecto = new Proyecto("Proyecto 2", "Otro proyecto", DateTime.Today.AddDays(2));
        _repoProyectos.Add(otroProyecto);

        List<ProyectoDTO> lista = _proyectoController.GetAll();

        Assert.AreEqual(2, lista.Count);
        Assert.AreEqual(otroProyecto.Nombre, lista[1].Nombre);
        Assert.AreEqual(proyectoEjemplo.Nombre, lista[0].Nombre);
    }
    
    [TestMethod]
    public void AgregarProyectoAgregaCorrectamente()
    {
        ProyectoDTO dto = new ProyectoDTO
        {
            Nombre = "Nuevo Proyecto",
            Descripcion = "Descripción del nuevo proyecto"
        };
        DateTime fechaInicio = DateTime.Today.AddDays(5);

        _proyectoController.AgregarProyecto(dto, fechaInicio);

        List<ProyectoDTO> lista = _proyectoController.GetAll();
        ProyectoDTO? proyectoAgregado = lista.FirstOrDefault(p => p.Nombre == dto.Nombre && p.Descripcion == dto.Descripcion);

        Assert.IsNotNull(proyectoAgregado);
        Proyecto proyectoEnRepo = _repoProyectos.GetById(proyectoAgregado.Id);
        Assert.AreEqual(dto.Nombre, proyectoEnRepo.Nombre);
        Assert.AreEqual(dto.Descripcion, proyectoEnRepo.Descripcion);
        Assert.AreEqual(fechaInicio, proyectoEnRepo.FechaInicio);
    }

    [TestMethod]
    public void EliminarProyectoEliminaCorrectamente()
    {
        _proyectoController.EliminarProyecto(proyectoEjemplo.Id);

        List<ProyectoDTO> lista = _proyectoController.GetAll();
        bool existe = lista.Any(p => p.Id == proyectoEjemplo.Id);
        Assert.IsFalse(existe);
    }

    [TestMethod]
    public void ModificarProyectoModificaCorrectamente()
    {
        ProyectoDTO proyecto = Convertidor.AProyectoDTO(proyectoEjemplo);
        proyecto.Descripcion = "Descripcion nueva";
        _proyectoController.ModificarProyecto(proyecto, DateTime.Today.AddDays(4));

        Assert.AreEqual(_repoProyectos.GetById(proyecto.Id).FechaInicio, DateTime.Today.AddDays(4));
        Assert.AreEqual("Descripcion nueva", _repoProyectos.GetById(proyecto.Id).Descripcion);
    }
    
    [TestMethod]
    public void UsuarioEsAdminDeAlgunProyecto()
    {
        
        proyectoEjemplo.Admin = usuarioEjemplo;
       bool esAdminDeAlgunProyecto = _proyectoController.EsAdminDeAlgunProyecto(Convertidor.AUsuarioDTO(usuarioEjemplo));
       Assert.IsTrue(esAdminDeAlgunProyecto);
    }

    [TestMethod]
    public void EliminarAsignacionesDeProyectosEliminaAsignacionesCorrectamente()
    {
        Usuario usuarioAdmin = new Usuario("admin@gmail.com", "admin","admin", "Contraseña1!", DateTime.Today.AddDays(-1));
        proyectoEjemplo.agregarMiembro(usuarioEjemplo);
        proyectoEjemplo.Admin = usuarioAdmin;

        var tarea = new Tarea("Tarea de ejemplo", "desc",DateTime.Today.AddDays(1), TimeSpan.FromDays(2), false);
        proyectoEjemplo.agregarTarea(tarea);
        proyectoEjemplo.AsignarUsuarioATarea(usuarioEjemplo, tarea);

        var usuarioDTO = new UsuarioDTO { Id = usuarioEjemplo.Id };

        Assert.IsTrue(proyectoEjemplo.Miembros.Contains(usuarioEjemplo));
        Assert.IsTrue(tarea.UsuariosAsignados.Contains(usuarioEjemplo));

        _proyectoController.EliminarAsignacionesDeProyectos(usuarioDTO);

        Assert.IsFalse(proyectoEjemplo.Miembros.Contains(usuarioEjemplo));
        Assert.IsFalse(tarea.UsuariosAsignados.Contains(usuarioEjemplo));
    }

    [TestMethod]
    public void DevolverLosProyectosDeUnUsuario()
    {
        proyectoEjemplo.Admin = usuarioEjemplo;
       List<ProyectoDTO> proyectosDelUsuario = _proyectoController.ProyectosDelUsuario(Convertidor.AUsuarioDTO(usuarioEjemplo));
       Assert.IsTrue(proyectosDelUsuario.Count == 1);
       Assert.IsTrue(proyectosDelUsuario[0].Id == proyectoEjemplo.Id);
    }

    [TestMethod]
    public void UsuarioEsAdminDelProyectoDevuelveTrue()
    {
        proyectoEjemplo.Admin = usuarioEjemplo;
        bool esAdmin = _proyectoController.UsuarioEsAdminDelProyecto(Convertidor.AUsuarioDTO(usuarioEjemplo), Convertidor.AProyectoDTO(proyectoEjemplo));
        Assert.IsTrue(esAdmin);
    }

}
