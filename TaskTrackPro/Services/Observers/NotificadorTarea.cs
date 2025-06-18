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
            string mensaje = $"La tarea '{tareaEliminada.Titulo}' fue eliminada del proyecto '{proyecto.Nombre}'.\n" +
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

        public void ModificacionDependencias(Proyecto proyecto, Tarea tareaModificacion)
        {
            Usuario admin = _repoAsignacionProyecto
                .GetAdminProyecto(proyecto.Id)
                .Usuario;
            string mensaje = 
                $"Se ha modificado una dependencia en la tarea '{tareaModificacion.Titulo}' en el proyecto '{proyecto.Nombre}'.\n"+
                $"Es muy probable que la fecha de fin del proyecto haya cambiado!!!";
            Notificacion notificacion = new Notificacion(mensaje);
            notificacion.AgregarUsuario(admin);
            _repoNotificaciones.Add(notificacion);
        }

        public void TareaModificada(Proyecto proyecto, Tarea tareaModificada)
        {
            Usuario admin = _repoAsignacionProyecto
                .GetAdminProyecto(proyecto.Id)
                .Usuario;
            string mensaje = 
                $"Se ha modificado la fecha de inicio en la tarea '{tareaModificada.Titulo}' en el proyecto '{proyecto.Nombre}'.\n"+
                $"Esto puede afectar a la fecha de fin del proyecto!";
            Notificacion notificacion = new Notificacion(mensaje);
            notificacion.AgregarUsuario(admin);
            _repoNotificaciones.Add(notificacion);
        }

        public void SeForzaronRecursos(Proyecto proyecto, Tarea tarea)
        {
            Usuario admin = _repoAsignacionProyecto
                .GetAdminProyecto(proyecto.Id)
                .Usuario;
            string mensaje = 
                $"Se usarán los recursos ya asignados a la tarea '{tarea.Titulo}' en el proyecto '{proyecto.Nombre}'.\n"+
                $"Esto atrasará el comienzo de la tarea";
            Notificacion notificacion = new Notificacion(mensaje);
            notificacion.AgregarUsuario(admin);
            _repoNotificaciones.Add(notificacion);
        }
    }
}