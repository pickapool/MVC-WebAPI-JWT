using MediatR;
using MVC.Domain.Models;
using MVC.Shared;

namespace MVC.WebAPI.Commands.UserCommands.CreateCommand
{
    public record class CreateAccountCommand(SignupModel? request) : IRequest<Result>;
}
