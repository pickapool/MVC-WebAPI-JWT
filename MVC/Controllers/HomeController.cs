using System.Diagnostics;
using System.Text.Json;
using System.Web.Helpers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;
using MVC.Domain.Models;
using MVC.Models;
using MVC.Services.LoginServices;

namespace MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILoginService _loginService;

    public HomeController(ILogger<HomeController> logger, ILoginService loginService)
    {
        _logger = logger;
        _loginService = loginService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel login)
    {
        try
        {
            var response = await _loginService.Authenticate(login);
            ViewBag.Token = JsonSerializer.Serialize(response);
            return View("Index");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            return View("Index", login);
        }
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
