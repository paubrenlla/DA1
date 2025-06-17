using Domain;
using IDataAcces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AsignacionRecursoTareaDataAccess : IDataAccessAsignacionRecursoTarea
    {
        private readonly SqlContext _context;

        public AsignacionRecursoTareaDataAccess(SqlContext context)
        {
            _context = context;
        }

        public void Add(AsignacionRecursoTarea asignacion)
        {
            _context.AsignacionesRecursoTarea.Add(asignacion);
            _context.SaveChanges();
        }

        public void Remove(AsignacionRecursoTarea asignacion)
        {
            _context.AsignacionesRecursoTarea.Remove(asignacion);
            _context.SaveChanges();
        }

        public AsignacionRecursoTarea? GetById(int id)
        {
            return _context.AsignacionesRecursoTarea
                .Include(a => a.Recurso)
                .Include(a => a.Tarea)
                .FirstOrDefault(a => a.Id == id);
        }

        public List<AsignacionRecursoTarea> GetAll()
        {
            return _context.AsignacionesRecursoTarea
                .Include(a => a.Recurso)
                .Include(a => a.Tarea)
                .AsNoTracking()
                .ToList();
        }

        public List<AsignacionRecursoTarea> GetByTarea(int tareaId)
        {
            return _context.AsignacionesRecursoTarea
                .Include(a => a.Recurso)
                .Include(a => a.Tarea)
                .Where(a => a.Tarea.Id == tareaId)
                .ToList();
        }

        public List<AsignacionRecursoTarea> GetByRecurso(int recursoId)
        {
            return _context.AsignacionesRecursoTarea
                .Include(a => a.Recurso)
                .Include(a => a.Tarea)
                .Where(a => a.Recurso.Id == recursoId)
                .ToList();
        }

        public int CantidadDelRecurso(AsignacionRecursoTarea asignacion)
        {
            AsignacionRecursoTarea asign = GetById(asignacion.Id);
            if (asign is null)
                throw new KeyNotFoundException("Asignación no encontrada");
            return asign.CantidadNecesaria;
        }

        public AsignacionRecursoTarea? GetByRecursoYTarea(int recursoId, int tareaId)
        {
            return _context.AsignacionesRecursoTarea
                .Include(a => a.Recurso)
                .Include(a => a.Tarea)
                .FirstOrDefault(a => a.Recurso.Id == recursoId && a.Tarea.Id == tareaId);
        }

        public void Update(AsignacionRecursoTarea asignacion)
        {
            _context.Update(asignacion);
            _context.SaveChanges();
        }
    }
}
