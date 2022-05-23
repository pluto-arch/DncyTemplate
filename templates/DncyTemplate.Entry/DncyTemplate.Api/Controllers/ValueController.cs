using Microsoft.AspNetCore.Mvc;

namespace DncyTemplate.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Index()
        {
            return Enumerable.Repeat<string>("111", 200);
        }
    }
}