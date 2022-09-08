namespace WorkoutGlobal.CourseService.Api.Dto
{
    /// <summary>
    /// Represents DTO of course model for GET actions.
    /// </summary>
    public class CourseDto
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
        /// Official logotype of course.
        /// </summary>
        public byte[] Logo { get; set; }

        /// <summary>
        /// Course creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
