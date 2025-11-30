using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Service.Implementation;
using Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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

        public async Task<IActionResult> Index()
        {
            List<Anime> allAnimes = await _animeService.GetAll();
            return View(allAnimes);
        }

        [Route("Animes/{id}")]
        public IActionResult GetAnimeById(Guid id)
        {
            Anime? anime = _animeService.GetById(id);
            if(anime == null)
            {
                return RedirectToAction(nameof(Index));
            }
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
            else
            {
                Guid userId = Guid.Parse(userIdStr);
                _unwatchedAnimeService.AddToList(userId, anime);
                TempData["Message"] = $"Added {anime.Name} to watch list";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
