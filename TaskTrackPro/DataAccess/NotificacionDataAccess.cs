using BusinessLogic;
using IDataAcces;

namespace Repositorios;

public class NotificacionDataAccess : IDataAccessNotificacion
{
    private List<Notificacion> _listaNotificaciones;

    public NotificacionDataAccess()
    {
        _listaNotificaciones = new List<Notificacion>();
    }

    public void Add(Notificacion notificacion)
    {
        _listaNotificaciones.Add(notificacion);
    }

    public void Remove(Notificacion notificacion)
    {
        _listaNotificaciones.Remove(notificacion);
    }

    public Notificacion GetById(int id)
    {
        return _listaNotificaciones.FirstOrDefault(n => n.Id == id);
    }

    public List<Notificacion> GetAll()
    {
        return _listaNotificaciones;
    }

    public List<Notificacion> NotificacionesNoLeidas(Usuario usuario)
    {
        return _listaNotificaciones
            .Where(n => n.UsuariosNotificados.Contains(usuario) &&
                        !n.VistaPorUsuarios.Contains(usuario))
            .ToList(); 
    }
}