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
        public ActionResult<GetStudentsResponse> GetStudents()
        {
            _logger.LogInformation("Get Students request received");

            var response = new GetStudentsResponse
            {
                Students = _studentRepository.GetStudents().ToDto(),
            };

            _logger.LogInformation("Get students request succeeded with response {@Response}", response);

            return Ok(response);
        }

        [HttpPost]
        public ActionResult<AddStudentResponse> AddStudent(AddStudentRequest request)
        {
            _logger.LogInformation("Add Student request received: {@Request}", request);

            var neptunCode = _studentRepository.AddStudent(request.ToStudentEntity());

            var response = new AddStudentResponse
            {
                NeptunCode = neptunCode,
            };

            _logger.LogInformation("Add Student request succeded with response {@Response}", response);

            return Ok(response);
        }

        [HttpDelete("{neptunCode}")]
        public ActionResult DeleteStudent(Guid neptunCode)
        {
            _studentRepository.DeleteStudent(neptunCode);
            return NoContent();
        }

        [HttpPut("{neptunCode}")]
        public ActionResult UpdateStudent(Guid neptunCode, [FromBody] UpdateStudentRequest request)
        {
            _studentRepository.UpdateStudent(neptunCode, request.ToStudentEntity());
            return NoContent();
        }

        [HttpGet("{neptunCode}")]
        public ActionResult<GetStudentResponse> GetStudent(Guid neptunCode)
        {
            var student = _studentRepository.GetStudent(neptunCode);

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
