using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using VideoOnDemand.Models;

using VideoOnDemand.Repositories;

namespace VideoOnDemand.Controllers
{
    public class HomeController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;

        public HomeController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var rep = new MockReadRepository();
            var courses = rep.GetCourses("0830e0ea-293c-41d4-9d28-3c0b36e323f7");
            var course = rep.GetCourse("0830e0ea-293c-41d4-9d28-3c0b36e323f7", 1);
            var video = rep.GetVideo("0830e0ea-293c-41d4-9d28-3c0b36e323f7", 1);
            var videos = rep.GetVideos("0830e0ea-293c-41d4-9d28-3c0b36e323f7", 1);
            
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
