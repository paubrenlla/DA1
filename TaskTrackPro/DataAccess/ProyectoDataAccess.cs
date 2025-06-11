using Domain;
using IDataAcces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ProyectoDataAccess :IDataAccessProyecto
{
    private readonly SqlContext _context;
    
    public ProyectoDataAccess(SqlContext context)
    {
        _context = context;
    }
    public void Add(Proyecto proyecto)
    {
        if (_context.Proyectos.Any(p => p.Id == proyecto.Id))
            throw new ArgumentException("El Proyecto ya existe en el sistema.");

        _context.Proyectos.Add(proyecto);
        _context.SaveChanges();
    }
    
    public void Remove(Proyecto proyecto)
    {
       _context.Remove(proyecto);
       _context.SaveChanges();
    }
    
    public Proyecto GetById(int id)
    {
       Proyecto proyecto = _context.Proyectos.FirstOrDefault(p => p.Id == id);
       if (proyecto is null)
           throw new ArgumentException(nameof(id), "El proyecto no existe.");
       return proyecto;
    }

    public List<Proyecto> GetAll()
    {
        return _context.Proyectos.AsNoTracking().ToList();
    }

    public void Update(Proyecto proyecto)
    {
        _context.Update(proyecto);
        _context.SaveChanges();
    }
}