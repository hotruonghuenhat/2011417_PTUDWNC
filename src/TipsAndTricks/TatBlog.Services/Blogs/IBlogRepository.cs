﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
    // Tìm bài viết có tên định danh là 'slug' 
    // và được đăng vào tháng 'month' năm 'year'
    Task<Post> GetPostAsync(
    int year,
    int month,
    string slug,
    CancellationToken cancellationToken = default);

    // Tìm Top N bài viết phổ được nhiều người xem nhất 
    Task<IList<Post>> GetPopularArticlesAsync(
    int numPosts,
    CancellationToken cancellationToken = default);

    // Kiểm tra xem tên định danh của bài viết đã có hay chưa
    Task<bool> IsPostSlugExistedAsync(int postId, string slug,
    CancellationToken cancellationToken = default);

    // Tăng số lượt xem của một bài viết
    Task IncreaseViewCountAsync(
    int postId,
    CancellationToken cancellationToken = default);

    Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false,
    CancellationToken cancellationToken = default);

    Task <IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, 
        CancellationToken cancellationToken = default);

}