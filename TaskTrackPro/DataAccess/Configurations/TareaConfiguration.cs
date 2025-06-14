using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Configurations
{
    public class TareaConfiguration : IEntityTypeConfiguration<Tarea>
    {
        public void Configure(EntityTypeBuilder<Tarea> b)
        {
            b.ToTable("Tareas");
            b.HasKey(t => t.Id);

            // Mapea el Owned Type EstadoActual
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

            var ticksConv = new ValueConverter<TimeSpan, long>( //Esto es para poder convertir el TimeSpan a la DB
                ts => ts.Ticks,
                ticks => TimeSpan.FromTicks(ticks)
            );

            b.Property(t => t.Duracion)
                .HasConversion(ticksConv)
                .HasColumnType("bigint")     
                .IsRequired();

            b.Property(t => t.Holgura)
                .HasConversion(ticksConv)
                .HasColumnType("bigint") 
                .IsRequired();
        }
    }
}