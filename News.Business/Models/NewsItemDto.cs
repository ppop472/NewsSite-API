using News.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Business.Models
{
    public class NewsItemDto
    {
        public int Id { get; set; } = 0;
        public string Source { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Content { get; set; } = "";
        public string HtmlContent { get; set; } = "";
        public string Headline { get; set; } = "";
        public string LinkUrl { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string ThumbUrl { get; set; } = "";
        public string Language { get; set; } = "";
        public DateTime ?PublishedAt { get; set; }
    }
}

