using Domain;
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
        UsuarioDTO GetAdminDeProyecto(int id);
        List<UsuarioDTO>? GetMiembrosDeProyecto(int id);
        void AgregarMiembroProyecto(int usuarioId, int proyectoId);
        void EliminarMiembroDeProyecto(int miembroId, int proyectoId);
        List<TareaDTO> ObtenerRutaCritica(int proyectoId);
        List<TareaDTO> TareasNoCriticas(int proyectoId);
        List<TareaDTO> TareasOrdenadasPorInicio(int proyectoId);
        String Exportar(string dato);
        
    }
}
