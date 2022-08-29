namespace WorkoutGlobal.CourseService.Api.Dto
{
    /// <summary>
    /// Represents DTO of course model for PUT action.
    /// </summary>
    public class UpdationCourseDto
    {
        /// <summary>
        /// Course name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Course description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Official logotype of course.
        /// </summary>
        public byte[] Logo { get; set; }

        /// <summary>
        /// Identifier of course creator.
        /// </summary>
        public Guid CreatorId { get; set; }
    }
}
