using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers {
    public class BlogController : Controller {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;
        }

        //Action này xử lý Htttp request đến trang chủ của
        //ứng dụng web hoặc tìm kiếm bài viết theo từ khóa
        public async Task<IActionResult> Index(
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3) {

            //tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery() {
                //chỉ lấy những bài viết có trạng thái Published
                PublishedOnly = true,
                //tìm bài viết theo từ khóa
                KeyWord = keyword,
            };

            //Truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);

            //Lưu lại điều kiện truy vấn để hiển thị trong View
            ViewBag.PostQuery = postQuery;

            // Truyền danh sách bài viết vào View để render ra HTML
            return View(postsList);
        }

        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");
    }
}