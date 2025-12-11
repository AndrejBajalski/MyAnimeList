using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DomainModels;

namespace Service.Interface
{
    public interface IReviewService
    {
        Review? AddReview(Guid animeId, string userId, int rating, string? content);
        Review? UpdateReview(Guid animeId, string userId, int rating, string? content);
        Review? RemoveReview(Review review);
        Review? GetById(Guid reviewId); 
        Review? GetReview(Guid animeId, string userId);
        List<Review>? GetAllReviewsByAnimeId(Guid animeId);
        List<Review>? GetAllReviewsByUserId(string userId);
        Comment AddCommentToReview(Guid reviewId, string comment);
    }
}
