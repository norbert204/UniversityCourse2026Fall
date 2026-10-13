using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using StudentApi.Services;
using System.Net;

namespace StudentApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }


        [HttpGet]
        public ActionResult<List<Student>> GetStudents()
        {
            return Ok(_studentRepository.GetStudents());
        }

        [HttpPost]
        public ActionResult AddStudent(Student student)
        {
            try
            {
                _studentRepository.AddStudent(student);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
                //return BadRequest(
                //    new ProblemDetails
                //    {
                //        Title = "Bad Request",
                //        Status = (int)HttpStatusCode.BadRequest,
                //        Detail = ex.Message,
                //    });
            }
        }

        [HttpDelete("{neptunCode}")]
        public ActionResult DeleteStudent(string neptunCode)
        {
            _studentRepository.DeleteStudent(neptunCode);
            return NoContent();
        }

        [HttpPut("{neptunCode}")]
        public ActionResult UpdateStudent(string neptunCode, [FromBody] Student student)
        {
            _studentRepository.UpdateStudent(neptunCode, student);
            return NoContent();
        }

        [HttpGet("{neptunCode}")]
        public ActionResult GetStudent(string neptunCode)
        {
            var student = _studentRepository.GetStudent(neptunCode);

            if (student is not null)
            {
                return Ok(student);
            }

            return NotFound();
        }
    }
}
