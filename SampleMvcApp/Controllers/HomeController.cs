using System.Web.Mvc;
using SampleMvcApp.Models;

namespace SampleMvcApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

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
