using BusinessLogic;
using Repositorios;

namespace DataAccess_Tests;

[TestClass]
public class NotificacionDataAccess_Tests
{
    private NotificacionDataAccess NotificacionRepo;
    
    [TestInitialize]
    public void SetUp()
    {
        NotificacionRepo = new NotificacionDataAccess();
    }
    
    [TestMethod]
    public void AgregarNotificacion()
    {
        Notificacion notificacion = new Notificacion("esta es una notificacion de prueba");
        
        NotificacionRepo.Add(notificacion);
        
        Assert.IsTrue(NotificacionRepo.GetAll().Count()!=0);
        Assert.IsTrue(NotificacionRepo.GetAll().Contains(notificacion));
    }

    [TestMethod]
    public void EliminarNotificacionEliminaCorrectamente()
    {
        Notificacion notificacion = new Notificacion("esta es una notificacion de prueba");
        
        NotificacionRepo.Add(notificacion);
        
        Assert.IsTrue(NotificacionRepo.GetAll().Count()!=0);
        
        NotificacionRepo.Remove(notificacion);
        
        Assert.IsTrue(NotificacionRepo.GetAll().Count()==0);
        
    }
    
    [TestMethod]
    public void BuscarNotificacionPorIdDevuelveNotificacionCorrecta()
    {
        DB db = new DB();

        Notificacion notificacion = new Notificacion("Notificación de prueba");
        Notificacion notificacion2 = new Notificacion("Notificación de prueba");
        
        NotificacionRepo.Add(notificacion);
        NotificacionRepo.Add(notificacion2);

        Notificacion resultado= NotificacionRepo.GetById(notificacion.Id);
        
        Assert.AreEqual(resultado, notificacion);
    }
    
    [TestMethod]
    public void DevolverNotificacionesNoLeidasDeUnUsuario()
    {
        DB db = new DB();
        Notificacion notificacion = new Notificacion("Notificación de prueba");
        Notificacion notificacion2 = new Notificacion("Notificación de prueba");
        Usuario usuario1 = new Usuario("correo@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));

        NotificacionRepo.Add(notificacion);
        NotificacionRepo.Add(notificacion2);
        db.agregarUsuario(usuario1);
        notificacion.AgregarUsuario(usuario1);
        notificacion2.AgregarUsuario(usuario1);
        notificacion.MarcarComoVista(usuario1);
        
        List<Notificacion> noLeidas = NotificacionRepo.NotificacionesNoLeidas(usuario1);
        
        Assert.IsNotNull(noLeidas);
        Assert.AreEqual(notificacion2.Id, noLeidas[0].Id);
    }
}