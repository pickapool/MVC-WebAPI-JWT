using MediatR;
using Microsoft.AspNetCore.Identity;
using MVC.Domain.Models;
using MVC.Shared;
using MVC.WebAPI.Constants;
using MVC.WebAPI.Controllers;

namespace MVC.WebAPI.Commands.UserCommands.CreateCommand
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result>
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthController> _logger;
        public CreateAccountCommandHandler(UserManager<ApplicationUserModel> userManager, RoleManager<IdentityRole> roleManager, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<Result> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userManager.FindByNameAsync(request.SignupModel.Email);
                if (existingUser != null)
                {
                    return UserErrors.UserExist(existingUser.Id);
                }
                if (await _roleManager.RoleExistsAsync(Roles.User) == false)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(Roles.User));

                    if (roleResult.Succeeded == false)
                    {
                        var roleErros = roleResult.Errors.Select(e => e.Description);
                        _logger.LogError($"Failed to create user role. Errors : {string.Join(",", roleErros)}");
                        return UserErrors.FailedToCreateUser(string.Join(",", roleErros));
                    }
                }

                ApplicationUserModel user = new()
                {
                    Email = request.SignupModel.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = request.SignupModel.Email,
                    Name = request.SignupModel.Name,
                    EmailConfirmed = true
                };

                var createUserResult = await _userManager.CreateAsync(user, request.SignupModel.Password);
                if (createUserResult.Succeeded == false)
                {
                    var errors = createUserResult.Errors.Select(e => e.Description);
                    _logger.LogError($"Failed to create user. Errors: {string.Join(", ", errors)}");
                    return UserErrors.FailedToCreateUser(string.Join(", ", errors));
                }

                var addUserToRoleResult = await _userManager.AddToRoleAsync(user: user, role: Roles.User);

                if (addUserToRoleResult.Succeeded == false)
                {
                    var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                    _logger.LogError($"Failed to add role to the user. Errors : {string.Join(",", errors)}");
                }
                return Result.Success();
            }
            catch (Exception ex)
            {
                return StatusCodeErrors.StatusCode(StatusCodes.Status500InternalServerError.ToString(), ex.Message);
            }
        }
    }
}
