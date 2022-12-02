using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.Api.Contracts
{
    /// <summary>
    /// Represents basic interface for lesson repository.
    /// </summary>
    public interface ILessonRepository
    {
        /// <summary>
        /// Get lesson by id.
        /// </summary>
        /// <param name="id">Lesson id.</param>
        /// <param name="trackChanges">Tracking changes state.</param>
        /// <returns>Returns lesson by given id.</returns>
        public Task<Lesson> GetLessonAsync(Guid id, bool trackChanges = true);

        /// <summary>
        /// Get all lessons.
        /// </summary>
        /// <returns>Returns collection of all lessons.</returns>
        public Task<IEnumerable<Lesson>> GetAllLessonsAsync();

        /// <summary>
        /// Create lesson.
        /// </summary>
        /// <param name="courseLesson">Creation lesson.</param>
        /// <returns>Return generated id for new lesson.</returns>
        public Task<Guid> CreateLessonAsync(Lesson courseLesson);

        /// <summary>
        /// Delete lesson.
        /// </summary>
        /// <param name="id">Lesson id.</param>
        /// <returns></returns>
        public Task DeleteLessonAsync(Guid id);

        /// <summary>
        /// Update lesson.
        /// </summary>
        /// <param name="courseLesson">Updation model.</param>
        /// <returns></returns>
        public Task UpdateLessonAsync(Lesson courseLesson);

        /// <summary>
        /// Get lesson course by lesson identifier.
        /// </summary>
        /// <param name="lessonId">Lesson id.</param>
        /// <returns>Return course of lesson.</returns>
        public Task<Course> GetLessonCourseAsync(Guid lessonId);

        /// <summary>
        /// Update change video info in lessons.
        /// </summary>
        /// <param name="updationVideoId">Updation video id.</param>
        /// <param name="updateModel">Updation model.</param>
        /// <returns></returns>
        public Task UpdateLessonsVideoInfoAsync(string updationVideoId, Lesson updateModel);

        /// <summary>
        /// Delete account courses.
        /// </summary>
        /// <param name="deletionVideoId">Deletion account id.</param>
        /// <returns></returns>
        public Task DeleteLessonsVideoInfoAsync(string deletionVideoId);
    }
}
