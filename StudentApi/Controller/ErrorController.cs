using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Exceptions;

namespace StudentApi.Controller
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var exception = feature.Error;

            if (exception is EntityNotFoundException)
            {
                return NotFound();
            }

            return Problem(detail: exception.Message);
        }
    }
}
