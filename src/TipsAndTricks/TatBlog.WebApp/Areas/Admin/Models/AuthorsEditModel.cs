namespace TatBlog.WebApp.Areas.Admin.Models {
    public class AuthorsEditModel {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UrlSlug { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
    }
}
