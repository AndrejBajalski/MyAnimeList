using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DomainModels;
using Models.DTO;

namespace Service.Interface
{
    public interface IAnimeService
    {
        public Anime? GetById(Guid id);
        public Task<PaginatedResponse> GetAll(string? keyword, int page = 1);
        public Anime Create(Anime anime);
        public Anime? DeleteById(Guid id);
        public void SetCurrentPage(Pagination? p);
        public Pagination? GetCurrentPage();
    }
}
