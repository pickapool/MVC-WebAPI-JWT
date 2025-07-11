using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.WebAPI.Commands.UserCommands.CreateCommand;
using MVC.WebAPI.Services.TokenServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVC.WebAPI.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;
        private readonly ISender _sender;
        public AuthController(UserManager<ApplicationUserModel> userManager,
                              RoleManager<IdentityRole> roleManager, 
                              ILogger<AuthController> logger,
                              ITokenService tokenService,
                              AppDbContext appDbContext,
                              ISender sender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _tokenService = tokenService;
            _context = appDbContext;
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] CreateAccountCommand createAccountCommand)
        {
            var result = await _sender.Send(createAccountCommand);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    return BadRequest("Invalid username or password");
                }
                bool isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if (isValidPassword == false)
                {
                    return Unauthorized();
                }

                // creating the necessary claims
                List<Claim> authClaims = [
                        new (ClaimTypes.Name, user.UserName),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                // unique id for token
        ];

                var userRoles = await _userManager.GetRolesAsync(user);

                // adding roles to the claims. So that we can get the user role from the token.
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                // generating access token
                var token = _tokenService.GenerateAccessToken(authClaims);

                string refreshToken = _tokenService.GenerateRefreshToken();

                //save refreshToken with exp date in the database
                var tokenInfo = _context.TokenInfos.
                            FirstOrDefault(a => a.Username == user.UserName);

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

                return Ok(new TokenModel
                {
                    AccessToken = token,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
            }

        }
        [Authorize]
        [HttpPost("token/refresh")]
        public async Task<IActionResult> Refresh(TokenModel tokenModel)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(tokenModel.AccessToken);
                var username = principal.Identity.Name;

                var tokenInfo = _context.TokenInfos.SingleOrDefault(u => u.Username == username);
                if (tokenInfo == null
                    || tokenInfo.RefreshToken != tokenModel.RefreshToken
                    || tokenInfo.ExpiredAt <= DateTime.UtcNow)
                {
                    return BadRequest("Invalid refresh token. Please login again.");
                }

                var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                tokenInfo.RefreshToken = newRefreshToken; // rotating the refresh token
                await _context.SaveChangesAsync();

                return Ok(new TokenModel
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("token/revoke")]
        [Authorize]
        public async Task<IActionResult> Revoke()
        {
            try
            {
                var username = User.Identity.Name;

                var user = _context.TokenInfos.SingleOrDefault(u => u.Username == username);
                if (user == null)
                {
                    return BadRequest();
                }

                user.RefreshToken = string.Empty;
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
