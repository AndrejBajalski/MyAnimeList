using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Models.DomainModels;
using Models.DTO;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation
{
    public class DataFetchService
    {
        private const string BASE_URL = "https://api.jikan.moe/v4/anime";
        private readonly IRepository<Anime> _animeRepository;

        public DataFetchService(IRepository<Anime> repository)
        {
            _animeRepository = repository;
        }

        public async Task<PaginatedResponse> fetchAll(string Uri=BASE_URL)
        {
            List<AnimeDTO> animeDTOs = new List<AnimeDTO>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(Uri);
            var response = await client.GetFromJsonAsync<AllAnimesResponseDTO>("");
            Pagination pagination = response.pagination;
            List<AnimeDTO>? allAnimeDTOs = response.data;
            List<Anime> allAnime = new List<Anime>();
            foreach (var animeDto in allAnimeDTOs)
            {
                Anime anime = new()
                {
                    Id = IntToGuid(animeDto.mal_id),
                    Name = animeDto.title_english ?? animeDto.title_japanese,
                    NameJapanese = animeDto.title_japanese,
                    Description = animeDto.synopsis,
                    YearAired = animeDto.year,
                    Rating = animeDto.score,
                    NumberOfEpisodes = animeDto.episodes ?? 0,
                    ImageUrl = animeDto.images?.jpg?.image_url,
                    ImageUrlSmall = animeDto.images?.jpg?.small_image_url,
                    Genres = animeDto.genres?.Select(genre => genre.name).ToList() ?? new List<string>()
                };
                allAnime.Add(anime);
                _animeRepository.InsertIfAbsent(anime);
            }
            return new PaginatedResponse
            {
                Page = pagination,
                Animes = allAnime
            };
        }

        public async Task<PaginatedResponse> fetchByPage(int page)
        {
            string uri = BASE_URL + $"?page={page}";
            return await fetchAll(uri);
        }

        public async Task<PaginatedResponse> fetchByKeywordSearch(string keyword, int page)
        {
            string uri = BASE_URL + $"?page={page}&q={keyword}";
            return await fetchAll(uri);
        }

        private Guid IntToGuid(int id)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(id).CopyTo(bytes, 0);
            Guid guid = new Guid(bytes);
            return guid;
        }
    }
}
