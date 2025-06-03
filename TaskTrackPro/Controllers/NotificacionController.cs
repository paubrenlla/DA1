using Domain;
using Services;
using DTOs;

namespace Controllers;

public class NotificacionController
{
    private readonly INotificacionService _service;

    public NotificacionController(INotificacionService service)
    {
        _service = service;
    }
    
    public void Add(NotificacionDTO notificacion)
    {
        _service.Add(notificacion);
    }
    
    public void Remove(NotificacionDTO notificacion)
    {
        _service.Remove(notificacion);
    }
    
    public NotificacionDTO? GetById(int id)
    {
        return _service.GetById(id);
    }

    public List<NotificacionDTO> GetAll()
    {
        return _service.GetAll();
    }

    public List<NotificacionDTO> NotificacionesNoLeidas(UsuarioDTO usuario)
    {
        return _service.NotificacionesNoLeidas(usuario);
    }
    
    
    
    
    
    
    
}