using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.Admin.Controllers {
    public class DashboardController :Controller
    {
        public ActionResult Index() {
            return View();
        }
    }
}
