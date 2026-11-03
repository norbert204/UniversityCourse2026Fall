using StudentApi.Models;

namespace StudentApi.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();

        Task<Student> GetStudentAsync(Guid neptunCode);

        Task<List<Student>> GetStudentsByCodesAsync(List<Guid> neptunCodes);

        void AddStudent(Student student);

        void DeleteStudent(Student student);

        Task SaveChangesAsync();
    }
}