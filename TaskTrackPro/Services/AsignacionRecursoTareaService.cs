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
        AsignacionRecursoTarea? existeAsignacion = _asignacionRepo.GetByRecursoYTarea(dto.Recurso.Id, dto.Tarea.Id);

        if (existeAsignacion == null)
        {
            return CrearAsignacionSiNoExiste(dto);
        }

        if (dto.Recurso.CantidadDelRecurso < existeAsignacion.CantidadNecesaria + dto.Cantidad)
        {
            throw new ArgumentOutOfRangeException(nameof(dto.Cantidad), "No hay suficiente cantidad del recurso disponible.");
        }
        
        return ActualizarAsignacion(existeAsignacion, dto);
    }

    private AsignacionRecursoTareaDTO CrearAsignacionSiNoExiste(AsignacionRecursoTareaDTO dto)
    {
        Recurso recurso = _recursoRepo.GetById(dto.Recurso.Id);
        Tarea tarea = _tareaRepo.GetById(dto.Tarea.Id);
        AsignacionRecursoTarea  asignacionRecursoTarea = new AsignacionRecursoTarea(recurso, tarea, dto.Cantidad);
        
        _asignacionRepo.Add(asignacionRecursoTarea);
            
        tarea.ActualizarEstado(VerificarRecursosDeTareaDisponibles(tarea.Id));
            
        return Convertidor.AAsignacionRecursoTareaDTO(asignacionRecursoTarea);
    }

    private AsignacionRecursoTareaDTO ActualizarAsignacion(AsignacionRecursoTarea asignacion,  AsignacionRecursoTareaDTO dto)
    {
        asignacion.CantidadNecesaria += dto.Cantidad;
        
        asignacion.Tarea.ActualizarEstado(VerificarRecursosDeTareaDisponibles(asignacion.Tarea.Id));
        
        return Convertidor.AAsignacionRecursoTareaDTO(asignacion);
    }

    public void EliminarRecursoDeTarea(int idTarea)
    {
        AsignacionRecursoTarea asignacionRecursoTarea = _asignacionRepo.GetById(idTarea);
        Tarea tarea = asignacionRecursoTarea.Tarea;
        
        _asignacionRepo.Remove(asignacionRecursoTarea);
        
        tarea.ActualizarEstado(VerificarRecursosDeTareaDisponibles(tarea.Id));
    }

    public void ModificarAsignacion(AsignacionRecursoTareaDTO dto)
    {
        AsignacionRecursoTarea asignacionRecursoTarea = _asignacionRepo.GetById(dto.Id);
        
        asignacionRecursoTarea.Modificar(dto.Cantidad);
        
        asignacionRecursoTarea.Tarea.ActualizarEstado(VerificarRecursosDeTareaDisponibles(asignacionRecursoTarea.Tarea.Id));
    }

    public List<RecursoDTO> RecursosDeLaTarea(int tareaId)
    {
        List<AsignacionRecursoTarea> asignacionesFiltradas = _asignacionRepo.GetByTarea(tareaId);
        
        List<RecursoDTO> recursosDTO = asignacionesFiltradas.Select(a => a.Recurso)
            .Select(Convertidor.ARecursoDTO)
            .ToList();
        return recursosDTO;
    }

    public void EliminarRecursosDeTarea(int tareaId)
    {
        Tarea tarea = _tareaRepo.GetById(tareaId);
        List<AsignacionRecursoTarea> asignacionesFiltradas = _asignacionRepo.GetByTarea(tareaId);

        foreach (var asignacion in asignacionesFiltradas.ToList())
        {
            _asignacionRepo.Remove(asignacion);
        }
        tarea.ActualizarEstado(VerificarRecursosDeTareaDisponibles(tarea.Id));
    }

    public void ActualizarEstadoDeTareasConRecurso(int recursoID)
    {
        List<Tarea> tareas = _asignacionRepo.GetByRecurso(recursoID).Select(a => a.Tarea).ToList();
        foreach (Tarea tarea in tareas)
        { 
            tarea.ActualizarEstado(VerificarRecursosDeTareaDisponibles(tarea.Id));
        }
    }

    public bool RecursoEsExclusivo(int recursoID)
    {
        List<AsignacionRecursoTarea> tareasFiltradas = _asignacionRepo.GetByRecurso(recursoID);
        return tareasFiltradas.Count() > 1;
    }

    public bool VerificarRecursosDeTareaDisponibles(int TareaId)
    {
        List<AsignacionRecursoTarea> asignacionesDeTarea = _asignacionRepo.GetByTarea(TareaId);
        foreach (AsignacionRecursoTarea asignacion in asignacionesDeTarea)
        {
            if (!asignacion.Recurso.EstaDisponible(asignacion.CantidadNecesaria)) return false;
        }
        
        return true;
    }
}