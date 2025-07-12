using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Web.Helpers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC.Domain.Models;
using MVC.Models;
using MVC.Services.LoginServices;
using MVC.Services.TokenProviderServices;

namespace MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly ITokenProvider _tokenProvider;

    public HomeController(ILogger<HomeController> logger, IUserService userService, ITokenProvider tokenProvider)
    {
        _logger = logger;
        _userService = userService;
        _tokenProvider = tokenProvider;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Signup()
    {
        return View("Signup");
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel login)
    {
        try
        {
            var response = await _userService.Authenticate(login);
            _tokenProvider.SetToken(response);
            await HttpContextSignIn(response);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            return View("Index");
        }
    }
    private async Task HttpContextSignIn(TokenModel token)
    {
        var handler = new JwtSecurityTokenHandler();
            
        var currentToken = handler.ReadJwtToken(token.AccessToken);
        
        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, currentToken.Claims.FirstOrDefault( claim => claim.Type == JwtRegisteredClaimNames.Email).Value));
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName, currentToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.UniqueName).Value));
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, currentToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value));

        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _tokenProvider.ClearToken();
        return RedirectToAction("Index", "Home");
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
