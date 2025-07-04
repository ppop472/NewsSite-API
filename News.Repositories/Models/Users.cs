using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Repositories.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public int SourceId { get; set; } = 0;
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public int Telephone_number { get; set; } = 0;
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
