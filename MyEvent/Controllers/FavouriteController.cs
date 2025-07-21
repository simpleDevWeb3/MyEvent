using Microsoft.AspNetCore.Mvc;

namespace MyEvent.Controllers
{
    public class FavouriteController : Controller
    {
        [Route("/Followed")]
        public IActionResult Followed()
        {
            return View();
        }
    }
}
