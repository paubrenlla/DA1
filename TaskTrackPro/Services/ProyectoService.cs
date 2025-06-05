using DTOs;
using Domain;
using Domain.Enums;
using IDataAcces;

namespace Services
{
    public class ProyectoService : IProyectoService
    {
        private readonly IDataAccessProyecto _proyectoRepo;
        private readonly IDataAccessUsuario _usuarioRepo;
        private readonly IDataAccessAsignacionProyecto _asignacionRepo;

        public ProyectoService(
            IDataAccessProyecto proyectoRepo,
            IDataAccessUsuario usuarioRepo,
            IDataAccessAsignacionProyecto asignacionRepo)
        {
            _proyectoRepo = proyectoRepo;
            _usuarioRepo = usuarioRepo;
            _asignacionRepo = asignacionRepo;
        }

        public ProyectoDTO GetById(int id)
        {
            Proyecto proyecto = _proyectoRepo.GetById(id);
            return Convertidor.AProyectoDTO(proyecto);
        }

        public List<ProyectoDTO> GetAll()
        {
            return _proyectoRepo.GetAll()
                .Select(Convertidor.AProyectoDTO)
                .ToList();
        }

        public ProyectoDTO CrearProyecto(ProyectoDTO dto)
        {
            Proyecto nuevo = new Proyecto(dto.Nombre, dto.Descripcion, dto.FechaInicio);
            _proyectoRepo.Add(nuevo);
            return Convertidor.AProyectoDTO(nuevo);
        }

        public void Delete(int id)
        {
            Proyecto proyecto = _proyectoRepo.GetById(id);
            List<AsignacionProyecto> asignaciones = _asignacionRepo.UsuariosDelProyecto(id);
            foreach (AsignacionProyecto asign in asignaciones)
            {
                _asignacionRepo.Remove(asign);
            }
            _proyectoRepo.Remove(proyecto);
        }

        public void ModificarProyecto(ProyectoDTO dto)
        {
            var proyecto = _proyectoRepo.GetById(dto.Id);
            proyecto.Modificar(dto.Descripcion, dto.FechaInicio);
        }

        public bool UsuarioEsAdminDeAlgunProyecto(int usuarioId)
        {
            List<AsignacionProyecto> asignaciones = _asignacionRepo.GetAll();
            return asignaciones.Any(a=> a.Usuario.Id == usuarioId && a.Rol == Rol.Administrador);
        }

        public List<ProyectoDTO> ProyectosDelUsuario(int usuarioId)
        {
            return _asignacionRepo
                .AsignacionesDelUsuario(usuarioId)
                .Select(a => Convertidor.AProyectoDTO(a.Proyecto))
                .ToList();
        }


        public void EliminarAsignacionesDeUsuario(int usuarioId)
        {
            List<AsignacionProyecto> asignaciones = _asignacionRepo.AsignacionesDelUsuario(usuarioId);
            foreach (AsignacionProyecto asign in asignaciones)
            {
                _asignacionRepo.Remove(asign);
            }
        }

        public bool UsuarioEsAdminEnProyecto(int usuarioId, int proyectoId)
        {
            return _asignacionRepo.UsuarioEsAdminDelProyecto(usuarioId, proyectoId);
        }

        private void EliminarAdminAnterior(int proyectoId)
        {
            AsignacionProyecto antiguoAdmin = _asignacionRepo.GetAdminProyecto(proyectoId);
            _asignacionRepo.Remove(antiguoAdmin);
        }
        public void AsignarAdminDeProyecto(int usuarioId, int proyectoId)
        {
            EliminarAdminAnterior(proyectoId);
            
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
            Usuario  usuario = _usuarioRepo.GetById(usuarioId);
            AsignacionProyecto nuevoAdmin = new AsignacionProyecto(proyecto, usuario, Rol.Administrador);
            _asignacionRepo.Add(nuevoAdmin);
        }
        
        public UsuarioDTO GetAdminDeProyecto(int id)
        {
            return Convertidor.AUsuarioDTO(_asignacionRepo.GetAdminProyecto(id).Usuario);
        }

        public List<UsuarioDTO>? GetMiembrosDeProyecto(int id)
        {
            return _asignacionRepo.GetMiembrosDeProyecto(id).Select(Convertidor.AUsuarioDTO).ToList();
        }

        public void AgregarMiembroProyecto(int usuarioId, int proyectoId)
        {
            Usuario usuario = _usuarioRepo.GetById(usuarioId);
            if (_asignacionRepo.GetMiembrosDeProyecto(proyectoId).Contains(usuario))
                throw new ArgumentException("El usuario ya se encuentra asignado al proyecto.");
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
            AsignacionProyecto asignacion = new AsignacionProyecto(proyecto, usuario, Rol.Administrador); 
            _asignacionRepo.Add(asignacion);
        }

        public void EliminarMiembroDeProyecto(int miembroId, int proyectoId)
        {
            if(_asignacionRepo.GetAdminProyecto(proyectoId).Usuario.Id == miembroId)
                throw new ArgumentException("No puedes eliminar al Administrador de Proyecto");
            List<AsignacionProyecto> asignaciones = _asignacionRepo.AsignacionesDelUsuario(miembroId);
            AsignacionProyecto aEliminar = asignaciones.FirstOrDefault(a => a.Proyecto.Id == proyectoId);
            _asignacionRepo.Remove(aEliminar);
        }
    }
}
