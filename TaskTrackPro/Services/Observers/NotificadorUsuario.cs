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
            Notificacion notificacion = new Notificacion(
                mensaje: $"Tu nueva contraseña es: {contraseña}."
            );
            notificacion.UsuariosNotificados.Add(usuario);

            _notificacionRepo.Add(notificacion);
        }

        public void ConvertidoEnAdmin(Usuario usuario)
        {
            Notificacion notificacion = new Notificacion(
                mensaje: $"Felicidades, ahora eres Admin del sistema!\n" +
                         $"Recuerda, un gran poder conlleva una gran responsabilidad.");
            notificacion.UsuariosNotificados.Add(usuario);

            _notificacionRepo.Add(notificacion);
        }
    }
}