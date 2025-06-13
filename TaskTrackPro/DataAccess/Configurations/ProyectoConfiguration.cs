using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ProyectoConfiguration : IEntityTypeConfiguration<Proyecto>
    {
        public void Configure(EntityTypeBuilder<Proyecto> b)
        {
            b.ToTable("Proyectos");
            b.HasKey(p => p.Id);
            b.Property(p => p.Nombre)
                .IsRequired();
            b.Property(p => p.Descripcion)
                .IsRequired()
                .HasMaxLength(400);
            b.Property(p => p.FechaInicio)
                .IsRequired();

            b.HasMany(p => p.TareasAsociadas)
                .WithOne()
                .HasForeignKey("ProyectoId")
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(p => p.AsignacionesDelProyecto)
                .WithOne(a => a.Proyecto)
                .HasForeignKey("ProyectoId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}