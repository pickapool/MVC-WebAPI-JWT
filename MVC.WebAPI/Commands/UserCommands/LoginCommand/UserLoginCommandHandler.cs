using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.WebAPI.Constants;
using MVC.WebAPI.Services.TokenServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVC.WebAPI.Commands.UserCommands.LoginCommand
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, Result<TokenModel>>
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly ILogger<UserLoginCommandHandler> _logger;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;
        public UserLoginCommandHandler(UserManager<ApplicationUserModel> userManager,
                                       ILogger<UserLoginCommandHandler> logger,
                                       ITokenService tokenService,
                                       AppDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _tokenService = tokenService;
            _context = context;
        }
        public async Task<Result<TokenModel>> Handle(UserLoginCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(command.request.Username);
                if (user == null)
                {
                    return Result.Failure<TokenModel>(UserErrors.Unauthorized());
                }
                bool isValidPassword = await _userManager.CheckPasswordAsync(user, command.request.Password);
                if (isValidPassword == false)
                {
                    return Result.Failure<TokenModel>(UserErrors.Unauthorized());
                }

                // creating the necessary claims
                List<Claim> authClaims = [ new (ClaimTypes.Name, user.UserName), new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),];

                var userRoles = await _userManager.GetRolesAsync(user);

                // adding roles to the claims. So that we can get the user role from the token.
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                //generate access token
                var token = _tokenService.GenerateAccessToken(authClaims);
                //generate refresh token
                string refreshToken = _tokenService.GenerateRefreshToken();

                //save refreshToken with exp date in the database
                var tokenInfo = _context.TokenInfos.FirstOrDefault(a => a.Username == user.UserName);

                // If tokenInfo is null for the user, create a new one
                if (tokenInfo == null)
                {
                    var ti = new TokenInfoModel
                    {
                        Username = user.UserName,
                        RefreshToken = refreshToken,
                        ExpiredAt = DateTime.UtcNow.AddDays(7)
                    };
                    _context.TokenInfos.Add(ti);
                }
                // Else, update the refresh token and expiration
                else
                {
                    tokenInfo.RefreshToken = refreshToken;
                    tokenInfo.ExpiredAt = DateTime.UtcNow.AddDays(7);
                }

                await _context.SaveChangesAsync();

                var tokens = new TokenModel();
                tokens.AccessToken = token;
                tokens.RefreshToken = refreshToken;

                return Result.Success(tokens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Failure<TokenModel>(UserErrors.Unauthorized());
            }
        }
    }
}
