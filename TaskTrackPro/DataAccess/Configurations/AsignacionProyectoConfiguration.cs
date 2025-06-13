using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class AsignacionProyectoConfiguration : IEntityTypeConfiguration<AsignacionProyecto>
    {
        public void Configure(EntityTypeBuilder<AsignacionProyecto> b)
        {
            b.ToTable("AsignacionesProyecto");
            b.HasKey(a => a.Id);
            
            b.HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey("UsuarioId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}