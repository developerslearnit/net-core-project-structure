using Microsoft.AspNetCore.Mvc;

namespace ShephardTech.Web.Areas.Auth.Controllers
{
    [Area("auth")]
    [Route("auth")]
    public class AuthenticationController : Controller
    {

        [Route("login")]
        public IActionResult login()
        {
            return View();
        }
    }
}
