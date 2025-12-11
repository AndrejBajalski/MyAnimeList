using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum AnimeStatus
    {
        Watching,
        Completed,
        Paused,
        PlanToWatch,
        NotInList,
        ReviewAdded,
        All
    }

    public class StatusReader
    {
        public static string StatusToString(AnimeStatus status)
        {
            switch (status)
            {
                case AnimeStatus.Watching:
                    return "Watching";
                case AnimeStatus.Completed:
                    return "Completed";
                case AnimeStatus.Paused:
                    return "Paused";
                case AnimeStatus.PlanToWatch:
                    return "Plan to watch";
                case AnimeStatus.NotInList:
                    return "Not in my Watch List";
                case AnimeStatus.ReviewAdded:
                    return "Review posted";
                default:
                    return "All";
            }
        }
    }
}
