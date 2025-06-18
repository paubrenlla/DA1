using Domain;

namespace IDataAcces;

public interface IDataAccessNotificacion : IDataAccessGeneric<Notificacion>
{
    public void Update(Notificacion notificacion);
    public List<Notificacion> NotificacionesNoLeidas(Usuario usuario);

}