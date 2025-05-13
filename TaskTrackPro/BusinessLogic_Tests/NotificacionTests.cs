using BusinessLogic;

namespace BusinessLogic_Tests;

[TestClass]
public class NotificacionTests
{
  [TestMethod]
  public void CreoNotificacionCorrectamente()
  {
      string mensaje = "Esta es una notifcación de prueba";
      Notificacion notificacion = new Notificacion(mensaje);
      Assert.AreEqual(mensaje, notificacion.Mensaje);
  }
  
  [TestMethod]
  public void AgregeUsuariosANotificacion()
  {
      string mensaje = "Esta es una notifcación de prueba";
      Notificacion notificacion = new Notificacion(mensaje);
      
      Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
      Usuario u2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
      notificacion.AgregarUsuario(u);
      notificacion.AgregarUsuario(u2);

      Assert.AreEqual(2, notificacion.UsuariosNotificados.Count);
      Assert.IsTrue(notificacion.UsuariosNotificados.Contains(u));
      Assert.IsTrue(notificacion.UsuariosNotificados.Contains(u2));
  }

}