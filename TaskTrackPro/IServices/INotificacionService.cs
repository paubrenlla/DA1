using DTOs;

namespace IServices;

public interface INotificacionService
{
    void Add(NotificacionDTO notificacion);
    void Remove(NotificacionDTO notificacion);
    NotificacionDTO? GetById(int id);
    List<NotificacionDTO?> GetAll();
    List<NotificacionDTO?> NotificacionesNoLeidas(UsuarioDTO usuario);
    
    public void MarcarLeida(int notificacionId, int usuarioId);
}


