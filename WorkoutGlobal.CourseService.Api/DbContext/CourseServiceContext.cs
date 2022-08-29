using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.Api.DbContext
{
    /// <summary>
    /// Represents database context of course microservice project.
    /// </summary>
    public class CourseServiceContext : Microsoft.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// Ctor for set course microsevice context options.
        /// </summary>
        /// <param name="options">Context options.</param>
        public CourseServiceContext(DbContextOptions options)
            : base(options)
        { }

        /// <summary>
        /// Create relations between models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Relations

            modelBuilder.Entity<CourseLesson>()
                .HasOne(lesson => lesson.Course)
                .WithMany(course => course.CourseLessons)
                .HasForeignKey(lesson => lesson.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }

        /// <summary>
        /// Represents table of courses.
        /// </summary>
        public DbSet<Course> Courses { get; set; }

        /// <summary>
        /// Represents table of course lessons.
        /// </summary>
        public DbSet<CourseLesson> CourseLessons { get; set; }
    }
}
