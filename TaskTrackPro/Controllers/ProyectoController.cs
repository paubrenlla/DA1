using DTOs;
using Services;

namespace Controllers
{
    public class ProyectoController
    {
        private readonly IProyectoService _service;

        public ProyectoController(IProyectoService service)
        {
            _service = service;
        }

        public ProyectoDTO BuscarProyectoPorId(int id)
        {
            return _service.GetById(id);
        }

        public List<ProyectoDTO> GetAll()
        {
            return _service.GetAll();
        }

        public ProyectoDTO AgregarProyecto(ProyectoDTO dto)
        {
            return _service.CrearProyecto(dto);
        }

        public void EliminarProyecto(int id)
        {
            _service.Delete(id);
        }

        public void ModificarProyecto(ProyectoDTO dto)
        {
            _service.ModificarProyecto(dto);
        }

        public bool EsAdminDeAlgunProyecto(int usuarioId)
        {
            return _service.UsuarioEsAdminDeAlgunProyecto(usuarioId);
        }

        public List<ProyectoDTO> ProyectosDelUsuario(int usuarioId)
        {
            return _service.ProyectosDelUsuario(usuarioId);
        }

        public void EliminarAsignacionesUsuario(int usuarioId)
        {
            _service.EliminarAsignacionesDeUsuario(usuarioId);
        }

        public bool UsuarioEsAdminEnProyecto(int usuarioId, int proyectoId)
        {
            return _service.UsuarioEsAdminEnProyecto(usuarioId, proyectoId);
        }

        public void AsignarAdminProyecto(int usuarioId, int proyectoId)
        {
            _service.AsignarAdminDeProyecto(usuarioId, proyectoId);
        }
    }
}