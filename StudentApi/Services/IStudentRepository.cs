using StudentApi.Contracts;
using StudentApi.Models;

namespace StudentApi.Services
{
    public interface IStudentRepository
    {
        List<Student> GetStudents();

        Guid AddStudent(Student student);

        void DeleteStudent(Guid neptunCode);

        void UpdateStudent(Guid neptunCode, Student student);

        Student GetStudent(Guid neptunCode);
    }
}
