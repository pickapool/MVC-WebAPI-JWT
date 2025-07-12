using MVC.Domain.Models;

namespace MVC.Services.TokenProviderServices
{
    public interface ITokenProvider
    {
        void SetToken(TokenModel token);
        TokenModel GetToken();
        void ClearToken();
    }
}
