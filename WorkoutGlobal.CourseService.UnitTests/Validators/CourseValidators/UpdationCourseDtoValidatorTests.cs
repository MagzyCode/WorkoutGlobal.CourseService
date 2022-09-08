using AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using System.Security.Cryptography;
using WorkoutGlobal.CourseService.Api.Dto;
using WorkoutGlobal.CourseService.Api.Validators.CourseValidators;

namespace WorkoutGlobal.CourseService.UnitTests.Validators
{
    public class UpdationCourseDtoValidatorTests
    {
        private readonly UpdationCourseDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task ModelState_NullProperties_ReturnValidationResult()
        {
            // arrange
            var updationCourseDto = new UpdationCourseDto();

            // act
            var validationResult = await _validator.ValidateAsync(updationCourseDto);

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
            var updationCourseDto = _fixture.Build<UpdationCourseDto>()
                .With(x => x.Name, string.Empty)
                .With(x => x.Description, string.Empty)
                .With(x => x.Logo, Array.Empty<byte>())
                .With(x => x.CreatorId, Guid.Empty)
                .Create();

            // act
            var validationResult = await _validator.ValidateAsync(updationCourseDto);

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
            var updationCourseDto = _fixture.Build<UpdationCourseDto>()
                .With(x => x.Name, "Course name")
                .With(x => x.Description, "Course description")
                .With(x => x.Logo, RandomNumberGenerator.GetBytes(100))
                .With(x => x.CreatorId, Guid.NewGuid())
                .Create();

            // act
            var validationResult = await _validator.ValidateAsync(updationCourseDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(0);
            validationResult.IsValid.Should().BeTrue();
        }
    }
}
