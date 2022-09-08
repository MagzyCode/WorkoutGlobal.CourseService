using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.Api.Contracts
{
    /// <summary>
    /// Base interface for course repository.
    /// </summary>
    public interface ICourseRepository
    {
        /// <summary>
        /// Get course by id.
        /// </summary>
        /// <param name="id">Course id.</param>
        /// <param name="trackChanges">Tracking model state.</param>
        /// <returns>Returns course by given id.</returns>
        public Task<Course> GetCourseAsync(Guid id, bool trackChanges = true);

        /// <summary>
        /// Get all courses.
        /// </summary>
        /// <returns>Returns collection of all courses.</returns>
        public Task<IEnumerable<Course>> GetAllCoursesAsync();

        /// <summary>
        /// Create course.
        /// </summary>
        /// <param name="creationModel">Creation model.</param>
        /// <returns>Return generated id for new model.</returns>
        public Task<Guid> CreateCourseAsync(Course creationModel);

        /// <summary>
        /// Delete model by id.
        /// </summary>
        /// <param name="id">Course id.</param>
        /// <returns></returns>
        public Task DeleteCourseAsync(Guid id);

        /// <summary>
        /// Update course.
        /// </summary>
        /// <param name="course">Updation course.</param>
        /// <returns></returns>
        public Task UpdateCourseAsync(Course course);

        /// <summary>
        /// Get all lesson by course id.
        /// </summary>
        /// <param name="courseId">Course id.</param>
        /// <returns>Returns collection of course lessons.</returns>
        public Task<IEnumerable<Lesson>> GetAllCourseLessonsAsync(Guid courseId);
    }
}
