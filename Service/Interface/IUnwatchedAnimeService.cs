using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DomainModels;
using Models.Enums;

namespace Service.Interface
{
    public interface IUnwatchedAnimeService
    {
        public UnwatchedAnime GetById(Guid id);
        public List<UnwatchedAnime> GetAllByUser(string userId);
        public UnwatchedAnime? GetByUserAndAnimeId(string userId, Guid animeId, bool include=true);
        public Anime? GetAnimeByUserAndAnimeId(string userId, Guid animeId);
        public List<UnwatchedAnime> GetByStatus(string userId, AnimeStatus status);
        public void AddToList(string userId, Anime anime);
        public UnwatchedAnime? RemoveFromList(Guid animeId);
        public void IncreaseWatchPriority(string userId, Guid animeId);
        public UnwatchedAnime Update(UnwatchedAnime anime);
        public UnwatchedAnime UpdateStatus(Guid animeId, string userId, AnimeStatus status);

        public Dictionary<AnimeStatus, int> GetCountByStatus(string userId);

        public bool animeInList(Guid animeId, string userId);

        public List<AnimeStatus> AlreadyInWatchList(List<Anime> animes, string userId);

    }
}
