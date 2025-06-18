using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class NotificacionConfiguration : IEntityTypeConfiguration<Notificacion>
    {
        public void Configure(EntityTypeBuilder<Notificacion> b)
        {
            b.ToTable("Notificaciones");
            b.HasKey(n => n.Id);
            b.Property(n => n.Mensaje)
                .IsRequired();

            b.HasMany(n => n.UsuariosNotificados)
                .WithMany(u => u.NotificacionesRecibidas)
                .UsingEntity(j => j.ToTable("NotificacionUsuarios"));

            b.HasMany(n => n.VistaPorUsuarios)
                .WithMany(u => u.NotificacionesVistas)
                .UsingEntity(j => j.ToTable("NotificacionVistas"));
        }
    }
}