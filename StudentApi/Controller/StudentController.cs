using Microsoft.AspNetCore.Mvc;
using StudentApi.Contracts;
using StudentApi.Mappers;
using StudentApi.Services;

namespace StudentApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentService _studentService;
        private readonly ILessonService _lessonService;

        public StudentController(ILogger<StudentController> logger, IStudentService studentService,
            ILessonService lessonService)
        {
            _logger = logger;
            _studentService = studentService;
            _lessonService = lessonService;
        }


        [HttpGet]
        public async Task<ActionResult<GetStudentsResponse>> GetStudents()
        {
            _logger.LogInformation("Get Students request received");
            var students = await _studentService.GetStudentsAsync();
            var response = new GetStudentsResponse
            {
                Students = students.ToDto(),
            };

            _logger.LogInformation("Get students request succeeded with response {@Response}", response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<AddStudentResponse>> AddStudent(AddStudentRequest request)
        {
            _logger.LogInformation("Add Student request received: {@Request}", request);

            var neptunCode = await _studentService.AddStudentAsync(request.ToStudentEntity());

            var response = new AddStudentResponse
            {
                NeptunCode = neptunCode,
            };

            _logger.LogInformation("Add Student request succeded with response {@Response}", response);

            return Ok(response);
        }

        [HttpDelete("{neptunCode}")]
        public async Task<ActionResult> DeleteStudent(Guid neptunCode)
        {
            await _studentService.DeleteStudentAsync(neptunCode);
            return NoContent();
        }

        [HttpPut("{neptunCode}")]
        public async Task<ActionResult> UpdateStudent(Guid neptunCode, [FromBody] UpdateStudentRequest request)
        {
            await _studentService.UpdateStudentAsync(neptunCode, request.ToStudentEntity());
            return NoContent();
        }

        [HttpGet("{neptunCode}/lessons")]
        public async Task<ActionResult<GetLessonsResponse>> GetStudentLessons(Guid neptunCode)
        {
            var lessons = await _lessonService.GetStudentLessonsAsync(neptunCode);
            return Ok(new GetLessonsResponse { Lessons = lessons.ToDto() });
        }

        [HttpGet("{neptunCode}")]
        public async Task<ActionResult<GetStudentResponse>> GetStudent(Guid neptunCode)
        {
            var student = await _studentService.GetStudentAsync(neptunCode);

            if (student is not null)
            {
                return Ok(
                    new GetStudentResponse
                    {
                        Student = student.ToDto(),
                    });
            }

            return NotFound();
        }
    }
}
