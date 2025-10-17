using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DomainModels;
using Repository.Interface;

namespace MyAnimeList.Web.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        IRepository<Anime> _animeRepository;

        public LogoutModel(IRepository<Anime> animeRepository)
        {
            _animeRepository = animeRepository;
        }

        public void OnGet()
        {
            _animeRepository.Clear();
        }
    }
}
