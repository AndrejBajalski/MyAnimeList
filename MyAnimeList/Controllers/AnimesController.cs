using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Service.Implementation;
using Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Models.DTO;
using Models.Enums;
using System.Security.Claims;

namespace MyAnimeList.Web.Controllers
{
    public class AnimesController : Controller
    {
        private readonly DataFetchService _dataFetchService;
        private readonly IAnimeService _animeService;
        private readonly IUnwatchedAnimeService _unwatchedAnimeService;
        private readonly UserManager<User> _userManager;

        public AnimesController(DataFetchService dataFetchService, IAnimeService animeService, IUnwatchedAnimeService unwatchedAnimeService, UserManager<User> userManager)
        {
            _dataFetchService = dataFetchService;
            _animeService = animeService;
            _unwatchedAnimeService = unwatchedAnimeService;
            _userManager = userManager;
        }

        [Route("/")]
        [Route("/Animes")]
        public async Task<IActionResult> Index(string? keyword, int page=1)
        {
            PaginatedResponse response = await _animeService.GetAll(keyword, page);
            if (!keyword.IsNullOrEmpty())
                ViewBag.Keyword = keyword;
            List<Anime> allAnimes = response.Animes;
            if (User.Identity.IsAuthenticated)
            {
                List<AnimeStatus> statuses = _unwatchedAnimeService.AlreadyInWatchList(allAnimes, _userManager.GetUserId(User));
                ViewBag.Statuses = statuses;
                ViewBag.IsAuth = true;
            }
            else
                ViewBag.IsAuth = false;
            ViewBag.Page = response.Page;
            return View(allAnimes);
        }

        public IActionResult GetAnimeById(Guid id, AnimeStatus status)
        {
            Anime? anime = _animeService.GetById(id);
            if(anime == null)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Status = status;
            return View("AnimeDetails", anime);
        }

        [Authorize]
        public IActionResult AddToList(Guid id)
        {
            Anime? anime = _animeService.GetById(id);
            string? userIdStr = _userManager.GetUserId(User);
            if(anime == null)
            {
                TempData["Error"] = "Anime not found.";
                return RedirectToAction(nameof(Index));
            }
            var unwatched = _unwatchedAnimeService.GetByUserAndAnimeId(userIdStr, id);
            if(unwatched != null)
            {
                TempData["MessageType"] = MessageType.Info;
                TempData["Message"] = "This anime is already added to your watch list";
                return RedirectToAction(nameof(Index));
            }
                _unwatchedAnimeService.AddToList(userIdStr, anime);
                TempData["MessageType"] = MessageType.Success;
                TempData["Message"] = $"Added {anime.Name} to watch list";
                return RedirectToAction(nameof(Index));
        }
    }
}
