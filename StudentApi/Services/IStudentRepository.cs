using StudentApi.Contracts;
using StudentApi.Models;

namespace StudentApi.Services
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();

        Task<Guid> AddStudentAsync(Student student);

        Task DeleteStudentAsync(Guid neptunCode);

        Task UpdateStudentAsync(Guid neptunCode, Student student);

        Task<Student> GetStudentAsync(Guid neptunCode);
    }
}
