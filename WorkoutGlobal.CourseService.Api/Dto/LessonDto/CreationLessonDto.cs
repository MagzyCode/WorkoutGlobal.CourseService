﻿namespace WorkoutGlobal.CourseService.Api.Dto
{
    /// <summary>
    /// Represents DTO of lesson model for POST action.
    /// </summary>
    public class CreationLessonDto
    {
        /// <summary>
        /// Lesson title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Represents number of lesson in course.
        /// </summary>
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Content of lesson.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Identifier of video for course lesson.
        /// </summary>
        public string VideoId { get; set; }

        /// <summary>
        /// Course video title.
        /// </summary>
        public string VideoTitle { get; set; }

        /// <summary>
        /// Course video description.
        /// </summary>
        public string VideoDescription { get; set; }

        /// <summary>
        /// Represents course id for create relations.
        /// </summary>
        public Guid CourseId { get; set; }
    }
}
