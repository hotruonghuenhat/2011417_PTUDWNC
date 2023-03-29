using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components;
public class Top3PostWidget : ViewComponent {
    private readonly IBlogRepository _blogRepository;

    public Top3PostWidget(IBlogRepository blogRepository) {
        _blogRepository = blogRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync() {
        var posts = await _blogRepository.GetPopularArticleAsync(3);

        return View(posts);
    }
}