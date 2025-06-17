using MVC.Domain.Models;

namespace MVC.Services.LoginServices
{
    public interface ILoginService
    {
        Task<TokenModel> Authenticate(LoginModel loginModel);
    }
}
