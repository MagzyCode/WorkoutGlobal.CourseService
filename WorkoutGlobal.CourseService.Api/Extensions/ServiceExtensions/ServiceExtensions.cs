using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.CourseService.Api.DbContext;

namespace WorkoutGlobal.CourseService.Api.Extensions
{
    /// <summary>
    /// Base class for all service extensions.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configure database settings.
        /// </summary>
        /// <param name="services">Project services.</param>
        /// <param name="configuration">Project configuration.</param>
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<CourseServiceContext>(
                opts => opts.UseNpgsql(
                    connectionString: configuration.GetConnectionString("CourseConnectionString"),
                    npgsqlOptionsAction: b => b.MigrationsAssembly("WorkoutGlobal.CourseService.Api")));
    }
}
