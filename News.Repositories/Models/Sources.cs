namespace News.Repositories.Models
{
    public class Sources
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public string TypeId { get; set; } = "";
        public string Url { get; set; } = "";
        public string Module { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
