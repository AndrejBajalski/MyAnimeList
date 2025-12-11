using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Models.DomainModels;
using Models.Enums;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation
{
    public class UnwatchedAnimeService : IUnwatchedAnimeService
    {
        private readonly IRepository<UnwatchedAnime> _unwatchedAnimeRepository;

        public UnwatchedAnimeService(IRepository<UnwatchedAnime> unwatchedAnimeRepository)
        {
            _unwatchedAnimeRepository = unwatchedAnimeRepository;
        }

        public UnwatchedAnime GetById(Guid id)
        {
            return _unwatchedAnimeRepository
                        .Get(selector: u => u, predicate: u => u.Id == id, include: q => q.Include(u => u.Anime));
        }
        public UnwatchedAnime Update(UnwatchedAnime anime)
        {
            return _unwatchedAnimeRepository.Update(anime);
        }
        public UnwatchedAnime UpdateStatus(Guid animeId, string userId, AnimeStatus status)
        {
            var unwatched = GetByUserAndAnimeId(userId, animeId);
            unwatched.Status = status;
            return Update(unwatched);
        }

        public void AddToList(string userId, Anime anime)
        {
            var fetched = GetAnimeByUserAndAnimeId(userId, anime.Id);
            if (fetched != null) 
                return;
            UnwatchedAnime unwatchedAnime = new UnwatchedAnime
            {
                UserId = userId,
                AnimeId = anime.Id,
                Status = AnimeStatus.PlanToWatch //Default status
            };
            _unwatchedAnimeRepository.Insert(unwatchedAnime);
        }

        public List<UnwatchedAnime> GetAllByUser(string userId)
        {
            return _unwatchedAnimeRepository
                .GetAll(selector: u => u,
                        predicate: u => u.UserId == userId,
                        include: q => q.Include(u => u.Anime)
                        )
                .ToList();
        }

        public List<UnwatchedAnime> GetByStatus(string userId, AnimeStatus status)
        {
            return _unwatchedAnimeRepository
                .GetAll(selector: u => u,
                        predicate: u => u.UserId == userId && u.Status == status,
                        include: q => q.Include(u => u.Anime)
                        )
                .ToList();
        }

        public UnwatchedAnime? GetByUserAndAnimeId(string userId, Guid animeId, bool include=true)
        {
            if (include)
                return _unwatchedAnimeRepository
                    .Get(selector: u => u, predicate: u => u.UserId == userId && u.AnimeId == animeId, include: q => q.Include(u => u.Anime));
            return _unwatchedAnimeRepository
                .Get(selector: u => u, predicate: u => u.UserId == userId && u.AnimeId == animeId);
        }

        public Anime? GetAnimeByUserAndAnimeId(string userId, Guid animeId)
        {
            return _unwatchedAnimeRepository
                .Get(selector: u => u.Anime, predicate: u => u.UserId == userId && u.AnimeId == animeId, include: q => q.Include(u => u.Anime));
        }

        public void IncreaseWatchPriority(string userId, Guid animeId)
        {
            throw new NotImplementedException();
        }

        public UnwatchedAnime? RemoveFromList(Guid animeId)
        {
            var anime = _unwatchedAnimeRepository
                .Get(selector: u => u, predicate: u => u.Id == animeId, include: q => q.Include(u => u.Anime));
            _unwatchedAnimeRepository.Delete(anime);
            return anime;
        }

        public Dictionary<AnimeStatus, int> GetCountByStatus(string userId)
        {
            var dict = _unwatchedAnimeRepository.GetContext()
                .Where(u => u.UserId == userId)
                .GroupBy(u => u.Status)
                .ToDictionary(g => g.Key, g => g.Count()) ?? new Dictionary<AnimeStatus, int>(); ;
            int total = 0;
            foreach (var entry in dict)
            {
                total += entry.Value;
            }
            dict[AnimeStatus.All] = total;
            return dict;
        }

        public bool animeInList(Guid animeId, string userId)
        {
            var unwatched = GetByUserAndAnimeId(userId, animeId, include: false);
            return unwatched != null;
        }

        public List<AnimeStatus> AlreadyInWatchList(List<Anime> animes, string userId)
        {
            List<AnimeStatus> statuses = new List<AnimeStatus>();
            for (int i = 0; i < animes.Count; i++)
            {
                var inList = GetByUserAndAnimeId(userId, animes[i].Id, include: false);
                if (inList == null)
                    statuses.Add(AnimeStatus.NotInList);
                else
                    statuses.Add(inList.Status);
            }
            return statuses;
        }
    }
}
