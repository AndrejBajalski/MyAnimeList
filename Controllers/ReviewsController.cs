using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.DomainModels;
using Service.Interface;
using Newtonsoft.Json;

namespace MyAnimeList.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IAnimeService _animeService;
        private readonly UserManager<User> _userManager;

        public ReviewsController(IReviewService reviewService, IAnimeService animeService, UserManager<User> userManager)
        {
            _reviewService = reviewService;
            _animeService = animeService;
            _userManager = userManager;
        }

        public IActionResult Index(Guid animeId)
        {
            var anime = _animeService.GetById(animeId); 
            return View(anime);  
        }
        public IActionResult GetAllReviewsForAnime(Guid animeId)
        {
            var allReviews = _reviewService.GetAllReviewsByAnimeId(animeId);
            return View(allReviews);
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddReview(Guid animeId, string comment, int rating)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(User));
            var review = _reviewService.AddReview(animeId, userId, rating, comment);
            TempData["MessageType"] = MessageType.Success;
            TempData["Message"] = "Successfully added review!";
            return RedirectToAction("Index", new { animeId });
        }
        public IActionResult UpdateReview(Guid animeId)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(User));
            var review = _reviewService.GetReview(animeId, userId);
            TempData["Content"] = review.Content;
            TempData["Rating"] = review.Rating;
            return RedirectToAction("Index", new { review.AnimeId });
        }
        [HttpPost]
        [Authorize]
        public IActionResult UpdateReviewPost(Guid animeId, string comment, int rating)
        {
            Guid userId = Guid.Parse(_userManager.GetUserId(User));
            var review = _reviewService.UpdateReview(animeId, userId, rating, comment);
            TempData["MessageType"] = MessageType.Success;
            TempData["Message"] = "Successfully updated review!";
            TempData["Content"] = review.Content;
            TempData["Rating"] = review.Rating;
            return RedirectToAction("Index", new { animeId });
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddReplyToReview(Review review, string reply)
        {
            _reviewService.AddCommentToReview(review.Id, reply);
            TempData["MessageType"] = MessageType.Success;
            TempData["Message"] = "Successfully posted a comment to this review";
            return RedirectToAction(nameof(GetAllReviewsForAnime), review.AnimeId);
        }
    }
}
