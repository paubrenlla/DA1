using BusinessLogic;
using Repositorios;

namespace DataAccess_Tests;

[TestClass]
public class ProyectoDataAccess_Tests
{
    private ProyectoDataAccess ProyectoRepo;

    [TestInitialize]
    public void SetUp()
    {
        ProyectoRepo = new ProyectoDataAccess();
    }
    
    [TestMethod]
    public void AgregarProyecto()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jej";
        DateTime fechaInicio = DateTime.Today;
        

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        ProyectoRepo.Add(proyecto);
        Assert.AreEqual(1, ProyectoRepo.GetAll().Count);
        Assert.AreSame(proyecto, ProyectoRepo.GetAll()[0]);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarProyectoYaExistenteEnElSistema()
    {
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jej";
        DateTime fechaInicio = DateTime.Today;
        

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        ProyectoRepo.Add(proyecto);
        ProyectoRepo.Add(proyecto);
    }
    
    [TestMethod]
    public void EliminarProyecto()
    {
        
        string nombre = "Proyecto A";
        string descripcion = "Este es un proyecto para el TDD jej";
        DateTime fechaInicio = DateTime.Today;
        

        Proyecto proyecto = new Proyecto(nombre, descripcion, fechaInicio);
        
        ProyectoRepo.Add(proyecto);
        
        Assert.AreEqual(1, ProyectoRepo.GetAll().Count);
        Assert.AreSame(proyecto, ProyectoRepo.GetAll()[0]);
        
        ProyectoRepo.Remove(proyecto);
        Assert.AreEqual(0, ProyectoRepo.GetAll().Count);
        Assert.IsFalse(ProyectoRepo.GetAll().Contains(proyecto));
    }
    
    [TestMethod]
    public void BuscarProyectoPorIdDevuelveProyectoCorrecto()
    { 
        DB db = new DB();
        Proyecto p1= new Proyecto("Proyecto 1", "desc", DateTime.Today);
        Proyecto p2= new Proyecto("Proyecto 1", "desc", DateTime.Today);
        
        ProyectoRepo.Add(p1);
        ProyectoRepo.Add(p2);
        
        Proyecto resultado = ProyectoRepo.GetById(p2.Id);

        Assert.IsNotNull(resultado);
        Assert.AreEqual(p2.Id, resultado.Id);
        Assert.AreEqual(p2.Nombre, resultado.Nombre);
    }
    
    [TestMethod]
    public void DevolverProyectosDeUnUsuario()
    {
        Usuario usuario1 = new Usuario("correo@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Usuario usuario2 = new Usuario("correo2@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Proyecto p1= new Proyecto("Proyecto 1", "desc", DateTime.Today);
        Proyecto p2= new Proyecto("Proyecto 2", "desc", DateTime.Today);
        Proyecto p3= new Proyecto("Proyecto 3", "desc", DateTime.Today);
        p1.AsignarAdmin(usuario1);
        p2.AsignarAdmin(usuario2);
        p3.AsignarAdmin(usuario2);
        p2.agregarMiembro(usuario1);
        
        ProyectoRepo.Add(p1);
        ProyectoRepo.Add(p2);
        ProyectoRepo.Add(p3);
        List<Proyecto> proyectosDelUsuario = ProyectoRepo.ProyectosDelUsuario(usuario1);
        
        Assert.IsTrue(proyectosDelUsuario.Count == 2);
        Assert.IsTrue(proyectosDelUsuario.Contains(p1));
        Assert.IsTrue(proyectosDelUsuario.Contains(p2));
        Assert.IsFalse(proyectosDelUsuario.Contains(p3));
    }
    
    [TestMethod]
    public void VerSiUsuarioEsAdminDeUnProyecto()
    {
        
        Usuario usuario1 = new Usuario("correo@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Usuario usuario2 = new Usuario("correo2@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Proyecto p1= new Proyecto("Proyecto 1", "desc", DateTime.Today);
        p1.AsignarAdmin(usuario1);
        p1.agregarMiembro(usuario2);
        ProyectoRepo.Add(p1);
        
        bool Usuario1esAdmin = ProyectoRepo.esAdminDeAlgunProyecto(usuario1);
        bool Usuario2NoesAdmin = ProyectoRepo.esAdminDeAlgunProyecto(usuario2);
        
        Assert.IsTrue(Usuario1esAdmin);
        Assert.IsFalse(Usuario2NoesAdmin);
    }
    
    [TestMethod]
    public void EliminarAsignacionesDeProyectoDeUnUsuario()
    {
        Usuario usuario = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        Usuario usuario2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        Proyecto proyecto = new Proyecto("Proyecto","descripcion", DateTime.Today);
        proyecto.agregarMiembro(usuario);
        proyecto.AsignarAdmin(usuario2);
        Tarea tarea = new Tarea("Tarea", "descripcion", DateTime.Today, TimeSpan.FromDays(1), false);
        proyecto.agregarTarea(tarea);
        tarea.AgregarUsuario(usuario);
        
        ProyectoRepo.Add(proyecto);
        ProyectoRepo.EliminarAsignacionesDeProyectos(usuario);
        
        Assert.IsTrue(tarea.UsuariosAsignados.Count == 0);
        Assert.IsTrue(proyecto.Miembros.Count == 0);
        Assert.IsFalse(tarea.UsuariosAsignados.Contains(usuario));
        Assert.IsFalse(proyecto.Miembros.Contains(usuario));
    }
}