using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components {
    [ViewComponent(Name = "Top4AuthorsWidget")]
    public class BestAuthorsWidget : ViewComponent {
        private readonly IBlogRepository _blogRepository;

        public BestAuthorsWidget(IBlogRepository blogRepository) {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var authors = await _blogRepository.GetAuthorsMostPost(4);
            return View(authors);
        }
    }
}
