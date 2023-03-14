using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extentions;

namespace TatBlog.Services.Blogs {
    public class BlogRepository : IBlogRepository {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context) {
            _context = context;
        }
        // Lấy danh sách chuyên mục và số lượng bài viết 
        // nằm thuộc từng chuyên mục/chủ đề
        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default) {
            IQueryable<Category> categories = _context.Set<Category>();

            if (showOnMenu) {
                categories = categories.Where(x => x.ShowOnMenu);
            }

            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem() {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }
        //tìm bài viết có lượt xem nhiều, phố biến
        public async Task<IList<Post>> GetPopularArticleAsync(int numPosts, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default) {
            IQueryable<Post> postQuery = _context.Set<Post>().Include(x => x.Category).Include(x => x.Author);
            if (year > 0) {
                postQuery = postQuery.Where(x => x.PostedDate.Year == year);
            }
            if (month > 0) {
                postQuery = postQuery.Where(x => x.PostedDate.Month == month);
            }
            if (!string.IsNullOrWhiteSpace(slug)) {
                postQuery = postQuery.Where(x => x.UrlSlug == slug);
            }
            return await postQuery.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default) {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }

        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem() {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<Tag> FindTagBySlugAsync(string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Tag>()
                .Where(x => x.UrlSlug == slug)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<TagItem>> FindTagItemSlugAsync(CancellationToken cancellationToken = default) {
            var query = _context.Set<Tag>()
                       .Select(x => new TagItem() {
                           Id = x.Id,
                           Name = x.Name,
                           UrlSlug = x.UrlSlug,
                           Description = x.Description,
                           PostCount = x.Posts.Count(p => p.Published)
                       });
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Tag>()
                .Where(t => t.Id == id).ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<Category> FindCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Category>()
                    .FirstOrDefaultAsync(c => c.UrlSlug.Equals(slug), cancellationToken);
        }

        public async Task<Category> FindCategoryByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Category>()
                .FindAsync(id, cancellationToken);
        }

        public async Task<bool> AddOrEditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default) {
            _context.Entry(newCategory).State = newCategory.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Category>()
                .Where(c => c.Id == id).ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<bool> IsSlugOfCategoryExist(string slug, CancellationToken cancellationToken = default) {
            return await _context.Set<Category>().AnyAsync(c => c.UrlSlug.Equals(slug), cancellationToken);
        }

        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default) {
            var categoriesQuery = _context.Set<Category>()
                .Select(x => new CategoryItem() {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await categoriesQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

        public Task<object> CountByMostRecentMonthAsync(int month, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }

        public async Task<Post> FindPostByIdAsync(int id, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>().FindAsync(id, cancellationToken);
        }

        public async Task<bool> AddOrUpdatePostAsync(Post post, CancellationToken cancellationToken = default) {
            _context.Entry(post).State = post.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task ChangeStatusPublishedOfPostAsync(int id, CancellationToken cancellationToken = default) {
            await _context.Set<Post>().Where(p => p.Id == id).ExecuteUpdateAsync(p => p.SetProperty(
                x => x.Published, x => !x.Published), cancellationToken);
        }

        public async Task<IList<Post>> GetPostsByQualAsync(int num, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .OrderBy(x => x.Id)
                .Take(num)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Post>> FindPostByPostQueryAsync(PostQuery query, CancellationToken cancellationToken = default) {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .Where(
                    p => p.AuthorId == query.AuthorId
                    || p.CategoryId == query.CategoryId
                    || p.Category.UrlSlug.Equals(query.CategorySlug)
                    || p.PostedDate.Month == query.Month
                    || p.PostedDate.Year == query.Year
                    || p.Tags.Any(tagName => tagName.Name.Equals(query.Tag)))
                    .ToListAsync(cancellationToken);
        }

        public async Task<int> CountPostsOfPostQueryAsync(PostQuery query, CancellationToken cancellationToken = default) {
            var posts = await FindPostByPostQueryAsync(query);
            return posts.Count();
        }

        public async Task<IPagedList<Post>> GetPagedPostByPostQueryAsync(IPagingParams pagingParams, PostQuery query, CancellationToken cancellationToken = default) {
            var posts = _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .Where(
                    p => p.Published == query.PublishedOnly && p.AuthorId == query.AuthorId
                    || p.CategoryId == query.CategoryId
                    || p.Category.UrlSlug.Equals(query.CategorySlug)
                    || p.PostedDate.Month == query.Month
                    || p.PostedDate.Year == query.Year
                    || p.Tags.Any(tagName => tagName.Name.Equals(query.Tag)));

            return await posts.ToPagedListAsync(pagingParams, cancellationToken);
        }

        private IQueryable<Post> FilterPosts(PostQuery condition) {
            IQueryable<Post> posts = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);

            if (condition.PublishedOnly) {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished) {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.CategoryId > 0) {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug)) {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.AuthorId > 0) {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug)) {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug)) {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Tag)) {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.Tag));
            }

            if (!string.IsNullOrWhiteSpace(condition.KeyWord)) {
                posts = posts.Where(x => x.Title.Contains(condition.KeyWord) ||
                                         x.ShortDescription.Contains(condition.KeyWord) ||
                                         x.Description.Contains(condition.KeyWord) ||
                                         x.Category.Name.Contains(condition.KeyWord) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.KeyWord)));
            }

            if (condition.Year > 0) {
                posts = posts.Where(x => x.PostedDate.Year == condition.Year);
            }

            if (condition.Month > 0) {
                posts = posts.Where(x => x.PostedDate.Month == condition.Month);
            }
            if (condition.Day > 0) {
                posts = posts.Where(x => x.PostedDate.Day == condition.Day);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug)) {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }

            return posts;
        }

        public async Task<Post> GetPostsAsync(PostQuery query, CancellationToken cancellationToken = default) {
            IQueryable<Post> postQuery = _context.Set<Post>().Include(x => x.Category).Include(x => x.Author);
            if (query.Year > 0) {
                postQuery = postQuery.Where(x => x.PostedDate.Year == query.Year);
            }
            if (query.Month > 0) {
                postQuery = postQuery.Where(x => x.PostedDate.Month == query.Month);
            }
            if (query.Day > 0) {
                postQuery = postQuery.Where(x => x.PostedDate.Day == query.Day);
            }
            if (!string.IsNullOrWhiteSpace(query.UrlSlug)) {
                postQuery = postQuery.Where(x => x.UrlSlug == query.UrlSlug);
            }
            return await postQuery.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<TagItem>> GetListTagItemAsync(CancellationToken cancellationToken = default) {
            IQueryable<Tag> tagItems = _context.Set<Tag>();

            return await tagItems
                .Select(x => new TagItem() {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                })
            .ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<Post>> GetPagedPostsAsync(
        PostQuery condition,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default) {
            return await FilterPosts(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Post.PostedDate), "DESC",
                cancellationToken);
        }
        public async Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken = default) {
            return await _context.Set<Author>().Include(a => a.Posts).ToListAsync(cancellationToken);
        }

        public async Task GetPostByIdAsync(int id, bool v) {
            
        }
    }
}
