using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _context;

    public BlogRepository(BlogDbContext context) => _context = context;

    //Tìm bài viết có tên định danh là slug
    //và được đang vào tháng 'month' năm 'year'
    public async Task<Post> GetPostAsync(
    int year,
    int month,
    string slug,
    CancellationToken cancellationToken = default)
    {
        IQueryable<Post> postsQuery = _context.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author);

        if (year > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
        }
        if (month > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
        }
        if (!string.IsNullOrWhiteSpace(slug))
        {
            postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
        }
        return await postsQuery.FirstOrDefaultAsync(cancellationToken);
    }

    //tìm bài viết có lượt xem nhiều, phố biến
    public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Post>()
            .Include(x => x.Author)
            .Include(x => x.Category)
            .OrderByDescending(p => p.ViewCount)
            .Take(numPosts)
            .ToListAsync(cancellationToken);
    }



    // Tăng số lượt xem của một bài viết
    public async Task IncreaseViewCountAsync(
    int postId,
    CancellationToken cancellationToken = default)
    {

        await _context.Set<Post>()
            .Where(x => x.Id == postId)
            .ExecuteUpdateAsync(p =>
            p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
            cancellationToken);
    }

    // Lấy danh sách chuyên mục và số lượng bài viết 
    // nằm thuộc từng chuyên mục/chủ đề
    public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Category> categories = _context.Set<Category>();
        if (showOnMenu)
        {
            categories = categories.Where(x => x.ShowOnMenu);
        }

        return await categories
        .OrderBy(x => x.Name)
        .Select(x => new CategoryItem()
        {
            Id = x.Id,
            Name = x.Name,
            UrlSlug = x.UrlSlug,
            Description = x.Description,
            ShowOnMenu = x.ShowOnMenu,
            PostCount = x.Posts.Count(p => p.Published)
        })
        .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsPostSlugExistedAsync(int postId,
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Post>()
            .AnyAsync(x => x.Id != postId && x.UrlSlug == slug,
            cancellationToken);
    }

    public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams, 
        CancellationToken cancellationToken = default)
    {
        var tagQuery = _context.Set<Tag>()
            .Select(x => new TagItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                PostCount = x.Posts.Count(p => p.Published)
            });

        return await tagQuery
            .ToPagedListAsync(pagingParams, cancellationToken);
    }

    //Tìm Tag có tên định danh là slug
    public async Task<Tag> GetTagSlugAsync(
    string slug,
    CancellationToken cancellationToken = default)
    {
        IQueryable<Tag> tagsQuery = _context.Set<Tag>();

            tagsQuery = tagsQuery.Where(x => x.UrlSlug == slug);
        return await tagsQuery.FirstOrDefaultAsync(cancellationToken);
    }

    //Tìm chuyên mục có tên định danh là slug
    public async Task<Category> GetCategorySlugAsync(
    string slug,
    CancellationToken cancellationToken = default)
    {
        IQueryable<Category> tagsQuery = _context.Set<Category>();

        tagsQuery = tagsQuery.Where(x => x.UrlSlug == slug);
        return await tagsQuery.FirstOrDefaultAsync(cancellationToken);
    }


    //Tìm chuyên mục có mã số cho trước
    public async Task<Category> GetCategoryNumerAsync(
    int id,
    CancellationToken cancellationToken = default)
    {
        IQueryable<Category> tagsQuery = _context.Set<Category>();

        tagsQuery = tagsQuery.Where(x => x.Id == id);
        return await tagsQuery.FirstOrDefaultAsync(cancellationToken);
    }
}