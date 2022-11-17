using AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using System.Security.Cryptography;
using WorkoutGlobal.CourseService.Api.Dto;
using WorkoutGlobal.CourseService.Api.Validators.CourseValidators;

namespace WorkoutGlobal.CourseService.UnitTests.Validators
{
    public class CreationCourseDtoValidatorTests
    {
        private readonly CreationCourseDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task ModelState_NullProperties_ReturnValidationResult()
        {
            // arrange
            var creationCourseDto = new CreationCourseDto();

            // act
            var validationResult = await _validator.ValidateAsync(creationCourseDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(5);
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_EmptyProperties_ReturnValidationResult()
        {
            // arrange
            var creationCourseDto = _fixture.Build<CreationCourseDto>()
                .With(x => x.Name, string.Empty)
                .With(x => x.Description, string.Empty)
                .With(x => x.Logo, Array.Empty<byte>())
                .With(x => x.CreatorId, Guid.Empty)
                .With(x => x.CreationDate, default(DateTime))
                .With(x => x.CreatorFullName, string.Empty)
                .Create();

            // act
            var validationResult = await _validator.ValidateAsync(creationCourseDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(6);
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public async Task ModelState_ValidProperties_ReturnValidationResult()
        {
            // arrange
            var creationCourseDto = _fixture.Build<CreationCourseDto>()
                .With(x => x.Name, "Course name")
                .With(x => x.Description, "Course description")
                .With(x => x.Logo, RandomNumberGenerator.GetBytes(100))
                .With(x => x.CreatorId, Guid.NewGuid())
                .With(x => x.CreationDate, DateTime.Now)
                .With(x => x.CreatorFullName, "Creator")
                .Create();

            // act
            var validationResult = await _validator.ValidateAsync(creationCourseDto);

            // assert
            validationResult.Should().BeOfType(typeof(ValidationResult));
            validationResult.Should().NotBeNull();
            validationResult.Errors.Should().HaveCount(0);
            validationResult.IsValid.Should().BeTrue();
        }
    }
}
