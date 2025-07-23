using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.Domain.Models;
using MVC.Services.MedicineServices;
using MVC.Services.PatientServices;
using MVC.Services.TokenProviderServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class MedicineController : Controller
    {
        private readonly IMedicineService _medicineService;
        private readonly ITokenProvider _tokenProvider;
        List<MedicineModel> medicines = new();
        private async Task GetMedicines()
        {
            medicines = await _medicineService.GetAll<List<MedicineModel>>();
        }
        public MedicineController(IMedicineService medicineService, ITokenProvider provider)
        {
            _medicineService = medicineService;
            _tokenProvider = provider;
        }
        public async Task<IActionResult> Medicines()
        {
            TokenModel token = _tokenProvider.GetToken();
            if (token != null)
                await HttpContextSignIn(token);
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Medicines", "Medicine");
            await GetMedicines();
            return View(medicines);
        }
        public IActionResult AddMedicine()
        {
            return View(new MedicineModel());
        }
        public async Task<IActionResult> UpdateMedicine(long? medicineId)
        {
            var medicine = await _medicineService.GetById<MedicineModel>(medicineId);
            return View("AddMedicine", medicine);
        }
        public async Task<IActionResult> DeleteMedicine(long? medicineId)
        {
            try
            {
                var result = await _medicineService.Delete<long?>(medicineId);
                await GetMedicines();
                return View("Medicines", medicines);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Medicines", "Medicine");
            }
        }
        public async Task<IActionResult> AddCurrentMedicine(MedicineModel medicine)
        {
            try
            {
                var result = medicine.MedicineId == null ?
                    await _medicineService.Create<MedicineModel>(medicine) :
                    await _medicineService.Update<MedicineModel>(medicine);
                await GetMedicines();
                return View("AddMedicine", new MedicineModel());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Medicines", "Medicine");
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
