using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models {
    public class CategoryFilterModel {
        [DisplayName("Từ khoá")]
        public string KeyWord { get; set; } = string.Empty;

        [DisplayName("Hiển thị")]
        public bool ShowOnMenu { get; set; } = false;
    }
}
