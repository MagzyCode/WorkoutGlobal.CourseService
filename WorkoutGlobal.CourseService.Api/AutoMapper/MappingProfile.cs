using AutoMapper;
using WorkoutGlobal.CourseService.Api.Dto;
using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.Api.AutoMapper
{
    /// <summary>
    /// Class for configure mapping rules via AutoMapper library.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Ctor for set mapping rules for models and DTOs.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<CreationCourseDto, Course>();
            CreateMap<UpdationCourseDto, Course>();

            CreateMap<Lesson, LessonDto>();
            CreateMap<CreationCourseDto, Lesson>();
            CreateMap<UpdationCourseDto, Lesson>();
        }
    }
}
