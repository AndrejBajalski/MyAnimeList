using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DomainModels
{
    public class Review : BaseEntity
    {
        public required Guid AnimeId { get; set; }
        public Anime? anime { get; set; }
        public required string UserId { get; set; }
        public User? User { get; set; }
        public string? Content { get; set; }
        public int? Rating { get; set; }
        public DateTime LastModified { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
