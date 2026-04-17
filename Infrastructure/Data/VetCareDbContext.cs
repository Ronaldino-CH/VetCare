using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class VetCareDbContext : DbContext
    {
        public VetCareDbContext(DbContextOptions<VetCareDbContext> options) : base(options)
        {
        }

        //agregar los demás

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Mascota> Mascotas => Set<Mascota>();
        public DbSet<Cita> Citas => Set<Cita>();
        public DbSet<EstadoCita> EstadoCitas => Set<EstadoCita>();
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Domain.Entities.HistorialClinico> HistorialClinico => Set<Domain.Entities.HistorialClinico>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VetCareDbContext).Assembly);
        }
    }
}
