using Microsoft.AspNetCore.Mvc;
using StudentApi.Contracts;
using StudentApi.Mappers;
using StudentApi.Models;
using StudentApi.Services;
using System.Net;

namespace StudentApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepository _studentRepository;

        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepository)
        {
            _logger = logger;
            _studentRepository = studentRepository;
        }


        [HttpGet]
        public async Task<ActionResult<GetStudentsResponse>> GetStudents()
        {
            _logger.LogInformation("Get Students request received");
            var students = await _studentRepository.GetStudentsAsync();
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

            var neptunCode = await _studentRepository.AddStudentAsync(request.ToStudentEntity());

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
            await _studentRepository.DeleteStudentAsync(neptunCode);
            return NoContent();
        }

        [HttpPut("{neptunCode}")]
        public async Task<ActionResult> UpdateStudent(Guid neptunCode, [FromBody] UpdateStudentRequest request)
        {
            await _studentRepository.UpdateStudentAsync(neptunCode, request.ToStudentEntity());
            return NoContent();
        }

        [HttpGet("{neptunCode}")]
        public async Task<ActionResult<GetStudentResponse>> GetStudent(Guid neptunCode)
        {
            var student = await _studentRepository.GetStudentAsync(neptunCode);

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
