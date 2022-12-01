using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WorkoutGlobal.CourseService.Api.Contracts;
using WorkoutGlobal.CourseService.Api.Dto;
using WorkoutGlobal.CourseService.Api.Filters.ActionFilters;
using WorkoutGlobal.CourseService.Api.Models;
using WorkoutGlobal.CourseService.Api.Repositories;

namespace WorkoutGlobal.CourseService.Api.Controllers
{
    /// <summary>
    /// Represents lesson controller.
    /// </summary>
    [Route("api/lessons")]
    [ApiController]
    [Produces("application/json")]
    public class LessonController : ControllerBase
    {
        private IMapper _mapper;
        private ILessonRepository _lessonRepository;

        /// <summary>
        /// Ctor for lesson controller.
        /// </summary>
        /// <param name="lessonRepository">Lesson repository instanse.</param>
        /// <param name="mapper">AutoMapper instanse.</param>
        public LessonController(
            ILessonRepository lessonRepository,
            IMapper mapper)
        {
            Mapper = mapper;
            LessonRepository = lessonRepository;
        }

        /// <summary>
        /// AutoMapper property.
        /// </summary>
        public IMapper Mapper
        {
            get => _mapper;
            set => _mapper = value
                ?? throw new NullReferenceException("AutoMapper instanse cannot be null.");
        }

        /// <summary>
        /// Lesson repository instanse.
        /// </summary>
        public ILessonRepository LessonRepository
        {
            get => _lessonRepository;
            set => _lessonRepository = value
                ?? throw new NullReferenceException("Lesson repository instanse cannot be null.");
        }

        /// <summary>
        /// Get lesson by id.
        /// </summary>
        /// <param name="id">Lesson id.</param>
        /// <returns>Returns lesson by given id.</returns>
        /// <response code="200">Lesson was successfully get.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(type: typeof(LessonDto), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLesson(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable model cannot be found because id is empty."
                });

            var lesson = await LessonRepository.GetLessonAsync(id);

            if (lesson is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Model not found.",
                    Details = "Cannot find model with given id."
                });

            var lessonDto = Mapper.Map<LessonDto>(lesson);

            return Ok(lessonDto);
        }

        /// <summary>
        /// Get all lessons.
        /// </summary>
        /// <returns>Returns all lessons.</returns>
        /// <response code="200">Lessons were successfully get.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpGet]
        [ProducesResponseType(type: typeof(IEnumerable<LessonDto>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllLessons()
        {
            var lessons = await LessonRepository.GetAllLessonsAsync();

            var lessonDto = Mapper.Map<IEnumerable<LessonDto>>(lessons);

            return Ok(lessonDto);
        }

        /// <summary>
        /// Create lesson.
        /// </summary>
        /// <param name="creationLessonDto">Creation model.</param>
        /// <returns>Returns created id.</returns>
        /// <response code="201">Lesson was successfully created.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpPost]
        [ModelValidationFilter]
        [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLesson([FromBody] CreationLessonDto creationLessonDto)
        {
            var creationCourse = Mapper.Map<Lesson>(creationLessonDto);

            var createdId = await LessonRepository.CreateLessonAsync(creationCourse);

            return Created($"api/lessons/{createdId}", createdId);
        }

        /// <summary>
        /// Delete lesson.
        /// </summary>
        /// <param name="id">Lesson id.</param>
        /// <returns>Returns status code.</returns>
        /// <response code="204">Lesson was successfully deleted.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(type: typeof(int), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLesson(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable model cannot be found because id is empty."
                });

            var lesson = await LessonRepository.GetLessonAsync(id);

            if (lesson is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Model not found.",
                    Details = "Cannot find model with given id."
                });

            await LessonRepository.DeleteLessonAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Update lesson.
        /// </summary>
        /// <param name="id">Updation id.</param>
        /// <param name="updationLessonDto">Updation model.</param>
        /// <returns></returns>
        /// <response code="204">Lesson was successfully updated.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpPut("{id}")]
        [ModelValidationFilter]
        [ProducesResponseType(type: typeof(int), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLesson(Guid id, [FromBody] UpdationLessonDto updationLessonDto)
        {
            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable model cannot be found because id is empty."
                });

            var lesson = await LessonRepository.GetLessonAsync(id, false);

            if (lesson is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Model not found.",
                    Details = "Cannot find model with given id."
                });

            var courseId = lesson.CourseId;

            lesson = Mapper.Map<Lesson>(updationLessonDto);
            lesson.Id = id;
            lesson.CourseId = courseId;

            await LessonRepository.UpdateLessonAsync(lesson);

            return NoContent();
        }

        /// <summary>
        /// Get lesson course.
        /// </summary>
        /// <param name="id">Lesson id.</param>
        /// <returns>Returns lesson course.</returns>
        /// <response code="200">Lesson course was successfully get.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpGet("{id}/course")]
        [ProducesResponseType(type: typeof(CourseDto), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLessonCourse(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable model cannot be found because id is empty."
                });

            var lesson = await LessonRepository.GetLessonAsync(id);

            if (lesson is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Model not found.",
                    Details = "Cannot find model with given id."
                });

            var course = await LessonRepository.GetLessonCourseAsync(id);

            var courseDto = Mapper.Map<CourseDto>(course);

            return Ok(courseDto);
        }

        /// <summary>
        /// Partial update of video info in lesson models.
        /// </summary>
        /// <param name="updationVideoId">Video id.</param>
        /// <param name="patchDocument">Patch document.</param>
        /// <returns></returns>
        /// <response code="204">Lesson was successfully patched.</response>
        /// <response code="400">Incoming id isn't valid.</response>
        /// <response code="500">Something going wrong on server.</response>
        [HttpPatch("{updationVideoId}")]
        [ProducesResponseType(type: typeof(int), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLessonsVideoInfo(string updationVideoId, [FromBody] JsonPatchDocument<UpdationLessonDto> patchDocument)
        {
            if (patchDocument is null)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Patch document is null",
                    Details = "Patch document for partial updaton of course model is null."
                });

            if (string.IsNullOrEmpty(updationVideoId))
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Video id is empty or null.",
                    Details = "Id of video cannot be empty."
                });

            var updationDto = new UpdationLessonDto();
            patchDocument.ApplyTo(updationDto);

            var updationModel = Mapper.Map<Lesson>(updationDto);

            await LessonRepository.UpdateLessonsVideoInfoAsync(updationVideoId, updationModel);

            return NoContent();
        }

        /// <summary>
        /// Purge database for integration tests.
        /// </summary>
        /// <param name="id">Lesson id.</param>
        /// <returns>Returns status code.</returns>
        /// <response code="204">Lesson was successfully deleted.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpDelete("purge/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(type: typeof(int), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Purge(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable model cannot be found because id is empty."
                });

            var lesson = await LessonRepository.GetLessonAsync(id);

            if (lesson is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Model not found.",
                    Details = "Cannot find model with given id."
                });

            await LessonRepository.DeleteLessonAsync(id);

            return NoContent();
        }

    }
}
