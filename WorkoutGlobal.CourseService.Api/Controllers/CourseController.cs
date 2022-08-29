using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutGlobal.CourseService.Api.Contracts;
using WorkoutGlobal.CourseService.Api.Dto;
using WorkoutGlobal.CourseService.Api.Filters.ActionFilters;
using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.Api.Controllers
{
    /// <summary>
    /// Represents course controller.
    /// </summary>
    [Route("api/courses")]
    [ApiController]
    [Produces("application/json")]
    public class CourseController : ControllerBase
    {
        private ICourseRepository _courseRepository;
        private IMapper _mapper;

        /// <summary>
        /// Ctor for course controller.
        /// </summary>
        /// <param name="courseRepository">Course repository.</param>
        /// <param name="mapper">AutoMapper instanse.</param>
        public CourseController(
            ICourseRepository courseRepository,
            IMapper mapper)
        {
            CourseRepository = courseRepository;
            Mapper = mapper;
        }

        /// <summary>
        /// Repository manager.
        /// </summary>
        public ICourseRepository CourseRepository
        {
            get => _courseRepository;
            private set => _courseRepository = value ?? throw new NullReferenceException(nameof(value));
        }

        /// <summary>
        /// Auto mapping helper.
        /// </summary>
        public IMapper Mapper
        {
            get => _mapper;
            private set => _mapper = value ?? throw new NullReferenceException(nameof(value));
        }

        /// <summary>
        /// Get course by id.
        /// </summary>
        /// <param name="id">Course id.</param>
        /// <returns>Returns course by given id.</returns>
        /// <response code="200">Course was successfully get.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(type: typeof(CourseDto), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCourse(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable model cannot be found because id is empty."
                });

            var course = await CourseRepository.GetCourseAsync(id);

            if (course is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Model not found",
                    Details = "Cannot find model with given id."
                });

            var courseDto = Mapper.Map<CourseDto>(course);

            return Ok(courseDto);
        }

        /// <summary>
        /// Get all courses.
        /// </summary>
        /// <returns>Returns collection of courses.</returns>
        /// <response code="200">Courses was successfully get.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpGet]
        [ProducesResponseType(type: typeof(IEnumerable<CourseDto>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCourse()
        {
            var courses = await CourseRepository.GetAllCoursesAsync();

            var courseDtos = Mapper.Map<IEnumerable<CourseDto>>(courses);

            return Ok(courseDtos);
        }

        /// <summary>
        /// Create course.
        /// </summary>
        /// <param name="creationCourseDto">Creation model.</param>
        /// <returns>Returns location hidder and string with string-id.</returns>
        /// <response code="201">Course was successfully created.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpPost]
        [ModelValidationFilter]
        [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status201Created)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCourse([FromBody] CreationCourseDto creationCourseDto)
        {
            var creationCourse = _mapper.Map<Course>(creationCourseDto);

            var createdId = await CourseRepository.CreateCourseAsync(creationCourse);

            return Created($"api/courses/{createdId}", createdId);
        }

        /// <summary>
        /// Delete course by id.
        /// </summary>
        /// <param name="id">Course id.</param>
        /// <returns>Returns status code.</returns>
        /// <response code="204">Course was successfully deleted.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(type: typeof(int), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable model cannot be found because id is empty."
                });

            var course = await CourseRepository.GetCourseAsync(id);

            if (course is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Model not found",
                    Details = "Cannot find model with given id."
                });

            await CourseRepository.DeleteCourseAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Update course.
        /// </summary>
        /// <param name="id">Course id.</param>
        /// <param name="updationCourseDto">Updation course.</param>
        /// <returns></returns>
        /// <response code="204">Course was successfully updated.</response>
        /// <response code="400">Params of request is uncorrect.</response>
        /// <response code="404">Model don't exists.</response>
        /// <response code="500">Something wrong happen on server.</response>
        [HttpPut]
        [ModelValidationFilter]
        [ProducesResponseType(type: typeof(int), statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(type: typeof(ErrorDetails), statusCode: StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdationCourseDto updationCourseDto)
        {
            if (id == Guid.Empty)
                return BadRequest(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Id is empty.",
                    Details = "Searchable model cannot be found because id is empty."
                });

            var course = await CourseRepository.GetCourseAsync(id, false);

            if (course is null)
                return NotFound(new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Model not found",
                    Details = "Cannot find model with given id."
                });

            // TODO: Check that object no tracked and exception don't throw.
            course = Mapper.Map<Course>(updationCourseDto);
            course.Id = id;

            await CourseRepository.UpdateCourseAsync(course);

            return NoContent();
        }



    }
}
