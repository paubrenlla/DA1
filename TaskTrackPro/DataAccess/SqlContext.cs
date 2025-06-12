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
    
    public DbSet<AsignacionRecursoTarea> AsignacionesRecursoTarea { get; set; }
    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
        if(!Database.IsInMemory()) this.Database.Migrate();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Proyecto>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Nombre).IsRequired();
            b.Property(p => p.Descripcion).IsRequired().HasMaxLength(400);
            b.Property(p => p.FechaInicio).IsRequired();

            b.HasMany(p => p.TareasAsociadas)
                .WithOne()
                .HasForeignKey("ProyectoId")
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(p => p.AsignacionesDelProyecto)
                .WithOne(a => a.Proyecto)
                .HasForeignKey("ProyectoId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AsignacionProyecto>(b =>
        {
            b.HasKey(a => a.Id);

            b.HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey("UsuarioId")
                .OnDelete(DeleteBehavior.Restrict);

        });
        
        modelBuilder.Entity<Tarea>(b =>
        {
            b.HasKey(t => t.Id);
            b.OwnsOne(t => t.EstadoActual, estado =>
            {
                estado.Property(e => e.Valor)
                    .HasColumnName("EstadoValor")
                    .IsRequired();

                estado.Property(e => e.Fecha)
                    .HasColumnName("EstadoFecha")
                    .IsRequired(false);
            });
            
            b.HasMany(t => t.UsuariosAsignados)
                .WithMany(u => u.TareasAsignadas)
                .UsingEntity(j => j.ToTable("UsuarioTarea"));
        });
        modelBuilder.Entity<AsignacionRecursoTarea>(b =>
        {
            b.ToTable("AsignacionesRecursoTarea");
            b.HasKey(a => a.Id);

            b.HasOne(a => a.Recurso)
                .WithMany() 
                .HasForeignKey("RecursoId")
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(a => a.Tarea)
                .WithMany()
                .HasForeignKey("TareaId")
                .OnDelete(DeleteBehavior.Cascade);

            b.Property(a => a.CantidadNecesaria)
                .IsRequired();
        });
    }
}