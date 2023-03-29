namespace TatBlog.Core.DTO;

public class PostItem {
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public string Meta { get; set; }
    public string UrlSlug { get; set; }
    public string ImageUrl { get; set; }
    public int ViewCount { get; set; }
    public bool Published { get; set; }
    public DateTime PostedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string CategoryName { get; set; }
    public string AuthorName { get; set; }
    public IList<string> Tags { get; set; }
    public object SelectedTags { get; set; }
    public object CategoryList { get; set; }
    public object AuthorList { get; set; }
    public object ImageFile { get; set; }
}