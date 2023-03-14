using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components {
    public class Top5PostsWidget : ViewComponent {
        private readonly IBlogRepository _blogRepository;

        public Top5PostsWidget(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var posts = await _blogRepository.GetPostsByQualAsync(5);

            return View(posts);
        }
    }
}
