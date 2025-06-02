using Domain;
using DTOs;
using IDataAcces;

namespace Services;

public class AsignacionRecursoTareaService : IAsignacionRecursoTareaService
{
    private readonly IDataAccessRecurso _recursoRepo;
    private readonly IDataAccessTarea _tareaRepo;
    private readonly IDataAccessAsignacionRecursoTarea _asignacionRepo;

    public AsignacionRecursoTareaService(
        IDataAccessRecurso recursoRepo,
        IDataAccessTarea tareaRepo,
        IDataAccessAsignacionRecursoTarea asignacionRepo)
    {
        _recursoRepo = recursoRepo;
        _tareaRepo = tareaRepo;
        _asignacionRepo = asignacionRepo;
    }


    public AsignacionRecursoTareaDTO GetById(int id)
    {
        AsignacionRecursoTarea asignacionRecursoTarea = _asignacionRepo.GetById(id);
        return Convertidor.AAsignacionRecursoTareaDTO(asignacionRecursoTarea);
    }

    public List<AsignacionRecursoTareaDTO> GetAll()
    {
        return _asignacionRepo.GetAll()
            .Select(Convertidor.AAsignacionRecursoTareaDTO)
            .ToList();
    }

    public AsignacionRecursoTareaDTO CrearAsignacionRecursoTarea(AsignacionRecursoTareaDTO dto)
    {
        Recurso recurso = new Recurso(dto.Recurso.Nombre, dto.Recurso.Tipo, dto.Recurso.Descripcion,
            dto.Recurso.SePuedeCompartir, dto.Recurso.CantidadDelRecurso);
        
        Tarea tarea = new Tarea(dto.Tarea.Titulo, dto.Tarea.Descripcion, dto.Tarea.FechaInicio, dto.Tarea.Duracion,
            dto.Tarea.EsCritica);
        
        AsignacionRecursoTarea  asignacionRecursoTarea = new AsignacionRecursoTarea(recurso, tarea, dto.Cantidad);
        _asignacionRepo.Add(asignacionRecursoTarea);
        
        return Convertidor.AAsignacionRecursoTareaDTO(asignacionRecursoTarea);
    }

    public void EliminarRecursoDeTarea(int idTarea)
    {
        AsignacionRecursoTarea asignacionRecursoTarea = _asignacionRepo.GetById(idTarea);
        Tarea tarea = asignacionRecursoTarea.Tarea;
        _asignacionRepo.Remove(asignacionRecursoTarea);
        tarea.ActualizarEstado();
    }

    public void ModificarAsignacion(AsignacionRecursoTareaDTO dto)
    {
        AsignacionRecursoTarea asignacionRecursoTarea = _asignacionRepo.GetById(dto.Id);
        asignacionRecursoTarea.Modificar(dto.Cantidad);
    }

    public List<RecursoDTO> RecursosDeLaTarea(int tareaId)
    {
        Tarea tarea = _tareaRepo.GetById(tareaId);
        List<AsignacionRecursoTarea> asignacionesFiltradas = _asignacionRepo.GetByTarea(tarea);
        
        List<RecursoDTO> recursosDTO = asignacionesFiltradas.Select(a => a.Recurso)
            .Select(Convertidor.ARecursoDTO)
            .ToList();
        return recursosDTO;
    }

    public void EliminarRecursosDeTarea(int tareaId)
    {
        Tarea tarea = _tareaRepo.GetById(tareaId);
        List<AsignacionRecursoTarea> asignacionesFiltradas = _asignacionRepo.GetByTarea(tarea);

        foreach (var asignacion in asignacionesFiltradas.ToList())
        {
            _asignacionRepo.Remove(asignacion);
        }
        tarea.ActualizarEstado();
    }

    public void ActualizarEstadoDeTareasConRecurso(int recursoID)
    {
        Recurso recurso = _recursoRepo.GetById(recursoID);
        List<Tarea> tareas = _asignacionRepo.GetByRecurso(recurso).Select(a => a.Tarea).ToList();
        foreach (Tarea tarea in tareas)
        { 
            tarea.ActualizarEstado(VerificarRecursosDeTareaDisponibles(recursoID));
        }
    }

    public bool RecursoEsExclusivo(int recursoID)
    {
        Recurso recurso = _recursoRepo.GetById(recursoID);
        List<AsignacionRecursoTarea> tareasFiltradas = _asignacionRepo.GetByRecurso(recurso);
        return tareasFiltradas.Count() > 1;
    }

    public bool VerificarRecursosDeTareaDisponibles(int TareaId)
    {
        Tarea tarea = _tareaRepo.GetById(TareaId);
        List<AsignacionRecursoTarea> asignacionesDeTarea = _asignacionRepo.GetByTarea(tarea);
        foreach (AsignacionRecursoTarea asignacion in asignacionesDeTarea)
        {
            if (!asignacion.Recurso.EstaDisponible(asignacion.CantidadNecesaria)) return false;
        }
        
        return true;
    }
}