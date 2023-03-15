using TatBlog.WebApp.Extentions;
using TatBlog.WebApp.Mapsters;
using TatBlog.WebApp.Validations;

var builder = WebApplication.CreateBuilder(args); {
    builder.ConfigureMvc()
    .ConfigureNLog()
    .ConfigureServices()
    .ConfigureMapster()
    .ConfigureFluentValidation();
}

var app = builder.Build(); {
    app.UserRequestPipeline();
    app.UseBlogRoutes();
    app.UseDataSeeder();
}

app.Run();
