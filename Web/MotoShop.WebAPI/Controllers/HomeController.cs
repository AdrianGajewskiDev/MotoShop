using Microsoft.AspNetCore.Mvc;

namespace MotoShop.Web.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("Home index value");
        }
    }
}
