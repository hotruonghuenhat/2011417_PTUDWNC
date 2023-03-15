using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Middlewares;

namespace TatBlog.WebApp.Extentions {
    public static class WebApplicationExtensions {
        public static WebApplicationBuilder ConfigureMvc(this WebApplicationBuilder builder) {
            builder.Services.AddControllersWithViews();
            builder.Services.AddResponseCompression();

            return builder;
        }

        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder) {
            builder.Services.AddDbContext<BlogDbContext>(
       options => options.UseSqlServer(
           builder.Configuration.GetConnectionString("DefaultConnection"))
       );


            builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<IDataSeeder, DataSeeder>();


            return builder;
        }

        public static WebApplicationBuilder ConfigureNLog(this WebApplicationBuilder builder) {
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            return builder;
        }

        public static WebApplication UserRequestPipeline(this WebApplication app) {
            // Cấu hình HTTP Request pipeline

            // Thêm middleware để hiển thị thông báo lỗi
            if (app.Environment.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Blog/Error");

                // Thêm middleware cho việc áp dụng HSTS (Thêm header Strict-Transport-Security vào HTTP Response
                app.UseHsts();
            }

            // Thêm middleware để chuyển hướng HTTP sang HTTPS
            app.UseHttpsRedirection();

            // Thêm midleware phục vụ các yêu cầu liên quan tới các tập tin tĩnh
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<UserActivityMiddleware>();

            return app;
        }

        public static IApplicationBuilder UseDataSeeder(this IApplicationBuilder app) {
            using (var scope = app.ApplicationServices.CreateScope()) {
                try {
                    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
                    seeder.Initialize();
                }
                catch (Exception ex) {
                    scope.ServiceProvider.GetRequiredService<ILogger<Program>>()
                        .LogError(ex, "Could not insert data into database");
                }
            }
            return app;
        }
    }
}
