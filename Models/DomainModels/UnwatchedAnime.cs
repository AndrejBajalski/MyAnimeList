using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;
namespace Models.DomainModels
{
    public class UnwatchedAnime : BaseEntity
    {
        public required string UserId { get; set; }
        public User User { get; set; }
        public required Guid AnimeId { get; set; }
        public Anime Anime { get; set; }
        public AnimeStatus Status { get; set; }
    }
}
