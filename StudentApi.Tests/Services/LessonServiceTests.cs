using NSubstitute;
using StudentApi.Exceptions;
using StudentApi.Models;
using StudentApi.Repositories;
using StudentApi.Services;
using Xunit;

namespace StudentApi.Tests.Services
{
    public class LessonServiceTests
    {
        private readonly ILessonRepository _lessonRepository = Substitute.For<ILessonRepository>();
        private readonly IStudentRepository _studentRepository = Substitute.For<IStudentRepository>();
        private readonly LessonService _service;

        public LessonServiceTests()
        {
            _service = new LessonService(_lessonRepository, _studentRepository);
        }

        [Theory]
        [InlineData(true, "lessonRepository")]
        [InlineData(false, "studentRepository")]
        public void NullRepository_CreateLessonService_ThrowsArgumentNullException(bool nullLessonRepository, string parameterName)
        {
            // Arrange
            var lessonRepository = nullLessonRepository ? null : _lessonRepository;
            var studentRepository = nullLessonRepository ? _studentRepository : null;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(
                () => new LessonService(lessonRepository!, studentRepository!));

            // Assert
            Assert.Equal(parameterName, exception.ParamName);
        }

        [Fact]
        public async Task EmptyLessonId_GetLessonAsync_ThrowsWithoutAccessingRepositories()
        {
            // Arrange
            var lessonId = Guid.Empty;

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.GetLessonAsync(lessonId));

            // Assert
            Assert.Equal("lessonId", exception.ParamName);
            Assert.StartsWith("Lesson ID cannot be empty.", exception.Message);
            Assert.Empty(_lessonRepository.ReceivedCalls());
            Assert.Empty(_studentRepository.ReceivedCalls());
        }

        [Fact]
        public async Task NullLesson_AddLessonAsync_ThrowsWithoutAccessingRepositories()
        {
            // Arrange
            Lesson lesson = null!;

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _service.AddLessonAsync(lesson));

            // Assert
            Assert.Equal("lesson", exception.ParamName);
            Assert.Empty(_lessonRepository.ReceivedCalls());
            Assert.Empty(_studentRepository.ReceivedCalls());
        }

        [Fact]
        public async Task NullLesson_UpdateLessonAsync_ThrowsWithoutAccessingRepositories()
        {
            // Arrange
            var lessonId = Guid.NewGuid();

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _service.UpdateLessonAsync(lessonId, null!));

            // Assert
            Assert.Equal("lesson", exception.ParamName);
            Assert.Empty(_lessonRepository.ReceivedCalls());
            Assert.Empty(_studentRepository.ReceivedCalls());
        }

        [Fact]
        public async Task NullStudentCodes_AssignStudentsAsync_ThrowsWithoutAccessingRepositories()
        {
            // Arrange
            var lessonId = Guid.NewGuid();

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _service.AssignStudentsAsync(lessonId, null!));

            // Assert
            Assert.Equal("neptunCodes", exception.ParamName);
            Assert.Empty(_lessonRepository.ReceivedCalls());
            Assert.Empty(_studentRepository.ReceivedCalls());
        }

        [Fact]
        public async Task LessonWithoutId_AddLessonAsync_GeneratesIdAndSavesLesson()
        {
            // Arrange
            var lesson = new Lesson { Name = "Mathematics" };

            // Act
            var lessonId = await _service.AddLessonAsync(lesson);

            // Assert
            Assert.NotEqual(Guid.Empty, lessonId);
            Assert.Equal(lessonId, lesson.Id);
            _lessonRepository.Received(1).AddLesson(lesson);
            await _lessonRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task ExistingLesson_UpdateLessonAsync_UpdatesFieldsAndSaves()
        {
            // Arrange
            var lesson = new Lesson { Id = Guid.NewGuid(), Name = "Old name" };
            var update = new Lesson
            {
                Name = "Physics",
                Room = "A101",
                StartsAt = new DateTime(2026, 11, 4, 10, 0, 0),
                DurationMinutes = 90,
            };
            _lessonRepository.GetLessonAsync(lesson.Id).Returns(lesson);

            // Act
            await _service.UpdateLessonAsync(lesson.Id, update);

            // Assert
            Assert.Equal(update.Name, lesson.Name);
            Assert.Equal(update.Room, lesson.Room);
            Assert.Equal(update.StartsAt, lesson.StartsAt);
            Assert.Equal(update.DurationMinutes, lesson.DurationMinutes);
            await _lessonRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task DuplicateCodesAndAlreadyAssignedStudent_AssignStudentsAsync_AssignsOnlyNewStudentAndSaves()
        {
            // Arrange
            var assignedStudent = new Student { NeptunCode = Guid.NewGuid() };
            var newStudent = new Student { NeptunCode = Guid.NewGuid() };
            var lesson = new Lesson { Id = Guid.NewGuid(), Students = new List<Student> { assignedStudent } };
            var codes = new List<Guid> { assignedStudent.NeptunCode, newStudent.NeptunCode, newStudent.NeptunCode };
            var distinctCodes = new[] { assignedStudent.NeptunCode, newStudent.NeptunCode };
            _lessonRepository.GetLessonWithStudentsAsync(lesson.Id).Returns(lesson);
            _studentRepository.GetStudentsByCodesAsync(
                Arg.Is<List<Guid>>(requestedCodes => requestedCodes.SequenceEqual(distinctCodes)))
                .Returns(new List<Student> { assignedStudent, newStudent });

            // Act
            var result = await _service.AssignStudentsAsync(lesson.Id, codes);

            // Assert
            Assert.Equal(distinctCodes, result);
            Assert.Equal(2, lesson.Students.Count);
            Assert.Contains(assignedStudent, lesson.Students);
            Assert.Contains(newStudent, lesson.Students);
            await _studentRepository.Received(1).GetStudentsByCodesAsync(
                Arg.Is<List<Guid>>(requestedCodes => requestedCodes.SequenceEqual(distinctCodes)));
            await _lessonRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task MissingStudent_AssignStudentsAsync_ThrowsWithoutChangingAssignmentsOrSaving()
        {
            // Arrange
            var assignedStudent = new Student { NeptunCode = Guid.NewGuid() };
            var newStudent = new Student { NeptunCode = Guid.NewGuid() };
            var lesson = new Lesson { Id = Guid.NewGuid(), Students = new List<Student> { assignedStudent } };
            var codes = new List<Guid> { newStudent.NeptunCode, Guid.NewGuid() };
            _lessonRepository.GetLessonWithStudentsAsync(lesson.Id).Returns(lesson);
            _studentRepository.GetStudentsByCodesAsync(Arg.Any<List<Guid>>())
                .Returns(new List<Student> { newStudent });

            // Act
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.AssignStudentsAsync(lesson.Id, codes));

            // Assert
            Assert.Equal("One or more students do not exist", exception.Message);
            Assert.Same(assignedStudent, Assert.Single(lesson.Students));
            await _lessonRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task MissingLesson_AssignStudentsAsync_ThrowsWithoutLoadingStudentsOrSaving()
        {
            // Arrange
            var lessonId = Guid.NewGuid();
            var codes = new List<Guid> { Guid.NewGuid() };
            _lessonRepository.GetLessonWithStudentsAsync(lessonId)
                .Returns(Task.FromResult<Lesson>(null!));

            // Act
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.AssignStudentsAsync(lessonId, codes));

            // Assert
            Assert.Contains(lessonId.ToString(), exception.Message);
            await _studentRepository.DidNotReceive().GetStudentsByCodesAsync(Arg.Any<List<Guid>>());
            await _lessonRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task AssignedStudents_RemoveStudentAsync_RemovesOnlyRequestedStudentAndSaves()
        {
            // Arrange
            var student = new Student { NeptunCode = Guid.NewGuid() };
            var otherStudent = new Student { NeptunCode = Guid.NewGuid() };
            var lesson = new Lesson { Id = Guid.NewGuid(), Students = new List<Student> { student, otherStudent } };
            _lessonRepository.GetLessonWithStudentsAsync(lesson.Id).Returns(lesson);

            // Act
            await _service.RemoveStudentAsync(lesson.Id, student.NeptunCode);

            // Assert
            Assert.Same(otherStudent, Assert.Single(lesson.Students));
            await _lessonRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task UnassignedStudent_RemoveStudentAsync_ThrowsWithoutSaving()
        {
            // Arrange
            var assignedStudent = new Student { NeptunCode = Guid.NewGuid() };
            var lesson = new Lesson { Id = Guid.NewGuid(), Students = new List<Student> { assignedStudent } };
            _lessonRepository.GetLessonWithStudentsAsync(lesson.Id).Returns(lesson);

            // Act
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.RemoveStudentAsync(lesson.Id, Guid.NewGuid()));

            // Assert
            Assert.Equal("Student is not assigned to this lesson", exception.Message);
            Assert.Same(assignedStudent, Assert.Single(lesson.Students));
            await _lessonRepository.DidNotReceive().SaveChangesAsync();
        }
    }
}