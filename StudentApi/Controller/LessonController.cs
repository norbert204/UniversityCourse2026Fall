using Microsoft.AspNetCore.Mvc;
using StudentApi.Contracts;
using StudentApi.Mappers;
using StudentApi.Services;

namespace StudentApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<ActionResult<GetLessonsResponse>> GetLessons()
        {
            var lessons = await _lessonService.GetLessonsAsync();
            return Ok(new GetLessonsResponse { Lessons = lessons.ToDto() });
        }

        [HttpGet("{lessonId}")]
        public async Task<ActionResult<GetLessonResponse>> GetLesson(Guid lessonId)
        {
            var lesson = await _lessonService.GetLessonAsync(lessonId);
            if (lesson is null)
            {
                return NotFound();
            }

            return Ok(new GetLessonResponse { Lesson = lesson.ToDto() });
        }

        [HttpPost]
        public async Task<ActionResult<AddLessonResponse>> AddLesson(AddLessonRequest request)
        {
            var lessonId = await _lessonService.AddLessonAsync(request.ToLessonEntity());
            return Ok(new AddLessonResponse { Id = lessonId });
        }

        [HttpPut("{lessonId}")]
        public async Task<ActionResult> UpdateLesson(Guid lessonId, UpdateLessonRequest request)
        {
            await _lessonService.UpdateLessonAsync(lessonId, request.ToLessonEntity());
            return NoContent();
        }

        [HttpDelete("{lessonId}")]
        public async Task<ActionResult> DeleteLesson(Guid lessonId)
        {
            await _lessonService.DeleteLessonAsync(lessonId);
            return NoContent();
        }

        [HttpPost("{lessonId}/students")]
        public async Task<ActionResult<AssignStudentToLessonResponse>> AssignStudents(
            Guid lessonId, AssignStudentToLessonRequest request)
        {
            var neptunCodes = await _lessonService.AssignStudentsAsync(lessonId, request.NeptunCodes);
            return Ok(new AssignStudentToLessonResponse
            {
                LessonId = lessonId,
                NeptunCodes = neptunCodes,
            });
        }

        [HttpDelete("{lessonId}/students/{neptunCode}")]
        public async Task<ActionResult> RemoveStudent(Guid lessonId, Guid neptunCode)
        {
            await _lessonService.RemoveStudentAsync(lessonId, neptunCode);
            return NoContent();
        }

        [HttpGet("{lessonId}/students")]
        public async Task<ActionResult<GetStudentsResponse>> GetLessonStudents(Guid lessonId)
        {
            var students = await _lessonService.GetLessonStudentsAsync(lessonId);
            return Ok(new GetStudentsResponse { Students = students.ToDto() });
        }
    }
}