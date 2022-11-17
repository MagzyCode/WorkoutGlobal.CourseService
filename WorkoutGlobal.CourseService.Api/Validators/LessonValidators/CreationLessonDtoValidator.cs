﻿using FluentValidation;
using WorkoutGlobal.CourseService.Api.Dto;

namespace WorkoutGlobal.CourseService.Api.Validators.LessonValidators
{
    /// <summary>
    /// CreationLessonDto model validator.
    /// </summary>
    public class CreationLessonDtoValidator : AbstractValidator<CreationLessonDto>
    {
        /// <summary>
        /// Ctor for validator.
        /// </summary>
        public CreationLessonDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(lesson => lesson.Title)
                .NotEmpty()
                .Length(5, 500);

            RuleFor(lesson => lesson.SequenceNumber)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);

            When(lesson => lesson.Content is not null, () =>
            {
                RuleFor(lesson => lesson.Content)
                    .NotEmpty();
            });

            When(lesson => lesson.VideoId is not null, () =>
            {
                RuleFor(lesson => lesson.VideoId)
                    .NotEmpty();

                RuleFor(lesson => lesson.VideoTitle)
                    .NotEmpty();

                RuleFor(lesson => lesson.VideoDescription)
                    .NotEmpty();
            });

            RuleFor(lesson => lesson.CourseId)
                .NotEmpty();
        }
    }
}
