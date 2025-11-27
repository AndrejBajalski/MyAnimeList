using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DomainModels;
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

        public IActionResult Index()
        {
            string? id = _userManager.GetUserId(User);
            myList = _unwatchedAnimeService.GetAllByUser(Guid.Parse(id));
            return View(myList);
        }
        public IActionResult RemoveFromList(Guid id)
        {
            var removed = _unwatchedAnimeService.RemoveFromList(id);
            string? userId = _userManager.GetUserId(User);
            myList = _unwatchedAnimeService.GetAllByUser(Guid.Parse(userId));
            String message = $"Successfully removed {removed.Anime?.Name} from Watch List!";
            ViewBag.Message = message;
            return View(nameof(Index), myList);
        }
    }
}
