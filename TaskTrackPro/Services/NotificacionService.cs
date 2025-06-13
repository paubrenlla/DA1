using DTOs;
using Domain;
using IDataAcces;

namespace Services;

public class NotificacionService : INotificacionService
{
    private readonly IDataAccessNotificacion _notificacionRepo;
    public IDataAccessUsuario _usuarioRepo;
    
    public NotificacionService(IDataAccessNotificacion notificacionRepo, IDataAccessUsuario usuarioRepo)
    {
        _notificacionRepo = notificacionRepo;
        _usuarioRepo = usuarioRepo;
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
        Usuario buscado = _usuarioRepo.GetById(usuario.Id); 
        List<Notificacion> notificaciones = _notificacionRepo.NotificacionesNoLeidas(buscado);
        return notificaciones.Select(Convertidor.ANotificacionDTO).ToList();
    }
    
    public void MarcarLeida(int notificacionId, int usuarioId)
    {
        var notificacion = _notificacionRepo.GetById(notificacionId);
        if(notificacion is null)
            throw new ArgumentException("Notificación no encontrada", nameof(notificacionId));

        var usuario = _usuarioRepo.GetById(usuarioId);
        if(usuario is null)
            throw new ArgumentException("Usuario no encontrado", nameof(usuarioId));

        if (!notificacion.VistaPorUsuarios.Any(u => u.Id == usuarioId))
        {
            notificacion.VistaPorUsuarios.Add(usuario);
            _notificacionRepo.Update(notificacion);
        }
    }
}


