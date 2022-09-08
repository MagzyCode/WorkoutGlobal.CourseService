using AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using WorkoutGlobal.CourseService.Api.Dto;
using WorkoutGlobal.CourseService.Api.Validators.LessonValidators;

namespace WorkoutGlobal.CourseService.UnitTests.Validators
{
    public class UpdationLessonDtoValidatorTests
    {
        private readonly UpdationLessonDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task ModelState_NullProperties_ReturnValidationResult()
        {
            // arrange
            var updationLessonDto = new UpdationLessonDto();

            // act
            var validationResult = await _validator.ValidateAsync(updationLessonDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(2);
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_EmptyProperties_ReturnValidationResult()
        {
            // arrange
            var updationLessonDto = _fixture.Build<UpdationLessonDto>()
                .With(x => x.Title, string.Empty)
                .With(x => x.SequenceNumber, 0)
                .With(x => x.Content, string.Empty)
                .With(x => x.VideoId, string.Empty)
                .Create();

            // act
            var validationResult = await _validator.ValidateAsync(updationLessonDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(4);
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_ValidProperties_ReturnValidationResult()
        {
            // arrange
            var updationLessonDto = _fixture.Build<UpdationLessonDto>()
                .With(x => x.Title, "Lesson title")
                .With(x => x.SequenceNumber, 1)
                .With(x => x.Content, "Lesson content")
                .With(x => x.VideoId, $"5dsfkjh2r74dsjdhf3r4f")
                .Create();

            // act
            var validationResult = await _validator.ValidateAsync(updationLessonDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(0);
            validationResult.IsValid.Should().BeTrue();
        }
    }
}
