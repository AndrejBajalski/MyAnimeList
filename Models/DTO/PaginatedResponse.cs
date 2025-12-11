using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DomainModels;

namespace Models.DTO
{
    public class PaginatedResponse
    {
        public Pagination Page { get; set; }
        public List<Anime> Animes { get; set; }
    }
}
