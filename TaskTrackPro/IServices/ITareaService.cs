using Domain.Enums;
using DTOs;

namespace Services
{
    public interface ITareaService
    {
        TareaDTO BuscarTareaPorId(int id);
        
        List<TareaDTO> ListarTareasPorProyecto(int proyectoId);
        
        TareaDTO CrearTarea(int proyectoId, TareaDTO dto);
        
        void ModificarTarea(int tareaId, TareaDTO dto, int proyectoId);
        
        void MarcarComoEjecutandose(int tareaId);
        
        void MarcarComoCompletada(int tareaId);
        
        void AgregarDependencia(int tareaId, int dependenciaId, int proyectoId);
        
        void AgregarUsuario(int tareaId, int usuarioId);
        bool TieneSucesoras(TareaDTO tarea);
        bool TieneDependencias(TareaDTO tareaDto);
        bool UsuarioPerteneceALaTarea(int usuarioDtoId, int tareaId);
        void EliminarTarea(int proyectoId, int tareaId);
        TipoEstadoTarea GetEstadoTarea(int tareaId);
        List<UsuarioDTO>? ListarUsuariosDeTarea(int tareaId);
        void EliminarUsuarioDeTarea(int miembroId, int idTarea);
        void EliminarUsuarioDeTareasDeProyecto(int miembroId, int proyectoId);
        List<TareaDTO>? ObtenerDependenciasDeTarea(int tareaId);
        void EliminarDependencia(int tareaId, int dependenciaId, int proyectoId);
        List<TareaDTO> ListarTareasDelUsuario(int usuarioId, int proyectoId);
        bool PuedeCambiarDeEstado(int tareaSeleccionadaId);
        List<TareaDTO>? ObtenerTareasParaAgregarDependencia(int tareaSeleccionadaId, int proyectoId);
        bool PuedeAgregarDependencias(int tareaSeleccionadaId);
        bool PuedeEliminarTarea(TareaDTO tarea);
        void ActualizarEstadoTarea(TipoEstadoTarea estado, TareaDTO tareaSeleccionada);
        bool PuedeForzarRecursos(TareaDTO tarea);
        void ForzarRecursos(int proyectoId, int tareaId);
    }
}        