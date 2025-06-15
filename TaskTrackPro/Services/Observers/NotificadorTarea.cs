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
            Notificacion notificacion = new Notificacion(mensaje);
            notificacion.AgregarUsuario(admin);
            _repoNotificaciones.Add(notificacion);
        }
        
        public void TareaAgregada(Proyecto proyecto, Tarea tareaAgregada)
        {
            Usuario admin = _repoAsignacionProyecto
                .GetAdminProyecto(proyecto.Id)
                .Usuario;
            string mensaje = 
                $"Se ha agregado la tarea '{tareaAgregada.Titulo}' al proyecto '{proyecto.Nombre}'.\n"+
            $"Esto puede cambiar la fecha de fin del proyecto!!!";
            Notificacion notificacion = new Notificacion(mensaje);
            notificacion.AgregarUsuario(admin);
            _repoNotificaciones.Add(notificacion);
        }
    }
}