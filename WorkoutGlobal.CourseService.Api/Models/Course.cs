using WorkoutGlobal.CourseService.Api.Contracts;

namespace WorkoutGlobal.CourseService.Api.Models
{
    /// <summary>
    /// Model of courses.
    /// </summary>
    public class Course : IModel<Guid>
    {
        /// <summary>
        /// Unique identifier of model.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Course name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Course description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Identifier of course creator.
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// Creator full name.
        /// </summary>
        public string CreatorFullName { get; set; }

        /// <summary>
        /// Official logotype of course.
        /// </summary>
        public byte[] Logo { get; set; }

        /// <summary>
        /// Course creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Represents collection of course lessons.
        /// </summary>
        public ICollection<Lesson> CourseLessons { get; set; }
    }
}
