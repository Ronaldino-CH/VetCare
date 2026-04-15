using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class CitaConfiguration : IEntityTypeConfiguration<Cita>
    {
        public void Configure(EntityTypeBuilder<Cita> entity)
        {
            entity.ToTable("Citas");
            entity.HasKey(e => e.IdCita);

            entity.Property(e => e.Motivo)
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(e => e.Observaciones)
                .HasMaxLength(500);

            entity.Property(e => e.FechaHora)
                .IsRequired();

            entity.Property(e => e.FechaCrea)
                .IsRequired();

            entity.Property(e => e.FechaEdita)
                .IsRequired(false);

            entity.Property(e => e.IdMascota)
                .IsRequired();

            entity.Property(e => e.IdVeterinario)
                .IsRequired();

            entity.Property(e => e.IdEstadoCita)
                .IsRequired();

            entity.HasOne(e => e.Mascota)
                .WithMany()
                .HasForeignKey(e => e.IdMascota)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.IdVeterinario)
                .IsRequired();

            entity.Property(e => e.IdEstadoCita)
                .IsRequired();

            //entity.HasOne(e => e.Veterinario)
            //    .WithMany()
            //    .HasForeignKey(e => e.IdVeterinario)
            //    .OnDelete(DeleteBehavior.Restrict);

            //entity.HasOne(e => e.EstadoCita)
            //    .WithMany()
            //    .HasForeignKey(e => e.IdEstadoCita)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
