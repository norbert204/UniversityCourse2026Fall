using Microsoft.EntityFrameworkCore;
using StudentApi.Models;

namespace StudentApi.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _appDbContext;

        public StudentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _appDbContext.Students.AsNoTracking().ToListAsync();
        }

        public async Task<Student> GetStudentAsync(Guid neptunCode)
        {
            return await _appDbContext.Students
                .FirstOrDefaultAsync(student => student.NeptunCode == neptunCode);
        }

        public async Task<List<Student>> GetStudentsByCodesAsync(List<Guid> neptunCodes)
        {
            return await _appDbContext.Students
                .Where(student => neptunCodes.Contains(student.NeptunCode)).ToListAsync();
        }

        public void AddStudent(Student student)
        {
            _appDbContext.Students.Add(student);
        }

        public void DeleteStudent(Student student)
        {
            _appDbContext.Students.Remove(student);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}