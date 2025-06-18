using Domain;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Tests;

[TestClass]
public class NotificacionDataAccess_Tests
{
    private SqlContext _context;
    private NotificacionDataAccess notificacionRepo;
    
    [TestInitialize]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new SqlContext(options);
        notificacionRepo = new NotificacionDataAccess(_context);
    }
    
    [TestMethod]
    public void AgregarNotificacion()
    {
        Notificacion notificacion = new Notificacion("esta es una notificacion de prueba");
        
        notificacionRepo.Add(notificacion);
        
        Assert.IsTrue(notificacionRepo.GetAll().Count()!=0);
        Assert.IsTrue(notificacionRepo.GetAll()[0].Id==notificacion.Id);
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

        Notificacion notificacion = new Notificacion("Notificación de prueba");
        Notificacion notificacion2 = new Notificacion("Notificación de prueba");
        
        notificacionRepo.Add(notificacion);
        notificacionRepo.Add(notificacion2);

        Notificacion resultado= notificacionRepo.GetById(notificacion.Id);
        
        Assert.AreEqual(resultado.Id, notificacion.Id);
    }
    
    [TestMethod]
    public void DevolverNotificacionesNoLeidasDeUnUsuario()
    {
        Notificacion notificacion = new Notificacion("Notificación de prueba");
        Notificacion notificacion2 = new Notificacion("Notificación de prueba");
        Usuario usuario1 = new Usuario("correo@gmail.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        _context.Usuarios.Add(usuario1);
        _context.SaveChanges();
        notificacionRepo.Add(notificacion);
        notificacionRepo.Add(notificacion2);
        notificacion.AgregarUsuario(usuario1);
        notificacion2.AgregarUsuario(usuario1);
        notificacion.MarcarComoVista(usuario1);
        _context.SaveChanges();
        
        List<Notificacion> noLeidas = notificacionRepo.NotificacionesNoLeidas(usuario1);
        
        Assert.IsNotNull(noLeidas);
        Assert.AreEqual(notificacion2.Id, noLeidas[0].Id);
    }
}