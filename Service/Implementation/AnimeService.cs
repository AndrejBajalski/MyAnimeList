using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DomainModels;
using Service.Interface;
using Repository.Interface;
using Models.DTO;

namespace Service.Implementation
{
    public class AnimeService : IAnimeService
    {
        private readonly IRepository<Anime> _repository;
        private readonly DataFetchService _dataFetchService;
        public Pagination CurrentPage { get; set; }
        List<Anime> animesByPage = new List<Anime>();

        public AnimeService(IRepository<Anime> repository, DataFetchService dataFetchService)
        {
            _repository = repository;
            _dataFetchService = dataFetchService;
        }

        public Anime Create(Anime anime)
        {
            var fromRepository = GetById(anime.Id);
            if (fromRepository == null) 
                return _repository.Insert(anime);
            return fromRepository;
        }

        public Anime? DeleteById(Guid id)
        {
            Anime anime = GetById(id);
            return _repository.Delete(anime);
        }

        public async Task<PaginatedResponse> GetAll(string? keyword, int page=1)
        {
            int lastInPage = page * 25;
            var all = _repository.GetAll(selector: a => a).ToList();
            if (keyword != null)
            {
                return await _dataFetchService.fetchByKeywordSearch(keyword, page);
            }
            if(all.Count < lastInPage)
            {
                return await _dataFetchService.fetchByPage(page);
            }
            return new PaginatedResponse
            {
                Page = new Pagination { current_page = page, has_next_page = lastInPage <= 29425, last_visible_page = 1178 },
                Animes = all.GetRange((page - 1) * 25, 25)
            };
        }

        public Anime? GetById(Guid id)
        {
            return _repository.Get(selector: a=>a, predicate: a => a.Id == id);
        }

        public void SetCurrentPage(Pagination? p)
        {
            CurrentPage = p;
        }
        public Pagination? GetCurrentPage()
        {
            return CurrentPage;
        }
    }
}
