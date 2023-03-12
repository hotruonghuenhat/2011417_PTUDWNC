using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.Admin.Controllers {
    public class SubscribersController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}
