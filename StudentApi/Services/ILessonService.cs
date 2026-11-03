using StudentApi.Models;

namespace StudentApi.Services
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetLessonsAsync();

        Task<Lesson> GetLessonAsync(Guid lessonId);

        Task<Guid> AddLessonAsync(Lesson lesson);

        Task UpdateLessonAsync(Guid lessonId, Lesson lesson);

        Task DeleteLessonAsync(Guid lessonId);

        Task<List<Guid>> AssignStudentsAsync(Guid lessonId, List<Guid> neptunCodes);

        Task RemoveStudentAsync(Guid lessonId, Guid neptunCode);

        Task<List<Lesson>> GetStudentLessonsAsync(Guid neptunCode);

        Task<List<Student>> GetLessonStudentsAsync(Guid lessonId);
    }
}