using DTOs;

namespace Services
{
    public interface ITareaService
    {
        TareaDTO BuscarTareaPorId(int id);
        
        List<TareaDTO> ListarTareasPorProyecto(int proyectoId);
        
        TareaDTO CrearTarea(int proyectoId, TareaDTO dto);
        
        void ModificarTarea(int tareaId, TareaDTO dto);
        
        void MarcarComoEjecutandose(int tareaId);
        
        void MarcarComoCompletada(int tareaId);
        
        void AgregarDependencia(int tareaId, int dependenciaId);
        
        void AgregarUsuario(int tareaId, int usuarioId);
    }
}