namespace TatBlog.Core.DTO;

public class PostQuery {
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }
    public string CategorySlug { get; set; }
    public string TitleSlug { get; set; }
    public string TagSlug { get; set; }
    public string AuthorSlug { get; set; }
    public string UrlSlug { get; set; }
    public string PostSlug { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int Day { get; set; }
    public bool PublishedOnly { get; set; }
    public bool NotPublished { get; set; }
    public string Tag { get; set; }
    public string KeyWord { get; set; }
}