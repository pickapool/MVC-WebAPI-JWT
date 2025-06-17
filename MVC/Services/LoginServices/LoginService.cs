using MVC.Domain.Models;

namespace MVC.Services.LoginServices
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<TokenModel> Authenticate(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginModel);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(errorMessage);
            }
            return await response.Content.ReadFromJsonAsync<TokenModel>();
        }
    }
}
