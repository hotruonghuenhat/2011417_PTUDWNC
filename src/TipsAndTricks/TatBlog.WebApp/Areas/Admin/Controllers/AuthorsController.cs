using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.Admin.Controllers {
    public class AuthorsController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}
