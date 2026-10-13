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

        public void AddStudent(Student student)
        {
            if (_students.Any(x => x.NeptunCode == student.NeptunCode))
            {
                throw new InvalidOperationException($"Student with Neptun Code '{student.NeptunCode}' already exists");
            }

            _students.Add(student);
        }

        public void DeleteStudent(string neptunCode)
        {
            var studentToDelete = _students.FirstOrDefault(x => x.NeptunCode == neptunCode);

            _students.Remove(studentToDelete);
        }

        public Student GetStudent(string neptunCode)
        {
            return _students.FirstOrDefault(x => x.NeptunCode == neptunCode);
        }

        public List<Student> GetStudents()
        {
            return _students;
        }

        public void UpdateStudent(string neptunCode, Student student)
        {
            if (neptunCode != student.NeptunCode)
            {
                throw new InvalidOperationException($"Neptun Code provided in URI is not the same as in the Body");
            }

            var studentToUpdate = _students.FindIndex(x => x.NeptunCode == neptunCode);

            _students[studentToUpdate] = student;
        }
    }
}
