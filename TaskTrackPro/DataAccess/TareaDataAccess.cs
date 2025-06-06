using System.Net.Sockets;
using Domain;
using IDataAcces;

namespace DataAccess;

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
        Tarea tarea = _listaTareas.FirstOrDefault(t => t.Id == Id);
        if (tarea == null) 
            throw new ArgumentException("Tarea no encontrada");
        return tarea;

    }

    public List<Tarea> GetAll()
    {
        return _listaTareas;
    }

    public List<Tarea> GetTareasDeUsuarioEnProyecto(int miembroId, int proyectoId)
    {
        return _listaTareas
            .Where(t => t.Proyecto.Id == proyectoId)
            .Where(t => t.UsuariosAsignados
                .FirstOrDefault(u => u.Id == miembroId) != null)
            .ToList();
    }
}