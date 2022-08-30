using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.CourseService.Api.Contracts;
using WorkoutGlobal.CourseService.Api.DbContext;
using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.Api.Repositories
{
    /// <summary>
    /// Represent lesson repository.
    /// </summary>
    public class LessonRepository : BaseRepository<Lesson, Guid>, ILessonRepository
    {
        /// <summary>
        /// Ctor for lesson repository.
        /// </summary>
        /// <param name="configuration">Project configuration.</param>
        /// <param name="context">Database context.</param>
        public LessonRepository(
            IConfiguration configuration,
            CourseServiceContext context)
            : base(configuration, context)
        { }

        /// <summary>
        /// Create lesson.
        /// </summary>
        /// <param name="courseLesson">Creation lesson.</param>
        /// <returns>Return generated id for new lesson.</returns>
        public async Task<Guid> CreateLessonAsync(Lesson courseLesson)
        {
            var createdId = await CreateAsync(courseLesson);

            await SaveChangesAsync();

            return createdId;
        }

        /// <summary>
        /// Delete lesson.
        /// </summary>
        /// <param name="id">Lesson id.</param>
        /// <returns></returns>
        public async Task DeleteLessonAsync(Guid id)
        {
            await DeleteAsync(id);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Get all lessons.
        /// </summary>
        /// <returns>Returns collection of all lessons.</returns>
        public async Task<IEnumerable<Lesson>> GetAllLessonsAsync()
        {
            var models = await GetAll().ToListAsync();

            return models;
        }

        /// <summary>
        /// Get lesson by id.
        /// </summary>
        /// <param name="id">Lesson id.</param>
        /// <param name="trackChanges">Tracking changes state.</param>
        /// <returns>Returns lesson by given id.</returns>
        public async Task<Lesson> GetLessonAsync(Guid id, bool trackChanges = true)
        {
            var model = await GetAsync(id, trackChanges);

            return model;
        }

        /// <summary>
        /// Get lesson course by lesson identifier.
        /// </summary>
        /// <param name="lessonId">Lesson id.</param>
        /// <returns>Return course of lesson.</returns>
        public async Task<Course> GetLessonCourseAsync(Guid lessonId)
        {
            var lesson = await GetAsync(lessonId);

            var course = await Context.Courses
                .Where(course => course.Id == lesson.CourseId)
                .FirstOrDefaultAsync();

            return course;
        }

        /// <summary>
        /// Update lesson.
        /// </summary>
        /// <param name="courseLesson">Updation model.</param>
        /// <returns></returns>
        public async Task UpdateLessonAsync(Lesson courseLesson)
        {
            Update(courseLesson);

            await SaveChangesAsync();
        }
    }
}
