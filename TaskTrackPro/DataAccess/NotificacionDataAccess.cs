using Domain;
using IDataAcces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class NotificacionDataAccess : IDataAccessNotificacion
{
    private readonly SqlContext _context;

    public NotificacionDataAccess(SqlContext context)
    {
        _context = context;
    }

    public void Add(Notificacion notificacion)
    {
        _context.Notificaciones.Add(notificacion);
        _context.SaveChanges();
    }

    public void Remove(Notificacion notificacion)
    {
        _context.Notificaciones.Remove(notificacion);
        _context.SaveChanges();
    }

    public Notificacion? GetById(int id)
    {
        return _context.Notificaciones
            .Include(n => n.UsuariosNotificados)
            .Include(n => n.VistaPorUsuarios)
            .FirstOrDefault(n => n.Id == id);
    }

    public List<Notificacion> GetAll()
    {
        return _context.Notificaciones
            .Include(n => n.UsuariosNotificados)
            .Include(n => n.VistaPorUsuarios)
            .ToList();
    }

    public List<Notificacion> NotificacionesNoLeidas(Usuario usuario)
    {
        return _context.Notificaciones
            .Include(n => n.UsuariosNotificados)
            .Include(n => n.VistaPorUsuarios)
            .Where(n =>
                n.UsuariosNotificados.Any(u => u.Id == usuario.Id) &&
                !n.VistaPorUsuarios.Any(u => u.Id == usuario.Id))
            .ToList();
    }
    
    public void Update(Notificacion n)
    {
        _context.SaveChanges();
    }
}