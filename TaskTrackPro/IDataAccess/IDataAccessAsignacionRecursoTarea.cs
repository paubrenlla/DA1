using Domain;

namespace IDataAcces;

public interface IDataAccessAsignacionRecursoTarea: IDataAccessGeneric<AsignacionRecursoTarea>
{
    public void Add(AsignacionRecursoTarea asignacionRecursoTarea);

    public void Remove(AsignacionRecursoTarea data);

    public AsignacionRecursoTarea? GetById(int Id);

    public List<AsignacionRecursoTarea> GetAll();
    
    public List<AsignacionRecursoTarea> GetByTarea(int idTarea);
    
    public List<AsignacionRecursoTarea> GetByRecurso(int idRecurso);
    
    public int CantidadDelRecurso(AsignacionRecursoTarea asignacionRecursoTarea);
    
    public AsignacionRecursoTarea? GetByRecursoYTarea(int idRecurso, int idTarea);
    void Update(AsignacionRecursoTarea asignacion);
}