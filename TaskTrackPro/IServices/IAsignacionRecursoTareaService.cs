using DTOs;

namespace Services
{
    public interface IAsignacionRecursoTareaService
    {
        AsignacionRecursoTareaDTO GetById(int id);
        
        List<AsignacionRecursoTareaDTO> GetAll();
        
        AsignacionRecursoTareaDTO CrearProyecto(AsignacionRecursoTareaDTO dto);
        
        void EliminarRecursoDeTarea(int id);
        
        void ModificarAsignacion(AsignacionRecursoTareaDTO dto);
        
        List<RecursoDTO> RecursosDeLaTarea(int tareaId);

        void EliminarRecursosDeTarea (int tareaId);
        
        void ActualizarEstadoDeTareasConRecurso(int recursoID);

        bool RecursoEsExclusivo(int recursoID);
        
        bool VerificarRecursosDeTareaDisponibles(int TareaId);
    }
}