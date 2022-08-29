using Microsoft.EntityFrameworkCore;
using WorkoutGlobal.CourseService.Api.Contracts;
using WorkoutGlobal.CourseService.Api.DbContext;
using WorkoutGlobal.CourseService.Api.Models;

namespace WorkoutGlobal.CourseService.Api.Repositories
{
    public class LessonRepository : BaseRepository<Lesson, Guid>, ILessonRepository
    {
        public LessonRepository(
            IConfiguration configuration,
            CourseServiceContext context)
            : base(configuration, context)
        { }

        public async Task<Guid> CreateLesson(Lesson courseLesson)
        {
            var createdId = await CreateAsync(courseLesson);

            await SaveChangesAsync();

            return createdId;
        }

        public async Task DeleteLesson(Guid id)
        {
            await DeleteAsync(id);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Lesson>> GetAllLessons()
        {
            var models = await GetAll().ToListAsync();

            return models;
        }

        public async Task<Lesson> GetLesson(Guid id)
        {
            var model = await GetAsync(id);

            return model;
        }

        public async Task<Course> GetLessonCourse(Guid lessonId)
        {
            var lesson = await GetAsync(lessonId);

            var course = await Context.Courses
                .Where(course => course.Id == lesson.CourseId)
                .FirstOrDefaultAsync();

            return course;
        }

        public async Task UpdateLesson(Lesson courseLesson)
        {
            Update(courseLesson);

            await SaveChangesAsync();
        }
    }
}
