using Domain;
using IDataAcces;
using Moq;
using Services.Observers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Services_Tests.Observers_Tests
{
    [TestClass]
    public class NotificadorUsuarioTests
    {
        private readonly Usuario _usuarioEjemplo = new Usuario(
            email: "test@example.com",
            nombre: "Test",
            apellido: "User",
            pwd: "Contraseña1!",
            fechaNacimiento: new DateTime(1990, 1, 1)
        );

        [TestMethod]
        public void CambioContraseña_CreaNotificacionConMensajeCorrecto_Y_AsociaUsuario()
        {
            var mockNotificacionRepo = new Mock<IDataAccessNotificacion>();
            var observer = new NotificadorUsuario(mockNotificacionRepo.Object);

            string nuevaPwd = "nuevaPwd456";
            string esperadoMensaje = $"Tu nueva contraseña es: {nuevaPwd}.";

            observer.CambioContraseña(_usuarioEjemplo, nuevaPwd);

            mockNotificacionRepo.Verify(repo => repo.Add(
                    It.Is<Notificacion>(n =>
                        n.Mensaje == esperadoMensaje
                        && n.UsuariosNotificados.Contains(_usuarioEjemplo)
                    )),
                Times.Once,
                "Se esperaba que se creara una Notificación con el mensaje de nueva contraseña y se asociara el usuario."
            );
        }

        [TestMethod]
        public void ConvertidoEnAdmin_CreaNotificacionConMensajeCorrecto_Y_AsociaUsuario()
        {
            var mockNotificacionRepo = new Mock<IDataAccessNotificacion>();
            var observer = new NotificadorUsuario(mockNotificacionRepo.Object);

            string esperadoMensaje =
                "Felicidades, ahora eres Admin del sistema!\n" +
                "Recuerda, un gran poder conlleva una gran responsabilidad.";

            observer.ConvertidoEnAdmin(_usuarioEjemplo);

            mockNotificacionRepo.Verify(repo => repo.Add(
                    It.Is<Notificacion>(n =>
                        n.Mensaje == esperadoMensaje
                        && n.UsuariosNotificados.Contains(_usuarioEjemplo)
                    )),
                Times.Once
            );
        }
    }
}
