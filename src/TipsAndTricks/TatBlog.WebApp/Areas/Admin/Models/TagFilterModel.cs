using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models {
    public class TagFilterModel {
        [DisplayName("Từ khoá")]
        public string KeyWord { get; set; } = string.Empty;
    }
}
