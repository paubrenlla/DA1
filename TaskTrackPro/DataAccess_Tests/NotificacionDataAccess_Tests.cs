using BusinessLogic;
using Repositorios;

namespace DataAccess_Tests;

[TestClass]
public class NotificacionDataAccess_Tests
{
    private NotificacionDataAccess notificacionRepo;
    
    [TestInitialize]
    public void SetUp()
    {
        notificacionRepo = new NotificacionDataAccess();
    }
    
    [TestMethod]
    public void AgregarNotificacion()
    {
        Notificacion notificacion = new Notificacion("esta es una notificacion de prueba");
        
        notificacionRepo.Add(notificacion);
        
        Assert.IsTrue(notificacionRepo.GetAll().Count()!=0);
        Assert.IsTrue(notificacionRepo.GetAll().Contains(notificacion));
    }

    [TestMethod]
    public void EliminarNotificacionEliminaCorrectamente()
    {
        Notificacion notificacion = new Notificacion("esta es una notificacion de prueba");
        
        notificacionRepo.Add(notificacion);
        
        Assert.IsTrue(notificacionRepo.GetAll().Count()!=0);
        
        notificacionRepo.Remove(notificacion);
        
        Assert.IsTrue(notificacionRepo.GetAll().Count()==0);
        
    }
    
    [TestMethod]
    public void BuscarNotificacionPorIdDevuelveNotificacionCorrecta()
    {
        DB db = new DB();

        Notificacion notificacion = new Notificacion("Notificación de prueba");
        Notificacion notificacion2 = new Notificacion("Notificación de prueba");
        
        notificacionRepo.Add(notificacion);
        notificacionRepo.Add(notificacion2);

        Notificacion resultado= notificacionRepo.GetById(notificacion.Id);
        
        Assert.AreEqual(resultado, notificacion);
    }
    
    [TestMethod]
    public void DevolverNotificacionesNoLeidasDeUnUsuario()
    {
        Notificacion notificacion = new Notificacion("Notificación de prueba");
        Notificacion notificacion2 = new Notificacion("Notificación de prueba");
        Usuario usuario1 = new Usuario("correo@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));

        notificacionRepo.Add(notificacion);
        notificacionRepo.Add(notificacion2);
        notificacion.AgregarUsuario(usuario1);
        notificacion2.AgregarUsuario(usuario1);
        notificacion.MarcarComoVista(usuario1);
        
        List<Notificacion> noLeidas = notificacionRepo.NotificacionesNoLeidas(usuario1);
        
        Assert.IsNotNull(noLeidas);
        Assert.AreEqual(notificacion2.Id, noLeidas[0].Id);
    }
}