using Domain;
using Domain.Observers;
using IDataAcces;

namespace Services.Observers
{
    public class NotificadorTarea : ITareaObserver
    {
        private readonly IDataAccessNotificacion _repoNotificaciones;
        private readonly IDataAccessAsignacionProyecto _repoAsignacionProyecto;

        public NotificadorTarea(IDataAccessNotificacion repoNotificaciones, IDataAccessAsignacionProyecto repoAsignacionProyecto)
        {
            _repoNotificaciones = repoNotificaciones;
            _repoAsignacionProyecto = repoAsignacionProyecto;
        }

        public void TareaEliminada(Proyecto proyecto, Tarea tareaEliminada)
        {
            Usuario admin = _repoAsignacionProyecto.GetAdminProyecto(proyecto.Id).Usuario;
            var mensaje = $"La tarea '{tareaEliminada.Titulo}' fue eliminada del proyecto '{proyecto.Nombre}'.\n" +
                          $"Esto puede cambiar la fecha de fin del proyecto!!!";
            var notificacion = new Notificacion(mensaje);
            notificacion.AgregarUsuario(admin);
            _repoNotificaciones.Add(notificacion);
        }
    }
}