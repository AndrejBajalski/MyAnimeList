using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.DomainModels;
using Models.Enums;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review> _reviewRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IUnwatchedAnimeService _unwatchedAnimeService;

        public ReviewService(IRepository<Review> reviewRepository, IRepository<Comment> commentRepository, IUnwatchedAnimeService unwatchedAnimeService)
        {
            _reviewRepository = reviewRepository;
            _commentRepository = commentRepository;
            _unwatchedAnimeService = unwatchedAnimeService;
        }

        public void AddComment(Guid reviewId, string comment)
        {
            Comment newComment = new Comment(reviewId, comment);
            _commentRepository.Insert(newComment);
        }

        public Review? AddReview(Guid animeId, string userId, int rating, string? content)
        {
            var review = GetReview(animeId, userId);
            if (review != null)
                return review;
            review = new Review
            {
                AnimeId = animeId,
                UserId = userId,
                Content = content,
                Rating = rating,
                LastModified = DateTime.Now
            };
            review = _reviewRepository.Insert(review);
            if (!content.IsNullOrEmpty())
            {
                var comment = new Comment(review.Id, content);
                _commentRepository.Insert(comment);
            }
            _unwatchedAnimeService.UpdateStatus(animeId, userId, AnimeStatus.ReviewAdded);
            return review;
        }

        public Comment AddCommentToReview(Guid reviewId, string text)
        {
            Comment comment = new Comment(reviewId, text);
            return _commentRepository.Insert(comment);
        }

        public Review? UpdateReview(Guid animeId, string userId, int rating, string? content)
        {
            var review = GetReview(animeId, userId);
            review.Content = content;
            review.Rating = rating;
            review.LastModified = DateTime.Now;
            return _reviewRepository.Update(review);
        }
        public List<Review>? GetAllReviewsByAnimeId(Guid animeId)
        {
            return _reviewRepository
                .GetAll(selector: r => r, predicate: r => r.AnimeId == animeId,
                        include: q => q.Include(r => r.Comments).Include(r => r.User)
                        ).ToList();
        }

        public List<Review>? GetAllReviewsByUserId(string userId)
        {
            return _reviewRepository
               .GetAll(selector: r => r, predicate: r => r.UserId == userId).ToList();
        }

        public Review? GetById(Guid reviewId)
        {
            return _reviewRepository.Get(selector: r => r, predicate: r => r.Id ==  reviewId);
        }

        public Review? GetReview(Guid animeId, string userId)
        {
            return _reviewRepository
               .Get(selector: r => r,
                       predicate: r => r.UserId == userId && r.AnimeId == animeId);
        }

        public Review? RemoveReview(Review review)
        {
            return _reviewRepository.Delete(review);
        }
    }
}
