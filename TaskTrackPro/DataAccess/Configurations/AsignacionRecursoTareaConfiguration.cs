using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class AsignacionRecursoTareaConfiguration : IEntityTypeConfiguration<AsignacionRecursoTarea>
    {
        public void Configure(EntityTypeBuilder<AsignacionRecursoTarea> b)
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
        }
    }
}