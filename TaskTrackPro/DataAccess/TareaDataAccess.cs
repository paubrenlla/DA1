using System.Net.Sockets;
using Domain;
using IDataAcces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class TareaDataAccess : IDataAccessTarea
{
    private readonly SqlContext _context;        
    
    public TareaDataAccess(SqlContext context)
    {
        _context = context;
    }

    public void Add(Tarea tarea)
    {
        _context.Tareas.Add(tarea);
        _context.SaveChanges();
    }

    public void Remove(Tarea tarea)
    {
        _context.Tareas.Remove(tarea);    
        _context.SaveChanges();
    }

    public void Update(Tarea tarea)
    {
        _context.Update(tarea);
        _context.SaveChanges();
    }

    public Tarea? GetById(int Id)
    {
        Tarea tarea = _context.Tareas.Find(Id);
        if(tarea is null)
            throw new ArgumentException("No se encontro la asigancion de proyecto");
        return tarea;

    }

    public List<Tarea> GetAll()
    {
        return _context.Tareas.AsNoTracking().ToList();
    }
}