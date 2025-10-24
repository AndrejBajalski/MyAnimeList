using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Service.Implementation;
using Models.DomainModels;
using Microsoft.AspNetCore.Identity;

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

        public IActionResult AddToList(Guid id)
        {
            Anime? anime = _animeService.GetById(id);
            string? userIdStr = _userManager.GetUserId(User);
            if (anime!=null && userIdStr!=null)
            {
                Guid userId = Guid.Parse(userIdStr);
                _unwatchedAnimeService.AddToList(userId, anime);
            }
            else
            {
                ViewBag.Message = "Anime not found.";
            }
            return RedirectToAction("Index");
        }
    }
}
