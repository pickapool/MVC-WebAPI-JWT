using MVC.Domain.Models;

namespace MVC.Services.LoginServices
{
    public interface IUserService
    {
        Task<TokenModel> Authenticate(LoginModel loginModel);
    }
}
