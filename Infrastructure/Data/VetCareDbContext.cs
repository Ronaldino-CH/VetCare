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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VetCareDbContext).Assembly);
        }
    }
}
