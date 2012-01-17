using System.Web.Mvc;
using Curiosity.Common.Mvc;
using SampleMvcApp.Models;

namespace SampleMvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Flash()
        {
            TempData.Flash(new { success = "This is a success message." });
            TempData.Flash(new { error = "This is an error message."});
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TestForm()
        {
            var form = new SampleForm();
            return View(form);
        }

        [HttpPost]
        public ActionResult TestForm(SampleForm form)
        {
            return RedirectToAction("TestForm", "Home");
        }
    }
}
