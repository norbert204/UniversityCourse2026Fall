using Microsoft.AspNetCore.Mvc;

namespace StudentApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<string> GetStudentName(int id)
        {
            if (id == 0)
            {
                return Ok("Teszt Elek");
            }

            if (id == 1)
            {
                return Ok("Kiss Pista");
            }

            return NotFound();
        }
    }
}
