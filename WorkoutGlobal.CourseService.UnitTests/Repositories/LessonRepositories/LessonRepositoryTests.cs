using FluentAssertions;
using WorkoutGlobal.CourseService.Api.Models;
using WorkoutGlobal.CourseService.Api.Repositories;

namespace WorkoutGlobal.CourseService.UnitTests.Repositories
{
    public class LessonRepositoryTests
    {
        private readonly LessonRepository _testRepository;

        public LessonRepositoryTests()
        {
            _testRepository = new(null, null);
        }

        [Fact]
        public async Task CreateLessonAsync_NullParam_ReturnArgumentNullException()
        {
            // arrange
            Lesson course = null;

            // act
            var result = async () => await _testRepository.CreateLessonAsync(course);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteLessonAsync_EmptyParam_ReturnArgumentNullException()
        {
            // arrange
            var id = default(Guid);

            // act
            var result = async () => await _testRepository.DeleteLessonAsync(id);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetLessonCourseAsync_EmptyParam_ReturnArgumentNullException()
        {
            // arrange
            var id = Guid.Empty;

            // act
            var result = async () => await _testRepository.GetLessonCourseAsync(id);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetLessonAsync_EmptyParam_ReturnArgumentNullException()
        {
            // arrange
            var id = Guid.Empty;

            // act
            var result = async () => await _testRepository.GetLessonAsync(id);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateLessonAsync_NullParam_ReturnArgumentNullException()
        {
            // arrange
            Lesson course = null;

            // act
            var result = async () => await _testRepository.UpdateLessonAsync(course);

            // assert
            await result.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
