using StudentApi.Exceptions;
using StudentApi.Models;
using StudentApi.Repositories;

namespace StudentApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public Task<List<Student>> GetStudentsAsync()
        {
            return _studentRepository.GetStudentsAsync();
        }

        public Task<Student> GetStudentAsync(Guid neptunCode)
        {
            return _studentRepository.GetStudentAsync(neptunCode);
        }

        public async Task<Guid> AddStudentAsync(Student student)
        {
            student.NeptunCode = Guid.CreateVersion7();
            _studentRepository.AddStudent(student);
            await _studentRepository.SaveChangesAsync();
            return student.NeptunCode;
        }

        public async Task UpdateStudentAsync(Guid neptunCode, Student student)
        {
            var studentToUpdate = await GetRequiredStudentAsync(neptunCode);
            studentToUpdate.Name = student.Name;
            studentToUpdate.BirthDate = student.BirthDate;
            studentToUpdate.EmailAddress = student.EmailAddress;
            await _studentRepository.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(Guid neptunCode)
        {
            var student = await GetRequiredStudentAsync(neptunCode);
            _studentRepository.DeleteStudent(student);
            await _studentRepository.SaveChangesAsync();
        }

        private async Task<Student> GetRequiredStudentAsync(Guid neptunCode)
        {
            return await _studentRepository.GetStudentAsync(neptunCode)
                ?? throw new EntityNotFoundException($"Student with Neptun Code '{neptunCode}' does not exist");
        }
    }
}