using System.Diagnostics;
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
    public async Task<IActionResult> Login(LoginModel login)
    {
        try
        {
            var response = await _loginService.Authenticate(login);
            return View("Index");
        }
        catch (Exception ex) {
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
