using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DomainModels
{
    public class Anime : BaseEntity
    {
        public string? Name { get; set; }
        public string? NameJapanese { get; set; }
        public string? Description { get; set; }
        public int? YearAired { get; set; }
        public float? Rating { get; set; }
        public int? NumberOfEpisodes { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageUrlSmall { get; set; }
        public List<string> Genres { get; set; }
        //public List<string>? studios { get; set; }
    }
}
