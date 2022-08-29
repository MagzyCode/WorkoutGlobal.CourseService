using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.Api.Contracts
{
    public interface ILessonRepository
    {
        /// <summary>
        /// Get lesson by id.
        /// </summary>
        /// <param name="id">Lesson id.</param>
        /// <returns>Returns lesson by given id.</returns>
        public Task<Lesson> GetLesson(Guid id);

        /// <summary>
        /// Get all lessons.
        /// </summary>
        /// <returns>Returns collection of all lessons.</returns>
        public Task<IEnumerable<Lesson>> GetAllLessons();

        /// <summary>
        /// Create lesson.
        /// </summary>
        /// <param name="courseLesson">Creation lesson.</param>
        /// <returns>Return generated id for new lesson.</returns>
        public Task<Guid> CreateLesson(Lesson courseLesson);

        /// <summary>
        /// Delete lesson.
        /// </summary>
        /// <param name="id">Lesson id.</param>
        /// <returns></returns>
        public Task DeleteLesson(Guid id);

        /// <summary>
        /// Update lesson.
        /// </summary>
        /// <param name="courseLesson">Updation model.</param>
        /// <returns></returns>
        public Task UpdateLesson(Lesson courseLesson);

        /// <summary>
        /// Get lesson course by lesson identifier.
        /// </summary>
        /// <param name="lessonId">Lesson id.</param>
        /// <returns>Return course of lesson.</returns>
        public Task<Course> GetLessonCourse(Guid lessonId);
    }
}
