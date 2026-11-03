using StudentApi.Exceptions;
using StudentApi.Models;
using StudentApi.Repositories;

namespace StudentApi.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IStudentRepository _studentRepository;

        public LessonService(ILessonRepository lessonRepository, IStudentRepository studentRepository)
        {
            if (lessonRepository is null)
            {
                throw new ArgumentNullException(nameof(lessonRepository));
            }

            if (studentRepository is null)
            {
                throw new ArgumentNullException(nameof(studentRepository));
            }

            _lessonRepository = lessonRepository;
            _studentRepository = studentRepository;
        }

        public Task<List<Lesson>> GetLessonsAsync()
        {
            return _lessonRepository.GetLessonsAsync();
        }

        public Task<Lesson> GetLessonAsync(Guid lessonId)
        {
            if (lessonId == Guid.Empty)
            {
                throw new ArgumentException("Lesson ID cannot be empty.", nameof(lessonId));
            }

            return _lessonRepository.GetLessonAsync(lessonId);
        }

        public async Task<Guid> AddLessonAsync(Lesson lesson)
        {
            if (lesson is null)
            {
                throw new ArgumentNullException(nameof(lesson));
            }

            lesson.Id = Guid.CreateVersion7();
            _lessonRepository.AddLesson(lesson);
            await _lessonRepository.SaveChangesAsync();
            return lesson.Id;
        }

        public async Task UpdateLessonAsync(Guid lessonId, Lesson lesson)
        {
            if (lesson is null)
            {
                throw new ArgumentNullException(nameof(lesson));
            }

            var lessonToUpdate = await GetRequiredLessonAsync(lessonId);
            lessonToUpdate.Name = lesson.Name;
            lessonToUpdate.Room = lesson.Room;
            lessonToUpdate.StartsAt = lesson.StartsAt;
            lessonToUpdate.DurationMinutes = lesson.DurationMinutes;
            await _lessonRepository.SaveChangesAsync();
        }

        public async Task DeleteLessonAsync(Guid lessonId)
        {
            var lesson = await GetRequiredLessonAsync(lessonId);
            _lessonRepository.DeleteLesson(lesson);
            await _lessonRepository.SaveChangesAsync();
        }

        public async Task<List<Guid>> AssignStudentsAsync(Guid lessonId, List<Guid> neptunCodes)
        {
            if (neptunCodes is null)
            {
                throw new ArgumentNullException(nameof(neptunCodes));
            }

            var lesson = await GetRequiredLessonWithStudentsAsync(lessonId);
            var distinctCodes = neptunCodes.Distinct().ToList();
            var students = await _studentRepository.GetStudentsByCodesAsync(distinctCodes);
            if (students.Count != distinctCodes.Count)
            {
                throw new EntityNotFoundException("One or more students do not exist");
            }

            foreach (var student in students)
            {
                if (!lesson.Students.Any(assignedStudent => assignedStudent.NeptunCode == student.NeptunCode))
                {
                    lesson.Students.Add(student);
                }
            }

            await _lessonRepository.SaveChangesAsync();
            return distinctCodes;
        }

        public async Task RemoveStudentAsync(Guid lessonId, Guid neptunCode)
        {
            var lesson = await GetRequiredLessonWithStudentsAsync(lessonId);
            var student = lesson.Students.FirstOrDefault(student => student.NeptunCode == neptunCode);
            if (student is null)
            {
                throw new EntityNotFoundException("Student is not assigned to this lesson");
            }

            lesson.Students.Remove(student);
            await _lessonRepository.SaveChangesAsync();
        }

        public async Task<List<Lesson>> GetStudentLessonsAsync(Guid neptunCode)
        {
            if (await _studentRepository.GetStudentAsync(neptunCode) is null)
            {
                throw new EntityNotFoundException($"Student with Neptun Code '{neptunCode}' does not exist");
            }

            return await _lessonRepository.GetStudentLessonsAsync(neptunCode);
        }

        public async Task<List<Student>> GetLessonStudentsAsync(Guid lessonId)
        {
            await GetRequiredLessonAsync(lessonId);
            return await _lessonRepository.GetLessonStudentsAsync(lessonId);
        }

        private async Task<Lesson> GetRequiredLessonAsync(Guid lessonId)
        {
            var lesson = await _lessonRepository.GetLessonAsync(lessonId);
            if (lesson is null)
            {
                throw new EntityNotFoundException($"Lesson with Id '{lessonId}' does not exist");
            }

            return lesson;
        }

        private async Task<Lesson> GetRequiredLessonWithStudentsAsync(Guid lessonId)
        {
            var lesson = await _lessonRepository.GetLessonWithStudentsAsync(lessonId);
            if (lesson is null)
            {
                throw new EntityNotFoundException($"Lesson with Id '{lessonId}' does not exist");
            }

            return lesson;
        }
    }
}