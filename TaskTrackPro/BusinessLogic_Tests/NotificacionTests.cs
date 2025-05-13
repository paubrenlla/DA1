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
  
}