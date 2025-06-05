using Domain;
using Domain.Enums;
using IDataAcces;

namespace DataAccess
{
    public class AsignacionProyectoDataAccess : IDataAccessAsignacionProyecto
    {
        private List<AsignacionProyecto> _asignaciones;
        
        public AsignacionProyectoDataAccess()
        {
            _asignaciones = new List<AsignacionProyecto>();
        }
        public void Add(AsignacionProyecto asignacionProyecto)
        {
            _asignaciones.Add(asignacionProyecto);
        }

        public void Remove(AsignacionProyecto asignacionProyecto)
        {
            _asignaciones.Remove(asignacionProyecto);
        }
        public AsignacionProyecto? GetById(int Id)
        {
            return _asignaciones.FirstOrDefault(a => a.Id == Id);
        }

        public List<AsignacionProyecto> GetAll()
        {
            return _asignaciones.ToList();
        }
        public List<AsignacionProyecto> AsignacionesDelUsuario(int usuarioId)
        {
            return _asignaciones
                .Where(a => a.Usuario.Id == usuarioId)
                .ToList();
        }
        
        public List<AsignacionProyecto> UsuariosDelProyecto(int proyectoId)
        {
            return _asignaciones
                .Where(a => a.Proyecto.Id == proyectoId)
                .ToList();
        }
        

        public bool UsuarioEsAsignadoAProyecto(int usuarioId, int proyectoId)
        {
            return _asignaciones.Exists(a => a.Usuario.Id == usuarioId && a.Proyecto.Id == proyectoId);
        }

        public bool UsuarioEsAdminDelProyecto(int usuarioId, int proyectoId)
        {
            return _asignaciones.Exists(a => a.Usuario.Id == usuarioId 
                                             && a.Proyecto.Id == proyectoId 
                                             && a.Rol.Equals(Rol.Administrador));
        }

        public AsignacionProyecto GetAdminProyecto(int proyectoId)
        {
            return _asignaciones.FirstOrDefault(a => a.Proyecto.Id == proyectoId && a.Rol.Equals(Rol.Administrador));
        }

        public List<Usuario>? GetMiembrosDeProyecto(int id)
        {
            List<AsignacionProyecto> asignacionProyectos = _asignaciones.Where(a => a.Proyecto.Id == id).ToList();
            return asignacionProyectos.Select(p => p.Usuario).ToList();
        }
    }
}