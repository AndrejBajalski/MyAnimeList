using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class AllAnimesResponseDTO
    {
        public List<AnimeDTO>? data { get; set; }
        public Pagination? pagination { get; set; }

    }
    public class Pagination
    {
        public int last_visible_page { get; set; }
        public bool has_next_page { get; set; }
        public int current_page { get; set; }
    }
}
