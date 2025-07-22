using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.Domain.Models;
using MVC.Services.TokenProviderServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MVC.Services.LoginServices;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, ITokenProvider tokenProvider, IUserService userService)
        {
            _logger = logger;
            _tokenProvider = tokenProvider;
            _userService = userService;
        }
        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Patients", "Patient");
            }
            return View();
        }
        public async Task<IActionResult> Authenticate(LoginModel login)
        {
            try
            {
                if(!ModelState.IsValid)
                    return View("Login", login);
                var response = await _userService.Authenticate(login);
                _tokenProvider.SetToken(response);
                await HttpContextSignIn(response);
                return RedirectToAction("Patients", "Patient");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Login", new LoginModel());
            }
        }
        private async Task HttpContextSignIn(TokenModel token)
        {
            var handler = new JwtSecurityTokenHandler();

            var currentToken = handler.ReadJwtToken(token.AccessToken);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, currentToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, currentToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, currentToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
