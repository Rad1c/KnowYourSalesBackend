using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class LivenessController : BaseController
    {
        [HttpGet("check/{message}")]
        public IActionResult Check(string message)
        {
            return Ok($"Message: {message}");
        }
    }
}
