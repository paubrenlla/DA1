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

    public List<AsignacionRecursoTarea> GetByTarea(int idTarea)
    {
        return _asignacionesRecursoTareas.FindAll(a => a.Tarea.Id == idTarea);
    }

    public List<AsignacionRecursoTarea> GetByRecurso(int idRecurso)
    {
        return _asignacionesRecursoTareas.FindAll(a => a.Recurso.Id == idRecurso);
    }

    public int CantidadDelRecurso(AsignacionRecursoTarea asignacionRecursoTarea)
    {
        return GetById(asignacionRecursoTarea.Id).CantidadNecesaria;
    }

    public AsignacionRecursoTarea? GetByRecursoYTarea(int idRecurso, int idTarea)
    {
        AsignacionRecursoTarea? asignacion = _asignacionesRecursoTareas.Find(a => a.Recurso.Id == idRecurso && a.Tarea.Id == idTarea);
        return asignacion;
    }
    
    
}