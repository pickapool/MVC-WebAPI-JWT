using MVC.Domain;
using MVC.Shared;
using System.Text;
using System.Text.Json;

namespace MVC.Services.BaseService
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<T> SendAsync<T>(RequestModel request)
        
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("WebAPI");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");

            message.RequestUri = new Uri(request.RequestUrl);

            if(request.Data != null)
            {
                message.Content = new StringContent(JsonSerializer.Serialize(request.Data), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = null;

            switch(request.RequestType)
            {
                case Enums.RequestType.GET:
                    message.Method = HttpMethod.Get;
                    break;
                case Enums.RequestType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case Enums.RequestType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case Enums.RequestType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    throw new InvalidOperationException("Unsupported request type.");
            }
            response = await httpClient.SendAsync(message);

            if(!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
