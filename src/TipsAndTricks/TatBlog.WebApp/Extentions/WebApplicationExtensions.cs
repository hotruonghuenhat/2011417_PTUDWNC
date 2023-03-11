using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Extentions {
    public static class WebApplicationExtensions {
        //thêm các dịch vụ được yêu cầu bởi MVC framework
        public static WebApplicationBuilder ConfigureMvc(this WebApplicationBuilder builder) {
            builder.Services.AddControllersWithViews();
            builder.Services.AddResponseCompression();

            return builder;
        }
        //đăng ký các dịch vụ với DI container
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder) {
            builder.Services.AddDbContext<BlogDbContext>(
       options => options.UseSqlServer(
           builder.Configuration.GetConnectionString("DefaultConnection"))
       );

            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<IDataSeeder, DataSeeder>();


            return builder;
        }
        //cấu hình HTTP Requét pipeline
        public static WebApplication UserRequestPipeline(this WebApplication app) {

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

            return app;
        }

        //thêm dữ liệu mẫu vào CSDL
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