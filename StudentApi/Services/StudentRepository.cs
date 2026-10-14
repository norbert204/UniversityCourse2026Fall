using StudentApi.Exceptions;
using StudentApi.Models;

namespace StudentApi.Services
{
    public class StudentRepository : IStudentRepository
    {
        private readonly List<Student> _students;

        public StudentRepository()
        {
            _students = [];
        }

        public Guid AddStudent(Student student)
        {
            var neptunCode = Guid.CreateVersion7();

            student.NeptunCode = neptunCode;

            _students.Add(student);

            return neptunCode;
        }

        public void DeleteStudent(Guid neptunCode)
        {
            var studentToDelete = _students.FirstOrDefault(x => x.NeptunCode == neptunCode);

            if (studentToDelete is null)
            {
                throw new EntityNotFoundException($"Student with Neptun Code '{neptunCode}' already exists");
            }

            _students.Remove(studentToDelete);
        }

        public Student GetStudent(Guid neptunCode)
        {
            return _students.FirstOrDefault(x => x.NeptunCode == neptunCode);
        }

        public List<Student> GetStudents()
        {
            return _students;
        }

        public void UpdateStudent(Guid neptunCode, Student student)
        {
            var studentToUpdate = _students.FindIndex(x => x.NeptunCode == neptunCode);

            if (studentToUpdate < 0)
            {
                throw new EntityNotFoundException($"Student with Neptun Code '{neptunCode}' already exists");
            }

            student.NeptunCode = neptunCode;

            _students[studentToUpdate] = student;
        }
    }
}
