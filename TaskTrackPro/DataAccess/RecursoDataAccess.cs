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
        Recurso recurso = _context.Recursos.Find(id);
        if (recurso is null)
            throw new NullReferenceException("No existe el Recurso.");
        return recurso;
    }

    public List<Recurso> GetAll()
    {
        return _context.Recursos.AsNoTracking().ToList(); //Usamos AsNoTracking para no modificar nada en la DB
    }
    
    public void Update(Recurso recurso)
    {
        _context.Recursos.Update(recurso);
        _context.SaveChanges();
    }

    public void Remove(Recurso recurso)
    {
        _context.Recursos.Remove(recurso);
        _context.SaveChanges();
    }
}