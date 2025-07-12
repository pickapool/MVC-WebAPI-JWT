using MVC.Domain.Models;
using System.Text.Json;

namespace MVC.Services.TokenProviderServices
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("token");
        }

        public TokenModel GetToken()
        {
            string? token = null;
            bool hasToken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue("token", out token) ?? false;
            return hasToken == true ? JsonSerializer.Deserialize<TokenModel>(token) : null;
        }

        public void SetToken(TokenModel token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("token", JsonSerializer.Serialize(token));
        }
    }
}
