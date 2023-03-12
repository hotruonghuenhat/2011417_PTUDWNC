using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.Admin.Controllers {
    public class CategoriesController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}
