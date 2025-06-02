using Domain;

namespace IDataAcces;

public interface IDataAccessAsignacionRecursoTarea: IDataAccessGeneric<AsignacionRecursoTarea>
{
    public void Add(AsignacionRecursoTarea asignacionRecursoTarea);

    public void Remove(AsignacionRecursoTarea data);

    public AsignacionRecursoTarea? GetById(int Id);

    public List<AsignacionRecursoTarea> GetAll();
    
    public List<AsignacionRecursoTarea> GetByTarea(Tarea tarea);
    
    public List<AsignacionRecursoTarea> GetByRecurso(Recurso recurso);
    
    public int CantidadDelRecurso(AsignacionRecursoTarea asignacionRecursoTarea);
    
    public AsignacionRecursoTarea? GetByRecursoYTarea(Recurso recurso, Tarea tarea);
}