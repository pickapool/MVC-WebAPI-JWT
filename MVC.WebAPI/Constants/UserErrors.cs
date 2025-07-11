using MVC.Shared;

namespace MVC.WebAPI.Constants
{
    public static class UserErrors
    {
        public static Error UserExist(string id) => new ("Error.UserExist", $"User with ID {id} already exists");
        public static Error FailedToCreateUserRoles(string roleErrors) => new("Error.FailedToCreateUser", $"Failed to create user role. Errors : {roleErrors}");
        public static Error FailedToCreateUser(string errors) => new("Error.FailedToCreateUser", $"Failed to create user. Errors: {errors}");
    }
}
