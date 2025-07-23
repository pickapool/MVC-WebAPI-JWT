using MVC.Domain.Models;

namespace MVC.Services.PatientServices
{
    public interface IPatientService
    {
        Task<T> GetAll<T>();
        Task<T> GetById<T>(long? id);
        Task<T> Create<T>(PatientModel patient);
        Task<T> Update<T>(PatientModel patient);
        Task<T> Delete<T>(long? patient);
    }
}
