using AutoFixture;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Net;
using WorkoutGlobal.CourseService.Api.Dto;
using WorkoutGlobal.CourseService.Api.Models;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace WorkoutGlobal.CourseService.IntegrationTests.Controllers
{
    public class CourseControllerIntegrationTests : IAsyncLifetime
    {
        private readonly AppTestConnection<Guid> _appTestConnection;
        private readonly Fixture _fixture = new();
        private CreationCourseDto _creationModel;

        public CourseControllerIntegrationTests()
        {
            _appTestConnection = new();
        }

        public async Task InitializeAsync()
        {
            _appTestConnection.PurgeList.Clear();

            var image = await File.ReadAllBytesAsync(_appTestConnection.Configuration["TestImagePath"]);

            _creationModel = _fixture.Build<CreationCourseDto>()
                .With(x => x.Name, "Test course")
                .With(x => x.Description, "Test description")
                .With(x => x.CreatorId, Guid.NewGuid())
                .With(x => x.Logo, image)
                .With(x => x.CreationDate, DateTime.UtcNow)
                .Create();
        }

        public async Task DisposeAsync()
        {
            foreach (var id in _appTestConnection.PurgeList)
                _ = await _appTestConnection.AppClient.DeleteAsync($"api/courses/purge/{id}");

            await Task.CompletedTask;
        }

        [Fact]
        public async Task CreateCourse_NullParam_ReturnErrorDetails()
        {
            // arrange
            _creationModel = null;

            // act
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _creationModel);
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
        public async Task CreateCourse_InvalidModel_ReturnErrorDetails()
        {
            // arrange
            _creationModel = _fixture.Build<CreationCourseDto>()
                .OmitAutoProperties()
                .Create();

            // act
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _creationModel);
            var result = await createResponse.Content.ReadFromJsonAsync<ErrorDetails>();

            // assert
            createResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            result.Should().NotBeNull();
            result.Should().BeOfType<ErrorDetails>();
            result.Message.Should().Be("Dto model isn't valid.");
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateCourse_ValidState_ReturnCreatedResult()
        {
            // arrange
            var createUri = "api/courses";

            // act
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync(createUri, _creationModel);
            var result = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add(result);

            // assert
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetCourse_ModelExists_ReturnOkResult()
        {
            // arrange
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add(createdId);

            // act
            var getResponse = await _appTestConnection.AppClient.GetAsync($"api/courses/{createdId}");
            var result = await getResponse.Content.ReadFromJsonAsync<CourseDto>();

            // assert
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponse.Content.Should().NotBeNull();

            result.Should().NotBeNull();
            result.Should().BeOfType<CourseDto>();
            result.Id.Should().Be(createdId);
            result.Name.Should().Be("Test course");
            result.Description.Should().Be("Test description");
            result.CreatorId.Should().NotBeEmpty();
            result.Logo.Should().NotBeEmpty();
            result.CreationDate.Should().BeAfter(1.September(2022));
        }

        [Fact]
        public async Task GetAllCourses_ModelsExists_ReturnOkResult()
        {
            // arrange
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add(createdId);

            // act
            var getResponse = await _appTestConnection.AppClient.GetAsync("api/courses");
            var result = await getResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            // assert
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            getResponse.Content.Should().NotBeNull();

            result.Should().NotBeNull();
            result.Should().BeOfType<List<CourseDto>>();
            result.Count.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task DeleteCourse_ModelsExists_ReturnNoContentResult()
        {
            // arrange
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add(createdId);

            // act
            var deleteResponse = await _appTestConnection.AppClient.DeleteAsync($"api/courses/{createdId}");
            var getResponse = await _appTestConnection.AppClient.GetAsync($"api/courses/{createdId}");
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
        public async Task UpdateCourse_NullModel_ReturnBadRequestResult()
        {
            // arrange
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add(createdId);
            _creationModel = null;

            // act
            var updateResponse = await _appTestConnection.AppClient.PutAsJsonAsync($"api/courses/{createdId}", _creationModel);
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
        public async Task UpdateCourse_InvalidModel_ReturnBadRequestResult()
        {
            // arrange
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add(createdId);
            _creationModel = _fixture.Build<CreationCourseDto>()
                .OmitAutoProperties()
                .Create();

            // act
            var updateResponse = await _appTestConnection.AppClient.PutAsJsonAsync($"api/courses/{createdId}", _creationModel);
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
        public async Task UpdateCourse_ModelExists_ReturnBadRequestResult()
        {
            // arrange
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add(createdId);

            var updateImage = await File.ReadAllBytesAsync(_appTestConnection.Configuration["UpdateImagePath"]);
            var date = DateTime.UtcNow;
            var guid = Guid.NewGuid();
            var updationModel = _fixture.Build<UpdationCourseDto>()
                .With(x => x.Name, "Update test course")
                .With(x => x.Description, "Update test description")
                .With(x => x.CreatorId, guid)
                .With(x => x.Logo, updateImage)
                .Create();

            // act
            var updateResponse = await _appTestConnection.AppClient.PutAsJsonAsync($"api/courses/{createdId}", updationModel);
            var getResponse = await _appTestConnection.AppClient.GetAsync($"api/courses/{createdId}");
            var result = await getResponse.Content.ReadFromJsonAsync<CourseDto>();

            // assert
            updateResponse.Should().NotBeNull();
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            result.Should().NotBeNull();
            result.Should().BeOfType<CourseDto>();
            result.Id.Should().NotBeEmpty();
            result.Name.Should().Be("Update test course");
            result.Description.Should().Be("Update test description");
            result.CreatorId.Should().Be(guid);
            result.Logo.Should().BeEquivalentTo(updateImage);
        }

        [Fact]
        public async Task GetCourseLessons_ModelExists_ReturnOkResult()
        {
            // arrange
            var createResponse = await _appTestConnection.AppClient.PostAsJsonAsync("api/courses", _creationModel);
            var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            _appTestConnection.PurgeList.Add(createdId);

            // act
            var getResponse = await _appTestConnection.AppClient.GetAsync($"api/courses/{createdId}/lessons");
            var result = await getResponse.Content.ReadFromJsonAsync<List<LessonDto>>();

            // assert
            getResponse.Should().NotBeNull();
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
            result.Should().BeOfType<List<LessonDto>>();
        }
    }
}
