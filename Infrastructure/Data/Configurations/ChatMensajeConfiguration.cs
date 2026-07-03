using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ChatMensajeConfiguration : IEntityTypeConfiguration<ChatMensaje>
    {
        public void Configure(EntityTypeBuilder<ChatMensaje> entity)
        {
            entity.ToTable("ChatMensajes");
            entity.HasKey(e => e.IdMensaje);

            entity.Property(e => e.IdMensaje)
                .UseIdentityColumn();

            entity.Property(e => e.Mensaje)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(e => e.FechaEnvio)
                .HasColumnType("datetime2(0)")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .IsRequired();

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.ChatMensajes)
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.IdUsuario)
                .HasDatabaseName("IX_ChatMensajes_IdUsuario");

            entity.HasIndex(e => e.FechaEnvio)
                .HasDatabaseName("IX_ChatMensajes_FechaEnvio");

            entity.HasIndex(e => new { e.Activo, e.FechaEnvio })
                .HasDatabaseName("IX_ChatMensajes_Activo_FechaEnvio");
        }
    }
}
