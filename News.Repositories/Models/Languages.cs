namespace News.Repositories.Models
{
    public class Languages
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Short { get; set; } = "";
        public string Code { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
