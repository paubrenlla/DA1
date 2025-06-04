using DTOs;

namespace Services
{
    public interface IProyectoService
    {
        ProyectoDTO GetById(int id);
        
        List<ProyectoDTO> GetAll();
        
        ProyectoDTO CrearProyecto(ProyectoDTO dto);
        
        void Delete(int id);
        
        void ModificarProyecto(ProyectoDTO dto);
        
        bool UsuarioEsAdminDeAlgunProyecto(int usuarioId);
        
        List<ProyectoDTO> ProyectosDelUsuario(int usuarioId);
        
        void EliminarAsignacionesDeUsuario(int usuarioId);
        
        bool UsuarioEsAdminEnProyecto(int usuarioId, int proyectoId);

        void AsignarAdminDeProyecto(int usuarioId, int proyectoId);
    }
}
