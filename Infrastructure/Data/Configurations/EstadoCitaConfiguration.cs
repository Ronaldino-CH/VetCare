using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace Infrastructure.Data.Configurations
    {
        public class EstadoCitaConfiguration : IEntityTypeConfiguration<EstadoCita>
        {
            public void Configure(EntityTypeBuilder<EstadoCita> entity)
            {
                entity.ToTable("EstadoCita");
                entity.HasKey(e => e.IdEstadoCita);

                entity.Property(e => e.NombreEstado)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Codigo)
                    .HasMaxLength(2)
                    .IsRequired();
            }
        }
    }

}
