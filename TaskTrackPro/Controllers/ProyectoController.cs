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

        public UsuarioDTO GetAdminDeProyecto(int id)
        {
            return _service.GetAdminDeProyecto(id);
        }

        public List<UsuarioDTO>? GetMiembrosDeProyecto(int id)
        {
            return _service.GetMiembrosDeProyecto(id);
        }

        public void AgregarMiembroProyecto(int usuarioId, int proyectoId)
        {
            _service.AgregarMiembroProyecto(usuarioId, proyectoId);
        }

        public void EliminarMiembro(int miembroId, int proyectoId)
        {
            _service.EliminarMiembroDeProyecto(miembroId, proyectoId);
        }
        
        public List<TareaDTO> ObtenerRutaCritica(int proyectoId)
        {
            return _service.ObtenerRutaCritica(proyectoId);
        }

        public List<TareaDTO> TareasNoCriticas(int proyectoId)
        {
            return _service.TareasNoCriticas(proyectoId);
        }

        public List<TareaDTO> TareasOrdenadasPorInicio(int proyectoId)
        {
            return _service.TareasOrdenadasPorInicio(proyectoId);
        }

        public string Exportar(string dato)
        {
            return _service.Exportar(dato);
        }
        
        public void AsignarLiderProyecto(int usuarioLiderId, int nuevoId)
        {
            _service.AsignarLiderDeProyecto(usuarioLiderId, nuevoId);
        }

        public UsuarioDTO GetLiderDeProyecto(int id)
        {
            return _service.GetLiderDeProyecto(id);
        }

        public bool UsuarioEsLiderDeProyecto(int usuarioId, int proyectoId)
        {
            return _service.UsuarioEsLiderDeProyecto(usuarioId, proyectoId);
        }

        public bool UsuarioEsLiderOAdminDeAlgunProyecto(int usuarioId)
        {
            return _service.UsuarioEsLiderOAdminDeAlgunProyecto(usuarioId);
        }

        public bool UsuarioEsLiderDeAlgunProyecto(int usuarioLogeadoId)
        {
            return _service.UsuarioEsLiderDeAlgunProyecto(usuarioLogeadoId);
        }
    }
}