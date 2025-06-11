using Domain;
using IDataAcces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class RecursoDataAccess :IDataAccessRecurso
{
    private readonly SqlContext _context;
    
    public RecursoDataAccess(SqlContext context)
    {
        _context = context;
    }
    public void Add(Recurso recurso)
    {
        if (_context.Recursos.Any(r => r.Nombre == recurso.Nombre))
            throw new ArgumentException("El recurso ya existe en el sistema.");

        _context.Recursos.Add(recurso);
        _context.SaveChanges();
    }


    public Recurso? GetById(int id)
    {
        return _context.Recursos.Find(id);
    }

    public List<Recurso> GetAll()
    {
        return _context.Recursos.AsNoTracking().ToList(); //Usamos AsNoTracking para no modificar nada en la DB
    }

    public void Remove(Recurso recurso)
    {
        if (recurso.EstaEnUso())
            throw new ArgumentException("No se puede eliminar un recurso que está en uso.");

        _context.Recursos.Remove(recurso);
        _context.SaveChanges();
    }
}