using MediatR;
using MVC.Domain.Models;
using MVC.Shared;

namespace MVC.WebAPI.Commands.UserCommands.LoginCommand
{
    public record class UserLoginCommand(LoginModel? request) : IRequest<Result<TokenModel>>;
}
