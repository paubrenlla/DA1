using Domain;
using Domain.Observers;
using IDataAcces;

namespace Services.Observers;

public class ActualizadorEstadoTareas : IRecursoObserver
{
    private readonly IDataAccessAsignacionRecursoTarea _asignacionRepo;

    public ActualizadorEstadoTareas(IDataAccessAsignacionRecursoTarea asignacionRepo)
    {
        _asignacionRepo = asignacionRepo;
    }

    public void ActualizarTareasDeRecurso(Recurso recurso)
    {
        List<AsignacionRecursoTarea> asignaciones = _asignacionRepo.GetByRecurso(recurso.Id);

        foreach (var asignacion in asignaciones)
        {
            Tarea tarea = asignacion.Tarea;
            tarea.ActualizarEstado();
        }
    }
}
