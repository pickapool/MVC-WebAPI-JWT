using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.Domain.Models;
using MVC.Services.PatientServices;
using MVC.Services.TokenProviderServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly ITokenProvider _tokenProvider;
        List<PatientModel> _patients = new();
        private async Task GetPatients()
        {
            _patients = await _patientService.GetAll<List<PatientModel>>();
        }
        public PatientController(IPatientService patientService, ITokenProvider provider)
        {
            _patientService = patientService;
            _tokenProvider = provider;
        }
        public async Task<IActionResult> Patients()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "User");
            await GetPatients();
            return View(_patients);
        }
        public IActionResult AddPatient()
        {
            return View(new PatientModel());
        }
        public async Task<IActionResult> UpdatePatient(long? patientId)
        {
            var patient = await _patientService.GetById<PatientModel>(patientId);
            return View("AddPatient", patient);
        }
        public async Task<IActionResult> DeletePatient(long? patientId)
        {
            try
            {
                var result = await _patientService.Delete<long?>(patientId);
                await GetPatients();
                return View("Patients", _patients);
            }
            catch (Exception ex) {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Patients", "Patient");
            }
        }
        public async Task<IActionResult> AddCurrentPatient(PatientModel patient)
        {
            try
            {
                var result = patient.PatientId == null ? 
                    await _patientService.Create<PatientModel>(patient) :
                    await _patientService.Update<PatientModel>(patient);
                await GetPatients();
                return View("AddPatient", new PatientModel());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Patients", "Patient");
            }
        }
    }
}
