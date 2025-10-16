using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Service.Implementation;
using Models.DomainModels;

namespace MyAnimeList.Web.Controllers
{
    public class AnimesController : Controller
    {
        private readonly DataFetchService _dataFetchService;
        private readonly IAnimeService _animeService;

        public AnimesController(DataFetchService dataFetchService, IAnimeService animeService)
        {
            _dataFetchService = dataFetchService;
            _animeService = animeService;
        }

        public async Task<IActionResult> Index()
        {
            List<Anime> allAnimes = await _animeService.GetAll();
            return View(allAnimes);
        }
    }
}
