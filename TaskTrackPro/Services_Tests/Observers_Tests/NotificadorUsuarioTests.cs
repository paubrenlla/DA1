using Domain;
using DTOs;
using IDataAcces;
using Moq;
using Services.Observers;

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
                Times.Once
                );
        }
    }
}