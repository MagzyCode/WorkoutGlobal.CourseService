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
        /// <returns>Returns course by given id.</returns>
        public Task<Course> GetCourse(Guid id);

        /// <summary>
        /// Get all courses.
        /// </summary>
        /// <returns>Returns collection of all courses.</returns>
        public Task<IEnumerable<Course>> GetAllCourses();

        /// <summary>
        /// Create course.
        /// </summary>
        /// <param name="creationModel">Creation model.</param>
        /// <returns>Return generated id for new model.</returns>
        public Task<Guid> CreateCourse(Course creationModel);

        /// <summary>
        /// Delete model by id.
        /// </summary>
        /// <param name="id">Course id.</param>
        /// <returns></returns>
        public Task DeleteCourse(Guid id);

        /// <summary>
        /// Update course.
        /// </summary>
        /// <param name="course">Updation course.</param>
        /// <returns></returns>
        public Task UpdateCourse(Course course);

        /// <summary>
        /// Get all lesson by course id.
        /// </summary>
        /// <param name="courseId">Course id.</param>
        /// <returns>Returns collection of course lessons.</returns>
        public Task<IEnumerable<Lesson>> GetAllCourseLessons(Guid courseId);
    }
}
