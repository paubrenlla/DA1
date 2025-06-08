using Domain.Enums;
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
        bool TieneSucesoras(TareaDTO tarea);
        bool TieneDependencias(TareaDTO tareaDto);
        bool UsuarioPerteneceALaTarea(int usuarioDtoId, int tareaId);
        void EliminarTarea(int proyectoId, int tareaId);
        TipoEstadoTarea GetEstadoTarea(int tareaId);
        List<UsuarioDTO>? ListarUsuariosDeTarea(int tareaId);
        void EliminarUsuarioDeTarea(int miembroId, int idTarea);
        void EliminarUsuarioDeTareasDeProyecto(int miembroId, int proyectoId);
    }
}        