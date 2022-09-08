using FluentValidation;
using WorkoutGlobal.CourseService.Api.Dto;

namespace WorkoutGlobal.CourseService.Api.Validators.CourseValidators
{
    /// <summary>
    /// Validator for CreationCourseDto model.
    /// </summary>
    public class CreationCourseDtoValidator : AbstractValidator<CreationCourseDto>
    {
        /// <summary>
        /// Ctor for validator.
        /// </summary>
        public CreationCourseDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

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

            RuleFor(course => course.CreationDate)
                .NotEmpty();
        }
    }
}
