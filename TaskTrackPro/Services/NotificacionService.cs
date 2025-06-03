using DTOs;
using Domain;
using Domain.Enums;
using IDataAcces;

namespace Services;

public class NotificacionService : INotificacionService
{
    private readonly IDataAccessNotificacion _notificacionRepo;
    public IDataAccessUsuario _UsuarioRepo;
    
    public NotificacionService(IDataAccessNotificacion notificacionRepo)
    {
        _notificacionRepo = notificacionRepo;
    }
    
    public void Add(NotificacionDTO dto)
    {
        Notificacion nueva = new Notificacion(dto.Mensaje);
        _notificacionRepo.Add(nueva);
    }

    public void Remove(NotificacionDTO notificacion)
    {
        Notificacion? notificacionExistente = _notificacionRepo.GetById(notificacion.Id);
        if (notificacionExistente != null) _notificacionRepo.Remove(notificacionExistente);
    }

    public NotificacionDTO? GetById(int id)
    {
        Notificacion notificacion = _notificacionRepo.GetById(id);
        return Convertidor.ANotificacionDTO(notificacion);
    }
    
    public List<NotificacionDTO?> GetAll()
    {
        return _notificacionRepo.GetAll()
            .Select(Convertidor.ANotificacionDTO)
            .ToList();
    }
    

    public List<NotificacionDTO?> NotificacionesNoLeidas(UsuarioDTO usuario)
    {
        Usuario buscado = _UsuarioRepo.GetById(usuario.Id); 
        List<Notificacion> notificaciones = _notificacionRepo.NotificacionesNoLeidas(buscado);
        return notificaciones.Select(Convertidor.ANotificacionDTO).ToList();
    }
    
    
}


