using Domain;
using Domain.Observers;
using IDataAcces;

namespace Services.Observers
{
    public class NotificadorUsuario : IUsuarioObserver
    {
        private readonly IDataAccessNotificacion _notificacionRepo;

        public NotificadorUsuario(IDataAccessNotificacion notificacionRepo)
        {
            _notificacionRepo = notificacionRepo;
        }

        public void CambioContraseña(Usuario usuario, string contraseña)
        {
            var notificacion = new Notificacion(
                mensaje: $"Tu nueva contraseña es: {contraseña}."
            );
            notificacion.UsuariosNotificados.Add(usuario);

            _notificacionRepo.Add(notificacion);
        }
    }
}