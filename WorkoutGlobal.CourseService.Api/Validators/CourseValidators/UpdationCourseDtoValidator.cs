using FluentValidation;
using WorkoutGlobal.CourseService.Api.Dto;

namespace WorkoutGlobal.CourseService.Api.Validators.CourseValidators
{
    /// <summary>
    /// Validator for UpdationCourseDto model.
    /// </summary>
    public class UpdationCourseDtoValidator : AbstractValidator<UpdationCourseDto>
    {
        /// <summary>
        /// Ctor foe validator.
        /// </summary>
        public UpdationCourseDtoValidator()
        {
            RuleFor(course => course.Name)
                .NotEmpty()
                .Length(5, 200);

            RuleFor(course => course.Description)
                .NotEmpty()
                .Length(5, 500);

            When(course => course.Logo is not null, () =>
            {
                RuleFor(course => course.Logo)
                    .NotEmpty();
            });

            RuleFor(course => course.CreatorId)
                .NotEmpty();
        }
    }
}
