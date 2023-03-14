using TatBlog.WebApp.Extentions;
using TatBlog.WebApp.Mapsters;

var builder = WebApplication.CreateBuilder(args); {
    builder.ConfigureMvc()
        .ConfigureServices()
        .ConfigureMapster();
}

var app = builder.Build(); {
    app.UserRequestPipeline();
    app.UseBlogRoutes();
    app.UseDataSeeder();
}

app.Run();
