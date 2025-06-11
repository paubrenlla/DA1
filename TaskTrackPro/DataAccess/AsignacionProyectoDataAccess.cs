using Domain;
using Domain.Enums;
using IDataAcces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AsignacionProyectoDataAccess : IDataAccessAsignacionProyecto
    {
        private readonly SqlContext _context;        
        public AsignacionProyectoDataAccess(SqlContext context)
        {
            _context = context;
        }
        public void Add(AsignacionProyecto asignacionProyecto)
        {
            _context.AsignacionesProyecto.Add(asignacionProyecto);
            _context.SaveChanges();
        }

        public void Remove(AsignacionProyecto asignacionProyecto)
        {
            _context.AsignacionesProyecto.Remove(asignacionProyecto);
            _context.SaveChanges();
        }
        public AsignacionProyecto? GetById(int Id)
        {
            AsignacionProyecto asignacionProyecto = _context.AsignacionesProyecto.Find(Id);
            if(asignacionProyecto is null)
                throw new ArgumentException("No se encontro la asigancion de proyecto");
            return asignacionProyecto;
        }

        public List<AsignacionProyecto> GetAll()
        {
           return _context.AsignacionesProyecto.AsNoTracking().ToList();
        }
        public List<AsignacionProyecto> AsignacionesDelUsuario(int usuarioId)
        {
            return _context.AsignacionesProyecto
                .Where(a => a.Usuario.Id == usuarioId)
                .ToList();
        }
        
        public List<AsignacionProyecto> UsuariosDelProyecto(int proyectoId)
        {
            return _context.AsignacionesProyecto
                .Where(a => a.Proyecto.Id == proyectoId)
                .ToList();
        }
        

        public bool UsuarioEsAsignadoAProyecto(int usuarioId, int proyectoId)
        {
            return _context.AsignacionesProyecto.Count(a => a.Usuario.Id == usuarioId && a.Proyecto.Id == proyectoId) == 1;
        }

        public bool UsuarioEsAdminDelProyecto(int usuarioId, int proyectoId)
        {
            return _context.AsignacionesProyecto.Count(a => a.Usuario.Id == usuarioId 
                                                            && a.Proyecto.Id == proyectoId 
                                                            && a.Rol.Equals(Rol.Administrador)) == 1;
        }

        public AsignacionProyecto GetAdminProyecto(int proyectoId)
        {
            return _context.AsignacionesProyecto.FirstOrDefault(a => a.Proyecto.Id == proyectoId && a.Rol.Equals(Rol.Administrador));
        }

        public List<Usuario>? GetMiembrosDeProyecto(int id)
        {
            List<AsignacionProyecto> asignacionProyectos = _context.AsignacionesProyecto.Where(a => a.Proyecto.Id == id).ToList();
            return asignacionProyectos.Select(p => p.Usuario).ToList();
        }
    }
}