using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class TareaConfiguration : IEntityTypeConfiguration<Tarea>
    {
        public void Configure(EntityTypeBuilder<Tarea> b)
        {
            b.ToTable("Tareas");
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
        }
    }
}