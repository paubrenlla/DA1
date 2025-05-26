using System.Net.Sockets;
using BusinessLogic;
using IDataAcces;

namespace Repositorios;

public class TareaDataAccess : IDataAccessTarea
{
    private List<Tarea> _listaTareas;
    
    public TareaDataAccess()
    {
        _listaTareas = new List<Tarea>();
    }

    public void Add(Tarea tarea)
    {
        _listaTareas.Add(tarea);
    }

    public void Remove(Tarea data)
    {
        _listaTareas.Remove(data);
    }

    public Tarea? GetById(int Id)
    {
        return _listaTareas.FirstOrDefault(t => t.Id == Id);
    }

    public List<Tarea> GetAll()
    {
        return _listaTareas;
    }
    
}