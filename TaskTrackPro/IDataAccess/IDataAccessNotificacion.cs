using Domain;

namespace IDataAcces;

public interface IDataAccessNotificacion : IDataAccessGeneric<Notificacion>
{
    public List<Notificacion> NotificacionesNoLeidas(Usuario usuario);

}