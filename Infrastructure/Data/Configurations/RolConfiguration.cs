using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> entity)
        {
            entity.ToTable("Roles");
            entity.HasKey(e => e.IdRol);

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}