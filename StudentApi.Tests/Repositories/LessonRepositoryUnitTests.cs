using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StudentApi.Models;
using StudentApi.Repositories;
using Xunit;

namespace StudentApi.Tests.Repositories
{
    public class LessonRepositoryUnitTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly AppDbContext _context;
        private readonly LessonRepository _repository;

        public LessonRepositoryUnitTests()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;
            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();
            _repository = new LessonRepository(_context);
        }

        [Fact]
        public async Task UnorderedLessons_GetLessonsAsync_ReturnsLessonsOrderedByStartTimeWithoutTracking()
        {
            // Arrange
            var earlier = CreateLesson("Earlier", new DateTime(2026, 11, 4, 8, 0, 0));
            var later = CreateLesson("Later", earlier.StartsAt.AddHours(2));
            await SeedAsync(later, earlier);

            // Act
            var result = await _repository.GetLessonsAsync();

            // Assert
            Assert.Equal(new[] { earlier.Id, later.Id }, result.Select(lesson => lesson.Id));
            Assert.Empty(_context.ChangeTracker.Entries());
        }

        [Fact]
        public async Task LessonWithAssignedStudent_GetLessonWithStudentsAsync_LoadsAssignedStudent()
        {
            // Arrange
            var student = CreateStudent("Anna");
            var lesson = CreateLesson("Mathematics", new DateTime(2026, 11, 4));
            lesson.Students.Add(student);
            await SeedAsync(lesson);

            // Act
            var result = await _repository.GetLessonWithStudentsAsync(lesson.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(lesson.Id, result.Id);
            Assert.Equal(student.NeptunCode, Assert.Single(result.Students).NeptunCode);
        }

        [Fact]
        public async Task AssignedAndUnrelatedLessons_GetStudentLessonsAsync_ReturnsOnlyAssignedLessonsOrderedByStartTime()
        {
            // Arrange
            var student = CreateStudent("Anna");
            var earlier = CreateLesson("Earlier", new DateTime(2026, 11, 4, 8, 0, 0));
            var later = CreateLesson("Later", earlier.StartsAt.AddHours(2));
            var unrelated = CreateLesson("Unrelated", earlier.StartsAt.AddHours(-1));
            earlier.Students.Add(student);
            later.Students.Add(student);
            await SeedAsync(later, unrelated, earlier);

            // Act
            var result = await _repository.GetStudentLessonsAsync(student.NeptunCode);

            // Assert
            Assert.Equal(new[] { earlier.Id, later.Id }, result.Select(lesson => lesson.Id));
            Assert.Empty(_context.ChangeTracker.Entries());
        }

        [Fact]
        public async Task AssignedAndUnrelatedStudents_GetLessonStudentsAsync_ReturnsOnlyAssignedStudentsOrderedByName()
        {
            // Arrange
            var anna = CreateStudent("Anna");
            var zoltan = CreateStudent("Zoltan");
            var unrelatedStudent = CreateStudent("Bela");
            var lesson = CreateLesson("Mathematics", new DateTime(2026, 11, 4));
            var otherLesson = CreateLesson("Physics", lesson.StartsAt.AddHours(2));
            lesson.Students.Add(zoltan);
            lesson.Students.Add(anna);
            otherLesson.Students.Add(unrelatedStudent);
            await SeedAsync(lesson, otherLesson);

            // Act
            var result = await _repository.GetLessonStudentsAsync(lesson.Id);

            // Assert
            Assert.Equal(new[] { anna.NeptunCode, zoltan.NeptunCode }, result.Select(student => student.NeptunCode));
            Assert.Empty(_context.ChangeTracker.Entries());
        }

        [Fact]
        public async Task NewLesson_AddLessonAndSaveChangesAsync_PersistsLesson()
        {
            // Arrange
            var lesson = CreateLesson("Mathematics", new DateTime(2026, 11, 4));

            // Act
            _repository.AddLesson(lesson);
            await _repository.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            // Assert
            var persisted = await _context.Lessons.SingleAsync();
            Assert.Equal(lesson.Id, persisted.Id);
            Assert.Equal(lesson.Name, persisted.Name);
            Assert.Equal(lesson.Room, persisted.Room);
            Assert.Equal(lesson.StartsAt, persisted.StartsAt);
            Assert.Equal(lesson.DurationMinutes, persisted.DurationMinutes);
        }

        [Fact]
        public async Task LessonWithAssignedStudent_DeleteLessonAndSaveChangesAsync_RemovesOnlyRequestedLessonAndItsRelationships()
        {
            // Arrange
            var student = CreateStudent("Anna");
            var lesson = CreateLesson("Mathematics", new DateTime(2026, 11, 4));
            var otherLesson = CreateLesson("Physics", lesson.StartsAt.AddHours(2));
            lesson.Students.Add(student);
            await SeedAsync(lesson, otherLesson);
            var lessonToDelete = await _repository.GetLessonAsync(lesson.Id);

            // Act
            _repository.DeleteLesson(lessonToDelete);
            await _repository.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(otherLesson.Id, (await _context.Lessons.SingleAsync()).Id);
            var remainingStudent = await _context.Students.Include(student => student.Lessons).SingleAsync();
            Assert.Equal(student.NeptunCode, remainingStudent.NeptunCode);
            Assert.Empty(remainingStudent.Lessons);
        }

        private async Task SeedAsync(params Lesson[] lessons)
        {
            _context.Lessons.AddRange(lessons);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        private static Lesson CreateLesson(string name, DateTime startsAt)
        {
            return new Lesson
            {
                Id = Guid.NewGuid(),
                Name = name,
                Room = "A101",
                StartsAt = startsAt,
                DurationMinutes = 90,
            };
        }

        private static Student CreateStudent(string name)
        {
            return new Student
            {
                NeptunCode = Guid.NewGuid(),
                Name = name,
                BirthDate = new DateOnly(2000, 1, 2),
                EmailAddress = $"{name}@example.com",
            };
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}