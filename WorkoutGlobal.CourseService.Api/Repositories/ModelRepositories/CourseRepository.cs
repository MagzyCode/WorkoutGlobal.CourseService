using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.CourseService.Api.Contracts;
using WorkoutGlobal.CourseService.Api.DbContext;
using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.Api.Repositories.ModelRepositories
{
    /// <summary>
    /// Represents repository for course model.
    /// </summary>
    public class CourseRepository : BaseRepository<Course, Guid>, ICourseRepository
    {
        /// <summary>
        /// Ctor for course repository.
        /// </summary>
        /// <param name="configuration">Project configuration.</param>
        /// <param name="context">Database context.</param>
        public CourseRepository(
            IConfiguration configuration, 
            CourseServiceContext context)
            : base(configuration, context)
        { }

        /// <summary>
        /// Create course.
        /// </summary>
        /// <param name="creationModel">Creation model.</param>
        /// <returns>Return generated id for new model.</returns>
        public async Task<Guid> CreateCourse(Course creationModel)
        {
            var createdId = await CreateAsync(creationModel);

            await SaveChangesAsync();

            return createdId;
        }

        /// <summary>
        /// Delete model by id.
        /// </summary>
        /// <param name="id">Course id.</param>
        /// <returns></returns>
        public async Task DeleteCourse(Guid id)
        {
            await DeleteAsync(id);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Get all lesson by course id.
        /// </summary>
        /// <param name="courseId">Course id.</param>
        /// <returns>Returns collection of course lessons.</returns>
        public async Task<IEnumerable<Lesson>> GetAllCourseLessons(Guid courseId)
        {
            if (courseId == Guid.Empty)
                throw new ArgumentOutOfRangeException(nameof(courseId), courseId, "Course id cannot be empty.");

            var lessons = await Context.CourseLessons
                .Where(x => x.CourseId == courseId)
                .ToListAsync();

            return lessons;
        }

        /// <summary>
        /// Get all courses.
        /// </summary>
        /// <returns>Returns collection of all courses.</returns>
        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            var models = await GetAll().ToListAsync();

            return models;
        }

        /// <summary>
        /// Get course by id.
        /// </summary>
        /// <param name="id">Course id.</param>
        /// <returns>Returns course by given id.</returns>
        public async Task<Course> GetCourse(Guid id)
        {
            var model = await GetAsync(id);

            return model;
        }

        /// <summary>
        /// Update course.
        /// </summary>
        /// <param name="course">Updation course.</param>
        /// <returns></returns>
        public async Task UpdateCourse(Course course)
        {
            Update(course);
            await SaveChangesAsync();
        }
    }
}
