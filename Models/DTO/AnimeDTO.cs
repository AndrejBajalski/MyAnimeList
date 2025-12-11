using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class AnimeDTO
    {
        public int mal_id { get; set; } 
        public string? title_english { get; set; }
        public string? title_japanese { get; set; }
        public string? synopsis { get; set; }
        public int? year { get; set; }
        public List<GenreDTO>? genres { get; set; }
        //public List<string>? studios { get; set; }
        public float? score { get; set; }
        public int? episodes { get; set; }
        public ImagesDTO? images { get; set; }

    }

    public class ImagesDTO
    {
        public JpgImageDTO? jpg { get; set; }
    }
    public class JpgImageDTO
    {
        public string? image_url { get; set; }
        public string? small_image_url { get; set; }
        public string? large_image_url { get; set; }    
    }
    public class GenreDTO
    {
        public string name { get; set; } = "";
    }
}
