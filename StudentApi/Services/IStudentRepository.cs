using StudentApi.Models;

namespace StudentApi.Services
{
    public interface IStudentRepository
    {
        List<Student> GetStudents();

        void AddStudent(Student student);

        void DeleteStudent(string neptunCode);

        void UpdateStudent(string neptunCode, Student student);

        Student GetStudent(string neptunCode);
    }
}
