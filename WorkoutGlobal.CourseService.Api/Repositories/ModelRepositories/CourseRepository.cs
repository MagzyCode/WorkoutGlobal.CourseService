using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.CourseService.Api.Contracts;
using WorkoutGlobal.CourseService.Api.DbContext;
using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.Api.Repositories
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
        public async Task<Guid> CreateCourseAsync(Course creationModel)
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
        public async Task DeleteCourseAsync(Guid id)
        {
            await DeleteAsync(id);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Get all lesson by course id.
        /// </summary>
        /// <param name="courseId">Course id.</param>
        /// <returns>Returns collection of course lessons.</returns>
        public async Task<IEnumerable<Lesson>> GetAllCourseLessonsAsync(Guid courseId)
        {
            if (courseId == Guid.Empty)
                throw new ArgumentNullException(nameof(courseId), "Course id cannot be empty.");

            var lessons = await Context.CourseLessons
                .Where(x => x.CourseId == courseId)
                .ToListAsync();

            return lessons;
        }

        /// <summary>
        /// Get all courses.
        /// </summary>
        /// <returns>Returns collection of all courses.</returns>
        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            var models = await GetAll().ToListAsync();

            return models;
        }

        /// <summary>
        /// Get course by id.
        /// </summary>
        /// <param name="id">Course id.</param>
        /// <param name="trackChanges">Tracking model change.</param>
        /// <returns>Returns course by given id.</returns>
        public async Task<Course> GetCourseAsync(Guid id, bool trackChanges = true)
        {
            var model = await GetAsync(id, trackChanges);

            return model;
        }

        /// <summary>
        /// Update course.
        /// </summary>
        /// <param name="course">Updation course.</param>
        /// <returns></returns>
        public async Task UpdateCourseAsync(Course course)
        {
            Update(course);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Update account courses info.
        /// </summary>
        /// <param name="updationAccountId">Updation account id.</param>
        /// <param name="updateModel">Updation model.</param>
        /// <returns></returns>
        public async Task UpdateAccountCoursesAsync(Guid updationAccountId, Course updateModel)
        {
            var coursesForUpdate = await Context.Courses
                .Where(course => course.CreatorId == updationAccountId)
                .ToListAsync();

            foreach (var course in coursesForUpdate)
                course.CreatorFullName = updateModel.CreatorFullName;

            await SaveChangesAsync();
        }

        /// <summary>
        /// Delete account courses.
        /// </summary>
        /// <param name="deletionAccountId">Deletion account id.</param>
        /// <returns></returns>
        public async Task DeleteAccountCoursesAsync(Guid deletionAccountId)
        {
            var coursesForUpdate = Context.Courses
                .Where(course => course.CreatorId == deletionAccountId);

            Context.Courses.RemoveRange(coursesForUpdate);

            await SaveChangesAsync();
        }
    }
}
