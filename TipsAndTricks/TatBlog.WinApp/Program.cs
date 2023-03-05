using System;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;

//Tạo đối tượng DbContext để quản lý phiên làm việc với CSDL và trạng thái của các đối tượng
var context = new BlogDbContext();

var seeder = new DataSeeder(context);

seeder.Initialize();

//tạo đối tượng BlogRepository
IBlogRepository blogRepo = new BlogRepository(context);

var pagingParams = new PagingParams
{
    PageNumber = 1,
    PageSize = 5,
    SortColumn = "Name",
    SortOrder = "DESC"
};

////tìm 3 bài viết được xem và đọc nhiều nhất
//var posts = await blogRepo.GetPopularArticlesAsync(3);

////Xuất danh sách bài viết ra màn hình
//foreach (var post in posts)
//{
//    Console.WriteLine("ID      : {0}", post.Id);
//    Console.WriteLine("Title   : {0}", post.Title);
//    Console.WriteLine("View    : {0}", post.ViewCount);
//    Console.WriteLine("Date    : {0:MM/dd/yyyy}", post.PostedDate);
//    Console.WriteLine("Author  : {0}", post.Author);
//    Console.WriteLine("Category: {0}", post.Category);
//    Console.WriteLine("".PadRight(80, '-'));
//}

////đọc danh sách tác giả từ CSDL
//var authors = context.Authors.ToList();

////xuất danh sách tác giả ra màn hình
//Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}",
//    "ID", "FullName", "Email", "JoinedDate");

//foreach (var author in authors)
//{
//    Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}",
//        author.Id, author.FullName, author.Email, author.JoinedDate);
//}

//Console.WriteLine("".PadRight(80, '-'));

//var tagsList = context.Tags.ToList();

////xuất danh sách tag ra màn hình
//Console.WriteLine("{0,-5}{1,-50}",
//    "ID", "Name");

//foreach (var item in tagsList)
//{
//    Console.WriteLine("{0,-5}{1,-50}",
//        item.Id, item.Name);
//}

//var tagsSlugList = await blogRepo.GetTagSlugAsync("google-applications");

////xuất danh sách tag ra màn hình
//Console.WriteLine("{0,-5}{1,-50}",
//    "ID", "Name");
//Console.WriteLine("{0,-5}{1,-50}",
//    tagsSlugList.Id, tagsSlugList.Name);



//var categorySlugList = await blogRepo.GetTagSlugAsync("google-applications");

////xuất danh sách tag ra màn hình
//Console.WriteLine("{0,-5}{1,-50}",
//    "ID", "Name");
//Console.WriteLine("{0,-5}{1,-50}",
//    categorySlugList.Id, categorySlugList.Name);


var categoryNumberList = await blogRepo.GetCategoryNumerAsync(3);

//xuất danh sách tag ra màn hình
Console.WriteLine("{0,-5}{1,-50}",
    "ID", "Name");
Console.WriteLine("{0,-5}{1,-50}",
    categoryNumberList.Id, categoryNumberList.Name);