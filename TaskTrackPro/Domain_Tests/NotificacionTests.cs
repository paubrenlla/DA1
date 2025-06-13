using System.Reflection;
using Domain;

namespace Domain_Tests;

[TestClass]
public class NotificacionTests
{
    [TestInitialize]
    public void Setup()
    {
        typeof(Notificacion)
            .GetField("_contadorId", BindingFlags.Static | BindingFlags.NonPublic)
            ?.SetValue(null, 1);
    }  
    
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

  [TestMethod]
  public void UsuarioMarcaComoVistaUnaNotificacion()
  {
      string mensaje = "Esta es una notifcación de prueba";
      Notificacion notificacion = new Notificacion(mensaje);
      
      Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
      Usuario u2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
      notificacion.AgregarUsuario(u);
      notificacion.AgregarUsuario(u2);

      notificacion.MarcarComoVista(u2);
      
      Assert.AreEqual(1,notificacion.VistaPorUsuarios.Count);
  }

  [TestMethod]
  public void AgregarUsuarios_DeberiaAgregarTodosLosUsuariosALaLista()
  {
      Notificacion notificacion = new Notificacion("Nuevo mensaje");
      Usuario usuario1 = new Usuario("juan@gmail.com", "Juan", "Perez",  "EsValida1!", new DateTime(2000, 01, 01));
      Usuario usuario2 = new Usuario("ana@gmail.com", "Ana", "Perez",  "EsValida1!", new DateTime(2000, 01, 01));
      List<Usuario> usuarios = new List<Usuario> { usuario1, usuario2 };

      notificacion.AgregarUsuarios(usuarios);

      CollectionAssert.Contains(notificacion.UsuariosNotificados, usuario1);
      CollectionAssert.Contains(notificacion.UsuariosNotificados, usuario2);
      Assert.AreEqual(2, notificacion.UsuariosNotificados.Count);
  }

  [TestMethod]
  public void AgregarUsuarios_NoDeberiaAgregarNada_SiLaListaEsVacia()
  {
      Notificacion notificacion = new Notificacion("Mensaje vacío");
      List<Usuario> usuarios = new List<Usuario>();

      notificacion.AgregarUsuarios(usuarios);

      Assert.AreEqual(0, notificacion.UsuariosNotificados.Count);
  }

  [TestMethod]
  public void AgregarUsuarios_DeberiaAgregarDuplicados_SiHayUsuariosRepetidos()
  {
      Notificacion notificacion = new Notificacion("Mensaje duplicado");
      Usuario usuario = new Usuario("juan@gmail.com", "Juan", "Perez",  "EsValida1!", new DateTime(2000, 01, 01));
      List<Usuario> usuarios = new List<Usuario> { usuario, usuario };

      notificacion.AgregarUsuarios(usuarios);

      Assert.AreEqual(2, notificacion.UsuariosNotificados.Count);
      Assert.AreSame(notificacion.UsuariosNotificados[0], notificacion.UsuariosNotificados[1]);
  }
}