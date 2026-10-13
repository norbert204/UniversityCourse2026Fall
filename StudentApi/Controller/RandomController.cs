using Microsoft.AspNetCore.Mvc;

namespace StudentApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class RandomController : ControllerBase
    {
        private readonly RandomService _randomService;
        public RandomController(RandomService randomService)
        {
            _randomService = randomService;
        }

        [HttpGet]
        public int GetRandomNumber()
        {
            return _randomService.RandomNumber;
        }
    }
}
