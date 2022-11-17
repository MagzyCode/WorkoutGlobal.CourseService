using AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using WorkoutGlobal.CourseService.Api.Dto;
using WorkoutGlobal.CourseService.Api.Validators.LessonValidators;

namespace WorkoutGlobal.CourseService.UnitTests.Validators
{
    public class CreationLessonDtoValidatorTests
    {
        private readonly CreationLessonDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task ModelState_NullProperties_ReturnValidationResult()
        {
            // arrange
            var creationLessonDto = new CreationLessonDto();

            // act
            var validationResult = await _validator.ValidateAsync(creationLessonDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(3);
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_EmptyProperties_ReturnValidationResult()
        {
            // arrange
            var creationLessonDto = _fixture.Build<CreationLessonDto>()
                .With(x => x.Title, string.Empty)
                .With(x => x.SequenceNumber, 0)
                .With(x => x.Content, string.Empty)
                .With(x => x.VideoId, string.Empty)
                .With(x => x.CourseId, Guid.Empty)
                .With(x => x.VideoTitle, string.Empty)
                .With(x => x.VideoDescription, string.Empty)
                .Create();

            // act
            var validationResult = await _validator.ValidateAsync(creationLessonDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(7);
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_ValidProperties_ReturnValidationResult()
        {
            // arrange
            var creationLessonDto = _fixture.Build<CreationLessonDto>()
                .With(x => x.Title, "Lesson title")
                .With(x => x.SequenceNumber, 1)
                .With(x => x.Content, "Lesson content")
                .With(x => x.VideoId, $"5dsfkjh2r74dsjdhf3r4f")
                .With(x => x.CourseId, Guid.NewGuid())
                .With(x => x.VideoTitle, "Video title")
                .With(x => x.VideoDescription, "Video description")
                .Create();

            // act
            var validationResult = await _validator.ValidateAsync(creationLessonDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(0);
            validationResult.IsValid.Should().BeTrue();
        }
    }
}
