using MVC.Shared;

namespace MVC.WebAPI.Constants
{
    public class StatusCodeErrors
    {
        public static Error StatusCode(string code, string description) => new(code, description);
    }
}
