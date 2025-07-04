namespace News.Repositories.Models
{
    public class NewsItem
    {
        public int Id { get; set; } = 0;
        public string Guid { get; set; } = "";
        public int SourceId { get; set; } = 0;
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Content { get; set; } = "";
        public string HtmlContent { get; set; } = "";
        public string Headline { get; set; } = "";
        public string LinkUrl { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string ThumbUrl { get; set; } = "";
        public int LanguageId { get; set; } = 0;
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? Sent { get; set; }
    }
}
