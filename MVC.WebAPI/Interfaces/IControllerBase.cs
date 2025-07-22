using Microsoft.AspNetCore.Mvc;
using MVC.WebAPI.Commands.PatientCommands.AddPatient;

namespace MVC.WebAPI.Interfaces
{
    public interface IControllerBase
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> Add<T>(T t);
        Task<IActionResult> Update<T>(T t);
        Task<IActionResult> Delete<T>(T t);
        Task<IActionResult> GetById<T>(T t);
    }
}
