using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Models.Store;
using MotoShop.WebAPI.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.Web.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly DatabaseSeeder dbSeeder;
        private readonly ApplicationDatabaseContext _applicationDatabaseContext;

        public HomeController(ApplicationDatabaseContext applicationDatabaseContext, DatabaseSeeder dbSeeder)
        {
            _applicationDatabaseContext = applicationDatabaseContext;
            this.dbSeeder = dbSeeder;
        }

        [HttpGet("[action]")]
        public async Task <IActionResult> Index()
        {

            Advertisement advertisement = _applicationDatabaseContext.Advertisements
                .Where(x => x.ID == 16)
                .Include(x => x.Author)
                .Include(x => x.ShopItem)
                .FirstOrDefault();

            return Ok(advertisement);
        }
    }
}
