using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorkoutGlobal.CourseService.Api.Contracts;
using WorkoutGlobal.CourseService.Api.Controllers;
using WorkoutGlobal.CourseService.Api.Dto;
using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.UnitTests.Controllers
{
    public class CourseControllerTests
    {
        private readonly CourseController _courseController;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICourseRepository> _mockRepository;
        private readonly Fixture _fixture;

        public CourseControllerTests()
        {
            _fixture = new();

            _mockMapper = new Mock<IMapper>();
            _mockMapper
                .Setup(x => x.Map<CourseDto>(It.IsAny<Course>()))
                .Returns(new CourseDto());
            _mockMapper
                .Setup(x => x.Map<Course>(It.IsAny<CreationCourseDto>()))
                .Returns(new Course());
            _mockMapper
                .Setup(x => x.Map<Course>(It.IsAny<UpdationCourseDto>()))
                .Returns(new Course());
            _mockMapper
                .Setup(x => x.Map<IEnumerable<LessonDto>>(It.IsAny<IEnumerable<Lesson>>()))
                .Returns(Enumerable.Empty<LessonDto>());

            _mockRepository = new Mock<ICourseRepository>();
            _mockRepository
                .Setup(x => x.GetCourseAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(new Course());
            _mockRepository
                .Setup(x => x.CreateCourseAsync(It.IsAny<Course>()))
                .ReturnsAsync(Guid.Empty);
            _mockRepository
                .Setup(x => x.DeleteCourseAsync(It.IsAny<Guid>()));
            _mockRepository
                .Setup(x => x.UpdateCourseAsync(It.IsAny<Course>()));
            _mockRepository
                .Setup(x => x.GetAllCourseLessonsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(Enumerable.Empty<Lesson>());

            _courseController = new(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetCourse_InvalidParam_ReturnBadRequestResult()
        {
            // arrange 
            var id = Guid.Empty;

            // act
            var result = await _courseController.GetCourse(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Id is empty.");
            error.Details.Should().Be("Searchable model cannot be found because id is empty.");
        }

        [Fact]
        public async Task GetCourse_ModelNotFound_ReturnNotFoundResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            _mockRepository
                .Setup(x => x.GetCourseAsync(id, true));

            // act
            var result = await _courseController.GetCourse(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.As<NotFoundObjectResult>();
            notFoundResult.Value.Should().NotBeNull();
            notFoundResult.Value.Should().BeOfType<ErrorDetails>();

            var error = notFoundResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            error.Message.Should().Be("Model not found.");
            error.Details.Should().Be("Cannot find model with given id.");
        }

        [Fact]
        public async Task GetCourse_ModelExists_ReturnOkResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            // act
            var result = await _courseController.GetCourse(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<CourseDto>();
        }

        [Fact]
        public async Task GetAllCourses_ModelsExists_ReturnOkResult()
        {
            // arrange 
            _mockRepository
                .Setup(x => x.GetAllCoursesAsync())
                .ReturnsAsync(Enumerable.Empty<Course>());
            _mockMapper
                .Setup(x => x.Map<IEnumerable<CourseDto>>(It.IsAny<IEnumerable<Course>>()))
                .Returns(new List<CourseDto>());

            // act
            var result = await _courseController.GetAllCourses();

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<List<CourseDto>>();
        }

        [Fact]
        public async Task CreateCourse_ModelValid_ReturnCreatedResult()
        {
            // arrange 
            var creationCourseDto = _fixture.Create<CreationCourseDto>();

            // act
            var result = await _courseController.CreateCourse(creationCourseDto);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedResult>();

            var createdResult = result.As<CreatedResult>();
            createdResult.Value.Should().NotBeNull();
            createdResult.Value.Should().BeOfType<Guid>();
            createdResult.Value.Should().Be(Guid.Empty);
            createdResult.Location.Should().NotBeNullOrEmpty();
            createdResult.Location.Should().BeOfType<string>();
            createdResult.Location.Should().Be($"api/courses/{Guid.Empty}");
        }

        [Fact]
        public async Task DeleteCourse_InvalidParam_ReturnBadRequestResult()
        {
            // arrange 
            var id = Guid.Empty;

            // act
            var result = await _courseController.DeleteCourse(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Id is empty.");
            error.Details.Should().Be("Searchable model cannot be found because id is empty.");
        }

        [Fact]
        public async Task DeleteCourse_ModelNotFound_ReturnNotFoundResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            _mockRepository
                .Setup(x => x.GetCourseAsync(id, It.IsAny<bool>()));

            // act
            var result = await _courseController.DeleteCourse(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.As<NotFoundObjectResult>();
            notFoundResult.Value.Should().NotBeNull();
            notFoundResult.Value.Should().BeOfType<ErrorDetails>();

            var error = notFoundResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            error.Message.Should().Be("Model not found.");
            error.Details.Should().Be("Cannot find model with given id.");
        }

        [Fact]
        public async Task DeleteCourse_ModelExists_ReturnNoContentResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            // act
            var result = await _courseController.DeleteCourse(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
            result.As<NoContentResult>().StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task UpdateCourse_InvalidIdParam_ReturnBadRequestResult()
        {
            // arrange 
            var id = Guid.Empty;

            // act
            var result = await _courseController.UpdateCourse(id, null);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Id is empty.");
            error.Details.Should().Be("Searchable model cannot be found because id is empty.");
        }

        [Fact]
        public async Task UpdateCourse_ModelNotFound_ReturnNotFoundResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            _mockRepository
                .Setup(x => x.GetCourseAsync(id, It.IsAny<bool>()));

            // act
            var result = await _courseController.UpdateCourse(id, null);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.As<NotFoundObjectResult>();
            notFoundResult.Value.Should().NotBeNull();
            notFoundResult.Value.Should().BeOfType<ErrorDetails>();

            var error = notFoundResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            error.Message.Should().Be("Model not found.");
            error.Details.Should().Be("Cannot find model with given id.");
        }

        [Fact]
        public async Task UpdateCourse_ModelExists_ReturnNoContentResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            var updationCourseDto = _fixture.Create<UpdationCourseDto>();

            // act
            var result = await _courseController.UpdateCourse(id, updationCourseDto);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
            result.As<NoContentResult>().StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task GetCourseLessons_InvalidParam_ReturnBadRequestResult()
        {
            // arrange 
            var id = Guid.Empty;

            // act
            var result = await _courseController.GetCourseLessons(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();

            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeOfType<ErrorDetails>();

            var error = badRequestResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            error.Message.Should().Be("Id is empty.");
            error.Details.Should().Be("Searchable model cannot be found because id is empty.");
        }

        [Fact]
        public async Task GetCourseLessons_ModelNotFound_ReturnNotFoundResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            _mockRepository
                .Setup(x => x.GetCourseAsync(id, It.IsAny<bool>()));

            // act
            var result = await _courseController.GetCourseLessons(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();

            var notFoundResult = result.As<NotFoundObjectResult>();
            notFoundResult.Value.Should().NotBeNull();
            notFoundResult.Value.Should().BeOfType<ErrorDetails>();

            var error = notFoundResult.Value.As<ErrorDetails>();
            error.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            error.Message.Should().Be("Model not found.");
            error.Details.Should().Be("Cannot find model with given id.");
        }

        [Fact]
        public async Task GetCourseLessons_ModelExists_ReturnOkResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            _mockMapper
                .Setup(x => x.Map<IEnumerable<LessonDto>>(It.IsAny<IEnumerable<Lesson>>()))
                .Returns(new List<LessonDto>());

            // act
            var result = await _courseController.GetCourseLessons(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<List<LessonDto>>();
        }
    }
}
