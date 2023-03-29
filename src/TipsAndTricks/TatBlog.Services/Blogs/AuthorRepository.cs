using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extentions;

namespace TatBlog.Services.Blogs;

public class AuthorRepository : IAuthorRepository {
    private readonly BlogDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public AuthorRepository(BlogDbContext context, IMemoryCache memoryCache) {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<Author> GetAuthorBySlugAsync(
        string slug, CancellationToken cancellationToken = default) {
        return await _context.Set<Author>()
            .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
    }

    public async Task<Author> GetCachedAuthorBySlugAsync(
        string slug, CancellationToken cancellationToken = default) {
        return await _memoryCache.GetOrCreateAsync(
            $"author.by-slug.{slug}",
            async (entry) => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetAuthorBySlugAsync(slug, cancellationToken);
            });
    }

    public async Task<Author> GetAuthorByIdAsync(int authorId) {
        return await _context.Set<Author>().FindAsync(authorId);
    }

    public async Task<Author> GetCachedAuthorByIdAsync(int authorId) {
        return await _memoryCache.GetOrCreateAsync(
            $"author.by-id.{authorId}",
            async (entry) => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetAuthorByIdAsync(authorId);
            });
    }

    public async Task<IList<AuthorItem>> GetAuthorsAsync(
        CancellationToken cancellationToken = default) {
        return await _context.Set<Author>()
            .OrderBy(a => a.FullName)
            .Select(a => new AuthorItem() {
                Id = a.Id,
                FullName = a.FullName,
                Email = a.Email,
                JoinedDate = a.JoinedDate,
                ImageUrl = a.ImageUrl,
                UrlSlug = a.UrlSlug,
                PostCount = a.Posts.Count(p => p.Published)
            })
            .ToListAsync(cancellationToken);
    }

    //public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
    //    IPagingParams pagingParams,
    //    string name = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    return await _context.Set<Author>()
    //        .AsNoTracking()
    //        .WhereIf(!string.IsNullOrWhiteSpace(name),
    //            x => x.FullName.Contains(name))
    //        .Select(a => new AuthorItem()
    //        {
    //            Id = a.Id,
    //            FullName = a.FullName,
    //            Email = a.Email,
    //            JoinedDate = a.JoinedDate,
    //            ImageUrl = a.ImageUrl,
    //            UrlSlug = a.UrlSlug,
    //            PostCount = a.Posts.Count(p => p.Published)
    //        })
    //        .ToPagedListAsync(pagingParams, cancellationToken);
    //}

    public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
    IPagingParams pagingParams,
    string name = null,
    CancellationToken cancellationToken = default) {
        IQueryable<Author> authorQuery = _context.Set<Author>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(name)) {
            authorQuery = authorQuery.Where(x => x.FullName.Contains(name));
        }

        return await authorQuery.Select(a => new AuthorItem() {
            Id = a.Id,
            FullName = a.FullName,
            Email = a.Email,
            JoinedDate = a.JoinedDate,
            ImageUrl = a.ImageUrl,
            UrlSlug = a.UrlSlug,
            PostCount = a.Posts.Count(p => p.Published)
        })
               .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
        Func<IQueryable<Author>, IQueryable<T>> mapper,
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default) {
        var authorQuery = _context.Set<Author>().AsNoTracking();

        if (!string.IsNullOrEmpty(name)) {
            authorQuery = authorQuery.Where(x => x.FullName.Contains(name));
        }

        return await mapper(authorQuery)
            .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<bool> AddOrUpdateAsync(
        Author author, CancellationToken cancellationToken = default) {
        if (author.Id > 0) {
            _context.Authors.Update(author);
            _memoryCache.Remove($"author.by-id.{author.Id}");
        }
        else {
            _context.Authors.Add(author);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAuthorAsync(
        int authorId, CancellationToken cancellationToken = default) {
        return await _context.Authors
            .Where(x => x.Id == authorId)
            .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public async Task<bool> IsAuthorSlugExistedAsync(
        int authorId,
        string slug,
        CancellationToken cancellationToken = default) {
        return await _context.Authors
            .AnyAsync(x => x.Id != authorId && x.UrlSlug == slug, cancellationToken);
    }

    public async Task<bool> SetImageUrlAsync(
        int authorId, string imageUrl,
        CancellationToken cancellationToken = default) {
        return await _context.Authors
            .Where(x => x.Id == authorId)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(a => a.ImageUrl, a => imageUrl),
                cancellationToken) > 0;
    }

    public async Task<List<AuthorItem>> GetAuthorsHasMostPost(int numberOfAuthors, CancellationToken cancellationToken = default) {
        return await _context.Authors
            .Include(post => post.Posts)
            .Select(a => new AuthorItem() {
                Id = a.Id,
                UrlSlug = a.UrlSlug,
                ImageUrl = a.ImageUrl,
                Email = a.Email,
                FullName = a.FullName,
                Notes = a.Notes,
                PostCount = a.Posts.Count(p => p.Published)
            })
            .OrderBy(a => a.FullName)
            .Take(numberOfAuthors)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> CheckAuthorSlugExisted(int id, string slug, CancellationToken cancellationToken = default) {
        return await _context.Set<Author>()
            .AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
    }
    public async Task<bool> AddOrUpdateAuthorAsync(Author author, CancellationToken cancellationToken = default) {
        if (author.Id > 0)
            _context.Update(author);
        else
            _context.Add(author);

        var result = await _context.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<bool> DeleteAuthorByIdAsync(int? id, CancellationToken cancellationToken = default) {
        var author = await _context.Set<Author>().FindAsync(id);

        if (author is null) return await Task.FromResult(false);

        _context.Set<Author>().Remove(author);
        var rowsCount = await _context.SaveChangesAsync(cancellationToken);

        return rowsCount > 0;
    }

    public async Task<IList<Author>> Find_N_MostPostByAuthorAsync(int n, CancellationToken cancellationToken = default) {
        IQueryable<Author> authorsQuery = _context.Set<Author>();
        IQueryable<Post> postsQuery = _context.Set<Post>();

        return await authorsQuery.Join(postsQuery, a => a.Id, p => p.AuthorId,
                                    (author, post) => new {
                                        author.Id
                                    })
                                 .GroupBy(x => x.Id)
                                 .Select(x => new {
                                     AuthorId = x.Key,
                                     Count = x.Count()
                                 })
                                 .OrderByDescending(x => x.Count)
                                 .Take(n)
                                 .Join(authorsQuery, a => a.AuthorId, a2 => a2.Id,
                                  (preQuery, author) => new Author {
                                      Id = author.Id,
                                      FullName = author.FullName,
                                      UrlSlug = author.UrlSlug,
                                      ImageUrl = author.ImageUrl,
                                      JoinedDate = author.JoinedDate,
                                      Notes = author.Notes,
                                  }).ToListAsync();
    }
}