using Domain;
using Domain.Observers;
using IDataAcces;

namespace Services.Observers;

public class ActualizadorEstadoTareas : IRecursoObserver
{
    private readonly IDataAccessAsignacionRecursoTarea _asignacionRepo;
    private readonly IDataAccessTarea _tareasRepo;

    public ActualizadorEstadoTareas(IDataAccessAsignacionRecursoTarea asignacionRepo, IDataAccessTarea tareasRepo)
    {
        _asignacionRepo = asignacionRepo;
        _tareasRepo = tareasRepo;
    }

    public void ActualizarTareasDeRecurso(Recurso recurso)
    {
        List<AsignacionRecursoTarea> asignaciones = _asignacionRepo.GetByRecurso(recurso.Id);

        foreach (var asignacion in asignaciones)
        {
            Tarea tarea = asignacion.Tarea;
            
            tarea.ActualizarEstado(VerificarRecursosDisponibles(tarea.Id));
            _tareasRepo.Update(tarea);
        }
    }

    private bool VerificarRecursosDisponibles(int tareaId)
    {
        List<AsignacionRecursoTarea> asignacionesDeTarea = _asignacionRepo.GetByTarea(tareaId);
        foreach (AsignacionRecursoTarea asignacion in asignacionesDeTarea)
        {
            if (!asignacion.Recurso.EstaDisponible(asignacion.CantidadNecesaria)) return false;
        }
        return true;
    }
}
