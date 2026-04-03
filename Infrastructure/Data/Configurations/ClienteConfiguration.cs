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
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> entity)
        {
            entity.ToTable("Clientes");
            entity.HasKey(e => e.IdCliente);

            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Documento)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.Telefono)
                .HasMaxLength(20);

            entity.Property(e => e.Correo)
                .HasMaxLength(100);

            entity.Property(e => e.Direccion)
                .HasMaxLength(200);

            entity.Property(e => e.EstadoCliente)
                .IsRequired();

            entity.Property(e => e.FechaCrea)
                .IsRequired();
        }
    }
}
