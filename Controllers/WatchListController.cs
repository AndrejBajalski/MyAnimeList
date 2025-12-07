using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DomainModels;
using Models.Enums;
using Service.Interface;

namespace MyAnimeList.Web.Controllers
{
    public class WatchListController : Controller
    {
        private readonly IUnwatchedAnimeService _unwatchedAnimeService;
        private readonly UserManager<User> _userManager;
        private List<UnwatchedAnime> myList;

        public WatchListController(IUnwatchedAnimeService unwatchedAnimeService, UserManager<User> userManager)
        {
            _unwatchedAnimeService = unwatchedAnimeService;
            _userManager = userManager;
            myList = new List<UnwatchedAnime>();
        }

        [Authorize]
        public IActionResult Index(AnimeStatus? status)
        {
            string? id = _userManager.GetUserId(User);
            myList = status == null ?
                _unwatchedAnimeService.GetAllByUser(Guid.Parse(id)) :
                _unwatchedAnimeService.GetByStatus(Guid.Parse(id), status.Value);
            var statusCountDictionary = _unwatchedAnimeService.GetCountByStatus();
            ViewBag.StatusCount = new Dictionary<AnimeStatus, int>();
            ViewBag.Status = status ?? AnimeStatus.All;
            foreach (AnimeStatus s in Enum.GetValues(typeof(AnimeStatus))) {
                if (statusCountDictionary.ContainsKey(s))
                    ViewBag.StatusCount[s] = statusCountDictionary[s];
                else
                    ViewBag.StatusCount[s] = 0;
            }
            return View(myList);
        }
        public IActionResult RemoveFromList(Guid id)
        {
            var removed = _unwatchedAnimeService.RemoveFromList(id);
            TempData["Message"] = $"Successfully removed {removed?.Anime?.Name ?? "anime"} from Watch List!";
            TempData["MessageType"] = MessageType.Success;
            return RedirectToAction(nameof(Index));
        }
        public IActionResult UpdateStatus(Guid id, AnimeStatus status)
        {
            var unWatched = _unwatchedAnimeService.GetById(id);
            unWatched.Status = status;
            _unwatchedAnimeService.Update(unWatched);
            TempData["Message"] = $"Changed status of {unWatched?.Anime?.Name ?? "anime"} to {StatusReader.StatusToString(status)}!";
            TempData["MessageType"] = MessageType.Info;
            return RedirectToAction(nameof(Index));
        }
    }
}
