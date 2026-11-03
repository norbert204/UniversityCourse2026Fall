using StudentApi.Models;

namespace StudentApi.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetStudentsAsync();

        Task<Student> GetStudentAsync(Guid neptunCode);

        Task<Guid> AddStudentAsync(Student student);

        Task UpdateStudentAsync(Guid neptunCode, Student student);

        Task DeleteStudentAsync(Guid neptunCode);
    }
}