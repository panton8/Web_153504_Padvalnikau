using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Domain.Models;

namespace Web_153504_Padvalnikau.Services.ProductService;

public class ApiProductService : IProductService
{
        private HttpClient _httpClient;
        private string? _pageSize;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiProductService> _logger;
        private HttpContext? _httpContext;

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<ResponseData<Sneaker>> CreateAsync(Sneaker sneaker, IFormFile? formFile)
        {
            AddTokenToHeader();
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Sneakers");
            var response = await _httpClient.PostAsJsonAsync(uri, sneaker, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content
                    .ReadFromJsonAsync<ResponseData<Sneaker>>(_serializerOptions);
                return data; // dish;
            }
            _logger.LogError($"-----> object not created. Error:{response.StatusCode.ToString()}");
            return new ResponseData<Sneaker>
            {
                Success = false,
                ErrorMessage = $"Объект не добавлен. Error:{ response.StatusCode }"
            };
        }

        public async Task DeleteAsync(int id)
        {
            AddTokenToHeader();
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}sneakers/{id}");
            await _httpClient.DeleteAsync(urlString.ToString());
        }

        public async Task<ResponseData<Sneaker>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(new Uri($"{_httpClient.BaseAddress.AbsoluteUri}sneakers/{id}"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<Sneaker>>();
            }

            return new ResponseData<Sneaker>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }

        public async Task<ResponseData<ListModel<Sneaker>>> GetSneakerListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            AddTokenToHeader();
            
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}sneakers/");

            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            }
            if (pageNo > 1)
            {
                urlString.Append($"page{pageNo}");
            }
            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize));
            }
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content
                        .ReadFromJsonAsync<ResponseData<ListModel<Sneaker>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Sneaker>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return new ResponseData<ListModel<Sneaker>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }

        public async Task UpdateAsync(int id, Sneaker sneaker, IFormFile? formFile)
        {
            AddTokenToHeader();
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}sneakers/{id}");
            await _httpClient.PutAsJsonAsync(urlString.ToString(), sneaker, _serializerOptions);

            if (formFile != null)
            {
                await SaveImageAsync(id, formFile);
            }
        }
        
        private async Task SaveImageAsync(int id, IFormFile image)
        {
            AddTokenToHeader();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}sneakers/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;
            await _httpClient.SendAsync(request);
        }
        
        private async void AddTokenToHeader()
        {
            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }
}