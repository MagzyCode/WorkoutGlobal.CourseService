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
    public class LessonControllerTests
    {
        private readonly LessonController _lessonController;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILessonRepository> _mockRepository;
        private readonly Fixture _fixture;

        public LessonControllerTests()
        {
            _fixture = new();

            _mockMapper = new Mock<IMapper>();
            _mockMapper
                .Setup(x => x.Map<LessonDto>(It.IsAny<Lesson>()))
                .Returns(new LessonDto());
            _mockMapper
                .Setup(x => x.Map<Lesson>(It.IsAny<CreationLessonDto>()))
                .Returns(new Lesson());
            _mockMapper
                .Setup(x => x.Map<Lesson>(It.IsAny<UpdationLessonDto>()))
                .Returns(new Lesson());
            _mockMapper
                .Setup(x => x.Map<CourseDto>(It.IsAny<Course>()))
                .Returns(new CourseDto());

            _mockRepository = new Mock<ILessonRepository>();
            _mockRepository
                .Setup(x => x.GetLessonAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(new Lesson());
            _mockRepository
                .Setup(x => x.CreateLessonAsync(It.IsAny<Lesson>()))
                .ReturnsAsync(Guid.Empty);
            _mockRepository
                .Setup(x => x.DeleteLessonAsync(It.IsAny<Guid>()));
            _mockRepository
                .Setup(x => x.UpdateLessonAsync(It.IsAny<Lesson>()));
            _mockRepository
                .Setup(x => x.GetLessonCourseAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Course());

            _lessonController = new(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetLesson_InvalidParam_ReturnBadRequestResult()
        {
            // arrange 
            var id = Guid.Empty;

            // act
            var result = await _lessonController.GetLesson(id);

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
        public async Task GetLesson_ModelNotFound_ReturnNotFoundResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            _mockRepository
                .Setup(x => x.GetLessonAsync(id, It.IsAny<bool>()));

            // act
            var result = await _lessonController.GetLesson(id);

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
        public async Task GetLesson_ModelExists_ReturnOkResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            // act
            var result = await _lessonController.GetLesson(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<LessonDto>();
        }

        [Fact]
        public async Task GetAllLessons_ModelsExists_ReturnOkResult()
        {
            // arrange 
            _mockRepository
                .Setup(x => x.GetAllLessonsAsync())
                .ReturnsAsync(Enumerable.Empty<Lesson>());
            _mockMapper
                .Setup(x => x.Map<IEnumerable<LessonDto>>(It.IsAny<IEnumerable<Lesson>>()))
                .Returns(new List<LessonDto>());

            // act
            var result = await _lessonController.GetAllLessons();

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<List<LessonDto>>();
        }

        [Fact]
        public async Task CreateLesson_ModelValid_ReturnCreatedResult()
        {
            // arrange 
            var creationCourseDto = _fixture.Create<CreationLessonDto>();

            // act
            var result = await _lessonController.CreateLesson(creationCourseDto);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedResult>();

            var createdResult = result.As<CreatedResult>();
            createdResult.Value.Should().NotBeNull();
            createdResult.Value.Should().BeOfType<Guid>();
            createdResult.Value.Should().Be(Guid.Empty);
            createdResult.Location.Should().NotBeNullOrEmpty();
            createdResult.Location.Should().BeOfType<string>();
            createdResult.Location.Should().Be($"api/lessons/{Guid.Empty}");
        }

        [Fact]
        public async Task DeleteLesson_InvalidParam_ReturnBadRequestResult()
        {
            // arrange 
            var id = Guid.Empty;

            // act
            var result = await _lessonController.DeleteLesson(id);

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
        public async Task DeleteLesson_ModelNotFound_ReturnNotFoundResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            _mockRepository
                .Setup(x => x.GetLessonAsync(id, It.IsAny<bool>()));

            // act
            var result = await _lessonController.DeleteLesson(id);

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
        public async Task DeleteLesson_ModelExists_ReturnNoContentResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            // act
            var result = await _lessonController.DeleteLesson(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
            result.As<NoContentResult>().StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task UpdateLesson_InvalidIdParam_ReturnBadRequestResult()
        {
            // arrange 
            var id = Guid.Empty;

            // act
            var result = await _lessonController.UpdateLesson(id, null);

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
        public async Task UpdateLesson_ModelNotFound_ReturnNotFoundResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            _mockRepository
                .Setup(x => x.GetLessonAsync(id, It.IsAny<bool>()));

            // act
            var result = await _lessonController.UpdateLesson(id, null);

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
        public async Task UpdateLesson_ModelExists_ReturnNoContentResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            var updationLessonDto = _fixture.Create<UpdationLessonDto>();

            // act
            var result = await _lessonController.UpdateLesson(id, updationLessonDto);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
            result.As<NoContentResult>().StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task GetLessonCourse_InvalidParam_ReturnBadRequestResult()
        {
            // arrange 
            var id = Guid.Empty;

            // act
            var result = await _lessonController.GetLessonCourse(id);

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
        public async Task GetLessonCourse_ModelNotFound_ReturnNotFoundResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            _mockRepository
                .Setup(x => x.GetLessonAsync(id, It.IsAny<bool>()));

            // act
            var result = await _lessonController.GetLessonCourse(id);

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
        public async Task GetLessonCourse_ModelExists_ReturnOkResult()
        {
            // arrange 
            var id = Guid.NewGuid();

            // act
            var result = await _lessonController.GetLessonCourse(id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeOfType<CourseDto>();
        }
    }
}
