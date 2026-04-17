using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class HistorialClinicoConfiguration : IEntityTypeConfiguration<Domain.Entities.HistorialClinico>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.HistorialClinico> entity)
        {
            entity.ToTable("HistorialClinico");
            entity.HasKey(e => e.IdHistorial);

            entity.Property(e => e.Diagnostico)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.Tratamiento)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.Observaciones)
                .HasMaxLength(1000);

            entity.Property(e => e.FechaCrea)
                .IsRequired();

            entity.HasOne(e => e.Mascota)
                .WithMany()
                .HasForeignKey(e => e.IdMascota)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Veterinario)
                .WithMany()
                .HasForeignKey(e => e.IdVeterinario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}