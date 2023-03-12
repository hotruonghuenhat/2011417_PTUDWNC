using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.Admin.Controllers {
    public class PostsController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}
