using StudentApi.Models;

namespace StudentApi.Repositories
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> GetLessonsAsync();

        Task<Lesson> GetLessonAsync(Guid lessonId);

        Task<Lesson> GetLessonWithStudentsAsync(Guid lessonId);

        Task<List<Lesson>> GetStudentLessonsAsync(Guid neptunCode);

        Task<List<Student>> GetLessonStudentsAsync(Guid lessonId);

        void AddLesson(Lesson lesson);

        void DeleteLesson(Lesson lesson);

        Task SaveChangesAsync();
    }
}