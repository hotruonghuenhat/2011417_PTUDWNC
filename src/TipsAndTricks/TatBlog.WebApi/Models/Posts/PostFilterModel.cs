using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace TatBlog.WebApi.Models;
public class PostFilterModel : PagingModel {

    public bool PublishedOnly { get; set; }
    public bool NotPublished { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }

    public IEnumerable<SelectListItem> AuthorList { get; set; }
    public IEnumerable<SelectListItem> CategoryList { get; set; }
}
