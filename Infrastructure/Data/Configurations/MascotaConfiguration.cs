using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class MascotaConfiguration : IEntityTypeConfiguration<Mascota>
    {
        public void Configure(EntityTypeBuilder<Mascota> entity)
        {
            entity.ToTable("Mascotas");
            entity.HasKey(e => e.IdMascota);

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Especie)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Raza)
                .HasMaxLength(100);

            entity.Property(e => e.Sexo)
                .HasMaxLength(20);

            entity.Property(e => e.Color)
                .HasMaxLength(50);

            entity.Property(e => e.Peso)
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.EstadoMascota)
                .IsRequired();

            entity.Property(e => e.FechaCrea)
                .IsRequired();

            // 🔗 Relación con Cliente
            entity.HasOne(e => e.Cliente)
                .WithMany() // luego puedes cambiar a .WithMany(c => c.Mascotas)
                .HasForeignKey(e => e.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
