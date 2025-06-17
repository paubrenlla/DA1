using DTOs;
using Services;

namespace Controllers;

public class AsignacionRecursoTareaControllers
{
    private readonly IAsignacionRecursoTareaService _service;
    
    public AsignacionRecursoTareaControllers(IAsignacionRecursoTareaService service)
    {
        _service = service;
    }

    public List<AsignacionRecursoTareaDTO> GetAll()
    {
        return _service.GetAll();
    }

    public AsignacionRecursoTareaDTO GetById(int id)
    {
        return _service.GetById(id);
    }

    public AsignacionRecursoTareaDTO CrearAsignacionRecursoTarea(AsignacionRecursoTareaDTO dto)
    {
        return _service.CrearAsignacionRecursoTarea(dto);
    }

    public void EliminarRecursoDeTarea(int idTarea, int idRecurso)
    {
        _service.EliminarRecursoDeTarea(idTarea, idRecurso);
    }

    public void ModificarAsignacion(AsignacionRecursoTareaDTO dto)
    {
        _service.ModificarAsignacion(dto);
    }

    public List<RecursoDTO> RecursosDeLaTarea(int tareaId)
    {
        return _service.RecursosDeLaTarea(tareaId);
    }

    public void EliminarRecursosDeTarea(int tareaId)
    {
        _service.EliminarRecursosDeTarea(tareaId);
    }

    public void ActualizarEstadoDeTareasConRecurso(int recursoID)
    {
        _service.ActualizarEstadoDeTareasConRecurso(recursoID);
    }

    public bool RecursoEsExclusivo(int recursoID)
    {
        return _service.RecursoEsExclusivo(recursoID);
    }

    public bool VerificarRecursosDeTareaDisponibles(int TareaId)
    {
        return _service.VerificarRecursosDeTareaDisponibles(TareaId);
    }

    public List<AsignacionRecursoTareaDTO>? GetAsignacionesDeTarea(int idTarea)
    {
        return _service.GetAsignacionesDeTarea(idTarea);
    }

    public List<AsignacionRecursoTareaDTO> ObtenerAsignacionesRecursoEnFecha(int recursoId, DateTime? fechaSeleccionada)
    {
        return _service.ObtenerAsignacionesRecursoEnFecha(recursoId, fechaSeleccionada);
    }
}