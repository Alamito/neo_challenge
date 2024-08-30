using Microsoft.AspNetCore.Mvc;

namespace ChallengeNeo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHelloWorld()
        {
            var response = new { message = "Hello Worldddd", test = 1+1 };
            return Ok(response);
        }
    }
}
