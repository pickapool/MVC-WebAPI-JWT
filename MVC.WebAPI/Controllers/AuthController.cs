using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.WebAPI.Commands.UserCommands.CreateCommand;
using MVC.WebAPI.Commands.UserCommands.LoginCommand;
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
        public async Task<IActionResult> Login([FromBody] UserLoginCommand loginCommand)
        {
            Result result = await _sender.Send(loginCommand);
            if (result.IsSuccess)
                return Ok(result);
            if (result.Error.Code == StatusCodes.Status400BadRequest)
                return BadRequest(result.Error.Description);
            if (result.Error.Code == StatusCodes.Status401Unauthorized)
                return Unauthorized(result.Error.Description);
            return StatusCode(500, result.Error.Description);

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
