using DataAccess.Configurations;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class SqlContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Recurso> Recursos { get; set; }
    public DbSet<Tarea> Tareas { get; set; }
    public DbSet<AsignacionProyecto> AsignacionesProyecto { get; set; }
    public DbSet<Proyecto> Proyectos { get; set; }
    public DbSet<Notificacion> Notificaciones { get; set; }
    public DbSet<AsignacionRecursoTarea> AsignacionesRecursoTarea { get; set; }
    public SqlContext(DbContextOptions<SqlContext> options) : base(options) {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProyectoConfiguration());
        modelBuilder.ApplyConfiguration(new AsignacionProyectoConfiguration());
        modelBuilder.ApplyConfiguration(new TareaConfiguration());
        modelBuilder.ApplyConfiguration(new AsignacionRecursoTareaConfiguration());
        modelBuilder.ApplyConfiguration(new NotificacionConfiguration());
    }
}