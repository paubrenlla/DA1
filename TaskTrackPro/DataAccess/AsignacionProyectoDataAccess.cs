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
            AsignacionProyecto asign = _context.AsignacionesProyecto
                .Include(x => x.Usuario)
                .Include(x => x.Proyecto)
                .FirstOrDefault(x => x.Id == Id);
            if (asign is null)
                throw new ArgumentException("No existe la asignación");
            return asign;
        }

        public List<AsignacionProyecto> GetAll()
        {
           return _context.AsignacionesProyecto
               .Include(x=>x.Usuario)
               .Include(x=>x.Proyecto)
               .AsNoTracking()
               .ToList();
        }
        public List<AsignacionProyecto> AsignacionesDelUsuario(int usuarioId)
        {
            return _context.AsignacionesProyecto
                .Include(a => a.Proyecto)
                .Include(a => a.Usuario)
                .Where(a => a.Usuario.Id == usuarioId)
                .ToList();
        }
        
        public List<AsignacionProyecto> UsuariosDelProyecto(int proyectoId)
        {
            return _context.AsignacionesProyecto
                .Include(a => a.Proyecto)
                .Include(a => a.Usuario)
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
            return _context.AsignacionesProyecto
                .Include(a => a.Usuario)
                .Include(a => a.Proyecto)
                .FirstOrDefault(a => a.Proyecto.Id == proyectoId && a.Rol == Rol.Administrador);
        }

        public List<Usuario> GetMiembrosDeProyecto(int id)
        {
            return _context.AsignacionesProyecto
                .Where(a => a.Proyecto.Id == id)
                .Include(a => a.Usuario)
                .Select(a => a.Usuario)
                .ToList();
        }

        public AsignacionProyecto GetLiderProyecto(int proyectoId)
        {
            return _context.AsignacionesProyecto
                .Include(a => a.Usuario)
                .Include(a => a.Proyecto)
                .FirstOrDefault(a => a.Proyecto.Id == proyectoId && a.Rol == Rol.Lider);
        }
    }
}