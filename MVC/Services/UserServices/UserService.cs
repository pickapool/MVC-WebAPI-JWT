using MVC.Domain;
using MVC.Domain.Models;
using MVC.Services.BaseService;
using MVC.Shared;
using MVC.Shared.Extensions;

namespace MVC.Services.LoginServices
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseService _baseService;
        private readonly string defaultRequestUrl;
        private RequestModel request = new();
        public UserService(IConfiguration configuration, IBaseService baseService)
        {
            _configuration = configuration;
            _baseService = baseService;
            defaultRequestUrl = $"{_configuration["BaseAPIUrls:UserAPI"]}/api/auth";
        }
        public async Task<TokenModel> Authenticate(LoginModel model)
        { 
            request.RequestUrl = $"{defaultRequestUrl}/login";
            request.RequestType = Enums.RequestType.POST;
            request.Data = model.WrapModel("request"); 

            var response = await _baseService.SendAsync<TokenModel>(request);

            return response;
        }
    }
}
