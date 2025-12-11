using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DomainModels;
using Repository.Interface;

namespace MyAnimeList.Web.Areas.Misc.Pages
{
    public class ClearModel : PageModel
    {
        IRepository<Anime> _animeRepository;

        public ClearModel(IRepository<Anime> animeRepository)
        {
            _animeRepository = animeRepository;
        }

        public void OnGet()
        {
            _animeRepository.Clear();
        }
    }
}
