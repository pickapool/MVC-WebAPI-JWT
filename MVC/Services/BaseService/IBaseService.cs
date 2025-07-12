using MVC.Domain;

namespace MVC.Services.BaseService
{
    public interface IBaseService
    {
        Task<T> SendAsync<T>(RequestModel request);
    }
}
