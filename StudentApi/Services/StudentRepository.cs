using Microsoft.EntityFrameworkCore;
using StudentApi.Exceptions;
using StudentApi.Models;
using System.Runtime.InteropServices;

namespace StudentApi.Services
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _appDbContext;

        public StudentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> AddStudentAsync(Student student)
        {
            var neptunCode = Guid.CreateVersion7();

            student.NeptunCode = neptunCode;

            _appDbContext.Add(student);
            await _appDbContext.SaveChangesAsync();
            return neptunCode;
        }

        public async Task DeleteStudentAsync(Guid neptunCode)
        {
            var studentToDelete = _appDbContext.Students.FirstOrDefault(x => x.NeptunCode == neptunCode);

            if (studentToDelete is null)
            {
                throw new EntityNotFoundException($"Student with Neptun Code '{neptunCode}' already exists");
            }

            _appDbContext.Remove(studentToDelete);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Student> GetStudentAsync(Guid neptunCode)
        {
            return await _appDbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.NeptunCode == neptunCode);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _appDbContext.Students.AsNoTracking().ToListAsync();
        }

        public async Task UpdateStudentAsync(Guid neptunCode, Student student)
        {
            var studentToUpdate = await _appDbContext.Students.FirstOrDefaultAsync(x => x.NeptunCode == neptunCode);

            if (studentToUpdate is null)
            {
                throw new EntityNotFoundException($"Student with Neptun Code '{neptunCode}' already exists");
            }

            studentToUpdate.Name = student.Name;
            studentToUpdate.BirthDate = student.BirthDate;
            studentToUpdate.EmailAddress = student.EmailAddress;

            await _appDbContext.SaveChangesAsync();
        }
    }
}
