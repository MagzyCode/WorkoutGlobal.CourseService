using AutoFixture;
using FluentAssertions.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Net;
using WorkoutGlobal.CourseService.Api.Dto;
using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.IntegrationTests.Controllers
{
    public class LessonControllerIntegrationTests : IAsyncLifetime
    {
        private readonly AppTestConnection<(Guid, string)> _appTestConnection;
        private readonly Fixture _fixture = new();
        private CreationLessonDto _creationModel;
        private CreationCourseDto _courseModel;

        public LessonControllerIntegrationTests()
        {
            _appTestConnection = new();
        }

        public async Task InitializeAsync()
        {
            _appTestConnection.PurgeList.Clear();

            var image = await File.ReadAllBytesAsync(_appTestConnection.Configuration["TestImagePath"]);

            _courseModel = _fixture.Build<CreationCourseDto>()
                .With(x => x.Name, "Test course")
                .With(x => x.Description, "Test description")
                .With(x => x.CreatorId, Guid.NewGuid())
                .With(x => x.Logo, image)
                .With(x => x.CreationDate, DateTime.UtcNow)
                .Create();
        }

        private CreationLessonDto CreateLesson(Guid courseId)
        {
            return _fixture.Build<CreationLessonDto>()
                .With(x => x.Title, "Test title")
                .With(x => x.SequenceNumber, 1)
                .With(x => x.Content, "Lesson test content")
                .With(x => x.VideoId, "507f1f77bcf86cd799439011")
                .With(x => x.CourseId, courseId)
                .Create();
        }

        public async Task DisposeAsync()
        {
            foreach (var (id, table) in _appTestConnection.PurgeList)
                _ = await _appTestConnection.AppClient.DeleteAsync($"api/{table}/purge/{id}");

            await Task.CompletedTask;
        }

        [Fact]
        public async Task CreateLesson_NullParam_ReturnErrorDetails()
        {
            // arrange
            _creationModel = null;

            // act
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/lessons", _creationModel);
            var result = await createResponse.Content.ReadFromJsonAsync<ErrorDetails>();

            // assert
            createResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Should().BeOfType<ErrorDetails>();
            result.Message.Should().Be("Incoming DTO model is null.");
            result.Details.Should().Be("Incoming DTO model not contain any value.");
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateLesson_InvalidModel_ReturnErrorDetails()
        {
            // arrange
            _creationModel = _fixture.Build<CreationLessonDto>()
                .OmitAutoProperties()
                .Create();

            // act
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/lessons", _creationModel);
            var result = await createResponse.Content.ReadFromJsonAsync<ErrorDetails>();

            // assert
            createResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Should().BeOfType<ErrorDetails>();
            result.Message.Should().Be("Dto model isn't valid.");
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateLesson_ValidState_ReturnCreatedResult()
        {
            // arrange
            var createCourseResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _courseModel);
            var createdCourseId = await createCourseResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdCourseId, "courses"));
            _creationModel = CreateLesson(createdCourseId);

            // act
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/lessons", _creationModel);
            var result = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((result, "lessons"));

            // assert
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetLesson_ModelExists_ReturnOkResult()
        {
            // arrange
            var createCourseResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _courseModel);
            var createdCourseId = await createCourseResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdCourseId, "courses"));

            _creationModel = CreateLesson(createdCourseId);
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/lessons", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdId, "lessons"));

            // act
            var getResponse = await _appTestConnection.AppClient.GetAsync($"api/lessons/{createdId}");
            var result = await getResponse.Content.ReadFromJsonAsync<LessonDto>();

            // assert
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponse.Content.Should().NotBeNull();

            result.Should().NotBeNull();
            result.Should().BeOfType<LessonDto>();
            result.Id.Should().Be(createdId);
            result.Title.Should().Be("Test title");
            result.SequenceNumber.Should().Be(1);
            result.Content.Should().Be("Lesson test content");
            result.VideoId.Should().Be("507f1f77bcf86cd799439011");
            result.CourseId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAllLessons_ModelsExists_ReturnOkResult()
        {
            // arrange
            var createCourseResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _courseModel);
            var createdCourseId = await createCourseResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdCourseId, "courses"));

            _creationModel = CreateLesson(createdCourseId);
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/lessons", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdId, "lessons"));

            // act
            var getResponse = await _appTestConnection.AppClient.GetAsync("api/lessons");
            var result = await getResponse.Content.ReadFromJsonAsync<List<LessonDto>>();

            // assert
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponse.Content.Should().NotBeNull();

            result.Should().NotBeNull();
            result.Should().BeOfType<List<LessonDto>>();
            result.Count.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task DeleteLesson_ModelsExists_ReturnNoContentResult()
        {
            // arrange
            var createCourseResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _courseModel);
            var createdCourseId = await createCourseResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdCourseId, "courses"));

            _creationModel = CreateLesson(createdCourseId);
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/lessons", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdId, "lessons"));

            // act
            var deleteResponse = await _appTestConnection.AppClient.DeleteAsync($"api/lessons/{createdId}");
            var getResponse = await _appTestConnection.AppClient.GetAsync($"api/lessons/{createdId}");
            var getResult = await getResponse.Content.ReadFromJsonAsync<ErrorDetails>();

            // assert
            deleteResponse.Should().NotBeNull();
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            getResult.Should().NotBeNull();
            getResult.Should().BeOfType<ErrorDetails>();
            getResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            getResult.Message.Should().Be("Model not found.");
            getResult.Details.Should().Be("Cannot find model with given id.");
        }

        [Fact]
        public async Task UpdateLesson_NullModel_ReturnBadRequestResult()
        {
            // arrange
            var createCourseResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _courseModel);
            var createdCourseId = await createCourseResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdCourseId, "courses"));

            _creationModel = CreateLesson(createdCourseId);
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/lessons", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdId, "lessons"));

            _creationModel = null;

            // act
            var updateResponse = await _appTestConnection.AppClient.PutAsJsonAsync($"api/lessons/{createdId}", _creationModel);
            var result = await updateResponse.Content.ReadFromJsonAsync<ErrorDetails>();

            // assert
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Should().BeOfType<ErrorDetails>();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Message.Should().Be("Incoming DTO model is null.");
            result.Details.Should().Be("Incoming DTO model not contain any value.");
        }

        [Fact]
        public async Task UpdateLesson_InvalidModel_ReturnBadRequestResult()
        {
            // arrange
            var createCourseResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _courseModel);
            var createdCourseId = await createCourseResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdCourseId, "courses"));

            _creationModel = CreateLesson(createdCourseId);
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/lessons", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();

            _creationModel = _fixture.Build<CreationLessonDto>()
                .OmitAutoProperties()
                .Create();

            // act
            var updateResponse = await _appTestConnection.AppClient.PutAsJsonAsync($"api/lessons/{createdId}", _creationModel);
            var result = await updateResponse.Content.ReadFromJsonAsync<ErrorDetails>();

            // assert
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Should().BeOfType<ErrorDetails>();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Message.Should().Be("Dto model isn't valid.");
            result.Details.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task UpdateLesson_ModelExists_ReturnBadRequestResult()
        {
            // arrange
            var createCourseResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _courseModel);
            var createdCourseId = await createCourseResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdCourseId, "courses"));

            _creationModel = CreateLesson(createdCourseId);
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/lessons", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();

            var guid = Guid.NewGuid();
            var updationModel = _fixture.Build<UpdationLessonDto>()
                .With(x => x.Title, "Update test title")
                .With(x => x.SequenceNumber, 2)
                .With(x => x.Content, "Update lesson content")
                .With(x => x.VideoId, "607f1f77bcf86cd799439011")
                .Create();

            // act
            var updateResponse = await _appTestConnection.AppClient.PutAsJsonAsync($"api/lessons/{createdId}", updationModel);
            var getResponse = await _appTestConnection.AppClient.GetAsync($"api/lessons/{createdId}");
            var result = await getResponse.Content.ReadFromJsonAsync<LessonDto>();

            // assert
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            result.Should().NotBeNull();
            result.Should().BeOfType<LessonDto>();
            result.Id.Should().NotBeEmpty();
            result.Title.Should().Be("Update test title");
            result.SequenceNumber.Should().Be(2);
            result.Content.Should().Be("Update lesson content");
            result.VideoId.Should().Be("607f1f77bcf86cd799439011");
        }

        [Fact]
        public async Task GetLessonCourse_ModelExists_ReturnOkResult()
        {
            // arrange
            var createCourseResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _courseModel);
            var createdCourseId = await createCourseResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdCourseId, "courses"));

            _creationModel = CreateLesson(createdCourseId);
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/lessons", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add((createdId, "lessons"));

            // act
            var getResponse = await _appTestConnection.AppClient.GetAsync($"api/lessons/{createdId}/course");
            var result = await getResponse.Content.ReadFromJsonAsync<CourseDto>();

            // assert
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
            result.Should().BeOfType<CourseDto>();
        }
    }
}
