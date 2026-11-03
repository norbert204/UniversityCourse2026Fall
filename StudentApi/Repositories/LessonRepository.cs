using Microsoft.EntityFrameworkCore;
using StudentApi.Models;

namespace StudentApi.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly AppDbContext _appDbContext;

        public LessonRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Lesson>> GetLessonsAsync()
        {
            return await _appDbContext.Lessons.AsNoTracking()
                .OrderBy(lesson => lesson.StartsAt).ToListAsync();
        }

        public async Task<Lesson> GetLessonAsync(Guid lessonId)
        {
            return await _appDbContext.Lessons
                .FirstOrDefaultAsync(lesson => lesson.Id == lessonId);
        }

        public async Task<Lesson> GetLessonWithStudentsAsync(Guid lessonId)
        {
            return await _appDbContext.Lessons.Include(lesson => lesson.Students)
                .FirstOrDefaultAsync(lesson => lesson.Id == lessonId);
        }

        public async Task<List<Lesson>> GetStudentLessonsAsync(Guid neptunCode)
        {
            return await _appDbContext.Lessons.AsNoTracking()
                .Where(lesson => lesson.Students.Any(student => student.NeptunCode == neptunCode))
                .OrderBy(lesson => lesson.StartsAt).ToListAsync();
        }

        public async Task<List<Student>> GetLessonStudentsAsync(Guid lessonId)
        {
            return await _appDbContext.Students.AsNoTracking()
                .Where(student => student.Lessons.Any(lesson => lesson.Id == lessonId))
                .OrderBy(student => student.Name).ToListAsync();
        }

        public void AddLesson(Lesson lesson)
        {
            _appDbContext.Lessons.Add(lesson);
        }

        public void DeleteLesson(Lesson lesson)
        {
            _appDbContext.Lessons.Remove(lesson);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}