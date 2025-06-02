using Domain;
using IDataAcces;

namespace DataAccess;

public class AsignacionRecursoTareaDataAccess : IDataAccessAsignacionRecursoTarea
{
    private List<AsignacionRecursoTarea> _asignacionesRecursoTareas;
    
    public AsignacionRecursoTareaDataAccess()
    {
        _asignacionesRecursoTareas = new List<AsignacionRecursoTarea>();
    }
    public void Add(AsignacionRecursoTarea asignacionRecursoTarea)
    {
        _asignacionesRecursoTareas.Add(asignacionRecursoTarea);
    }

    public void Remove(AsignacionRecursoTarea asignacionProyecto)
    {
        _asignacionesRecursoTareas.Remove(asignacionProyecto);
    }
    public AsignacionRecursoTarea? GetById(int Id)
    {
        return _asignacionesRecursoTareas.FirstOrDefault(a => a.Id == Id);
    }

    public List<AsignacionRecursoTarea> GetAll()
    {
        return _asignacionesRecursoTareas;
    }

    public List<AsignacionRecursoTarea> GetByTarea(Tarea tarea)
    {
        return _asignacionesRecursoTareas.FindAll(a => a.Tarea == tarea);
    }

    public List<AsignacionRecursoTarea> GetByRecurso(Recurso recurso)
    {
        return _asignacionesRecursoTareas.FindAll(a => a.Recurso == recurso);
    }

    public int CantidadDelRecurso(AsignacionRecursoTarea asignacionRecursoTarea)
    {
        return GetById(asignacionRecursoTarea.Id).CantidadNecesaria;
    }

    public AsignacionRecursoTarea? GetByRecursoYTarea(Recurso recurso, Tarea tarea)
    {
        AsignacionRecursoTarea? asignacion = _asignacionesRecursoTareas.Find(a => a.Recurso == recurso && a.Tarea == tarea);
        return asignacion;
    }
    
    
}