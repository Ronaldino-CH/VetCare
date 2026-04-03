using Application.Clientes.Interfaces;
using Application.Clientes.Services;
using Infrastructure.Data;
using Infrastructure.Data.Clientes.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.IoC
{
    /// <summary>
    /// Clase estática encargada de registrar todas las dependencias del proyecto.
    /// Aquí centralizamos la configuración de:
    /// - DbContext
    /// - Repositorios
    /// - Servicios
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Método de extensión para registrar todas las dependencias de Infrastructure
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // =========================
            // 🔹 CONFIGURACIÓN DE BASE DE DATOS
            // =========================

            // Registramos el DbContext para usar Entity Framework con SQL Server
            services.AddDbContext<VetCareDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("VetCareConnection")));

            // =========================
            // AUTOMAPPER
            // =========================
            // Registra los perfiles de mapeo definidos en Application.Mapping
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // =========================
            // 🔹 REGISTRO DE REPOSITORIOS
            // =========================

            // Scoped = se crea una instancia por cada request HTTP
            services.AddScoped<IClienteRepository, ClienteRepository>();

            // =========================
            // 🔹 REGISTRO DE SERVICIOS
            // =========================

            services.AddScoped<IClienteService, ClienteService>();

            // Aquí luego irán:
            // services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            // services.AddScoped<IUsuarioService, UsuarioService>();

            return services;
        }
    }
}
