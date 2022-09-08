using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.CourseService.Api.Contracts;
using WorkoutGlobal.CourseService.Api.DbContext;
using WorkoutGlobal.CourseService.Api.Filters.ActionFilters;
using WorkoutGlobal.CourseService.Api.Repositories;

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

        /// <summary>
        /// Configure instances of repository classes.
        /// </summary>
        /// <param name="services">Project services.</param>
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
        }

        /// <summary>
        /// Configure instances of attributes.
        /// </summary>
        /// <param name="services">Project services.</param>
        public static void ConfigureAttributes(this IServiceCollection services) => services.AddScoped<ModelValidationFilterAttribute>();
    }
}
