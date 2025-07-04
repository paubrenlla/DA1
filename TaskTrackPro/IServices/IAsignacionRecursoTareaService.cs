using DTOs;

namespace IServices
{
    public interface IAsignacionRecursoTareaService
    {
        AsignacionRecursoTareaDTO GetById(int id);
        
        List<AsignacionRecursoTareaDTO> GetAll();
        
        AsignacionRecursoTareaDTO CrearAsignacionRecursoTarea(AsignacionRecursoTareaDTO dto);
        
        void EliminarRecursoDeTarea(int idTarea, int idRecurso);
        
        void ModificarAsignacion(AsignacionRecursoTareaDTO dto);
        
        List<RecursoDTO> RecursosDeLaTarea(int tareaId);

        void EliminarRecursosDeTarea (int tareaId);
        
        void ActualizarEstadoDeTareasConRecurso(int recursoID);

        bool RecursoEsExclusivo(int recursoID);
        
        bool VerificarRecursosDeTareaDisponibles(int TareaId);
        List<AsignacionRecursoTareaDTO>? GetAsignacionesDeTarea(int idTarea);
        List<AsignacionRecursoTareaDTO> ObtenerAsignacionesRecursoEnFecha(int recursoId, DateTime? fechaSeleccionada);
    }
}