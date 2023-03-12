using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.Admin.Controllers {
    public class CommentsController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}
