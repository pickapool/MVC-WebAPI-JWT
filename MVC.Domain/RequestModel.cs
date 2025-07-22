using MVC.Shared;

namespace MVC.Domain
{
    public class RequestModel
    {
        public Enums.RequestType RequestType { get; set; } = Enums.RequestType.GET;
        public string? RequestUrl { get; set; }
        public string? AccessToken { get; set; }
        public object? Data { get; set; }
    }
}
