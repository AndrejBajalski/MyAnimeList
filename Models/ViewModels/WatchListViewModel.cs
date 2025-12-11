using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DomainModels;

namespace Models.NewFolder
{
    public class WatchListViewModel
    {
        public List<UnwatchedAnime> animesByStatus { get; set;  }
        public int Total { get; set; }
        public int Planned { get; set; }
        public int Watching { get; set; }
        public int Completed { get; set; }
    }
}
