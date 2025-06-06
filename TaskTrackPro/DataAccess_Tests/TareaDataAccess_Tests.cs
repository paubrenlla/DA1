using Domain;
using DataAccess;

namespace DataAccess_Tests;

[TestClass]
public class TareaDataAccess_Tests
{
    private TareaDataAccess tareaRepo;

    [TestInitialize]
    public void SetUp()
    {
        tareaRepo = new TareaDataAccess();
    }
    
    [TestMethod]
    public void CrearNuevaTarea()
    {
        Tarea tarea = new Tarea("Tarea", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        tareaRepo.Add(tarea);
        
        Assert.AreEqual(1, tareaRepo.GetAll().Count);
        Assert.AreEqual(tarea, tareaRepo.GetAll()[0]);
    }
    
    [TestMethod]
    public void BuscarTareaPorIdDevuelveBien()
    {
        Tarea tarea = new Tarea("Tarea", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        Tarea tarea2 = new Tarea("Tarea2", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        tareaRepo.Add(tarea);
        tareaRepo.Add(tarea2);
        
        Tarea resultado = tareaRepo.GetById(tarea2.Id);
        Assert.AreEqual(resultado, tarea2);
    }
    
    [TestMethod]
    public void EliminarTarea()
    {
        Tarea tarea = new Tarea("Tarea", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        Tarea tarea2 = new Tarea("Tarea2", "Descripcion", DateTime.Now, TimeSpan.FromDays(1), false);
        tareaRepo.Add(tarea);
        tareaRepo.Add(tarea2);
        
        Assert.AreEqual(2, tareaRepo.GetAll().Count);
        
        tareaRepo.Remove(tarea);
        
        Assert.AreEqual(1, tareaRepo.GetAll().Count);
        Assert.IsFalse(tareaRepo.GetAll().Contains(tarea));
    }
    
    [TestMethod]
    public void GetTareasDeUsuarioEnProyecto_NoDevuelveTareas_SiProyectoNoCoincide()
    {
        Tarea tarea = new Tarea("Tarea 1", "Desc", DateTime.Now, TimeSpan.FromDays(1), false);
        Usuario usuario = new Usuario("juan@gmail.com", "Juan", "Ignacio", "Contrase1a!", DateTime.Now);
        Proyecto proyectoCorrecto = new Proyecto("Proyecto", "desc", DateTime.Now);
        Proyecto otroProyecto = new Proyecto("Otro Proyecto", "desc", DateTime.Now);

        tarea.UsuariosAsignados.Add(usuario);
        tarea.Proyecto = proyectoCorrecto;
        tareaRepo.Add(tarea);

        List<Tarea> resultado = tareaRepo.GetTareasDeUsuarioEnProyecto(usuario.Id, otroProyecto.Id);

        Assert.AreEqual(0, resultado.Count);
    }

    
    [TestMethod]
    public void GetTareasDeUsuarioEnProyecto_NoDevuelveTareas_SiUsuarioNoAsignado()
    {
        Tarea tarea = new Tarea("Tarea 1", "Desc", DateTime.Now, TimeSpan.FromDays(1), false);
        Usuario usuario = new Usuario("juan@gmail.com", "Juan", "Ignacio", "Contrase1a!", DateTime.Now);
        Usuario otroUsuario = new Usuario("juan2@gmail.com", "Juan", "Ignacio", "Contrase1a!", DateTime.Now);

        Proyecto proyecto = new Proyecto("Proyecto", "desc", DateTime.Now);

        tarea.UsuariosAsignados.Add(usuario);
        tarea.Proyecto = proyecto;
        tareaRepo.Add(tarea);

        List<Tarea> resultado = tareaRepo.GetTareasDeUsuarioEnProyecto(otroUsuario.Id, proyecto.Id);

        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void GetTareasDeUsuarioEnProyecto_DevuelveMultiplesTareasCorrectas()
    {
        Usuario usuario = new Usuario("juan@gmail.com", "Juan", "Ignacio", "Contrase1a!", DateTime.Now);
        Proyecto proyecto = new Proyecto("Proyecto", "desc", DateTime.Now);

        Tarea tarea1 = new Tarea("Tarea 1", "Desc", DateTime.Now, TimeSpan.FromDays(1), false);
        Tarea tarea2 = new Tarea("Tarea 2", "Desc", DateTime.Now, TimeSpan.FromDays(2), false);

        tarea1.UsuariosAsignados.Add(usuario);
        tarea2.UsuariosAsignados.Add(usuario);

        tarea1.Proyecto = proyecto;
        tarea2.Proyecto = proyecto;

        tareaRepo.Add(tarea1);
        tareaRepo.Add(tarea2);

        List<Tarea> resultado = tareaRepo.GetTareasDeUsuarioEnProyecto(usuario.Id, proyecto.Id);

        Assert.AreEqual(2, resultado.Count);
        CollectionAssert.Contains(resultado, tarea1);
        CollectionAssert.Contains(resultado, tarea2);
    }
}