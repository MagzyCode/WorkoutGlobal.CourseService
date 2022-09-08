using FluentAssertions;
using WorkoutGlobal.CourseService.Api.Models;
using WorkoutGlobal.CourseService.Api.Repositories;

namespace WorkoutGlobal.CourseService.UnitTests.Repositories
{
    public class CourseRepositoryTests
    {
        private readonly CourseRepository _testRepository;

        public CourseRepositoryTests()
        {
            _testRepository = new(null, null);
        }

        [Fact]
        public async Task CreateCourseAsync_NullParam_ReturnArgumentNullException()
        {
            // arrange
            Course course = null;

            // act
            var result = async () => await _testRepository.CreateCourseAsync(course);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteCourseAsync_EmptyParam_ReturnArgumentNullException()
        {
            // arrange
            var id = default(Guid);

            // act
            var result = async () => await _testRepository.DeleteCourseAsync(id);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetAllCourseLessonsAsync_EmptyParam_ReturnArgumentNullException()
        {
            // arrange
            var id = Guid.Empty;

            // act
            var result = async () => await _testRepository.GetAllCourseLessonsAsync(id);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetCourseAsync_EmptyParam_ReturnArgumentNullException()
        {
            // arrange
            var id = Guid.Empty;

            // act
            var result = async () => await _testRepository.GetCourseAsync(id);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateCourseAsync_NullParam_ReturnArgumentNullException()
        {
            // arrange
            Course course = null;

            // act
            var result = async () => await _testRepository.UpdateCourseAsync(course);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
