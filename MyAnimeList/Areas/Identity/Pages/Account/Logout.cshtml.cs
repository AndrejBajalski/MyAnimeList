using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Enums;
using Models.DomainModels;
using Repository.Interface;

namespace MyAnimeList.Web.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;

        public LogoutModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            await _signInManager.SignOutAsync();
            TempData["Message"] = "Successfully logged out!";
            TempData["MessageType"] = MessageType.Success;
            return RedirectToAction("Index", "Animes");
        }
    }
}
