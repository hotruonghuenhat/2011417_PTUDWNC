using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Extentions;

var builder = WebApplication.CreateBuilder(args); {
    builder.ConfigureMvc()
        .ConfigureServices();
}

var app = builder.Build(); {
    app.UserRequestPipeline();
    app.UseBlogRoutes();
    app.UseDataSeeder();
}

app.Run();
