﻿namespace WorkoutGlobal.CourseService.Api.Models
{
    /// <summary>
    /// Model of course lesson.
    /// </summary>
    public class CourseLesson
    {
        /// <summary>
        /// Unique identifier of course lesson.
        /// </summary>
        public Guid Id { get; set; }

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
        /// Represents course id for create relations.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Represents course model for create relations.
        /// </summary>
        public Course Course { get; set; }
    }
}
