using MVC.Domain.Models;

namespace MVC.Services.MedicineServices
{
    public interface IMedicineService
    {
        Task<T> GetAll<T>();
        Task<T> GetById<T>(long? medicineId);
        Task<T> Create<T>(MedicineModel medicine);
        Task<T> Update<T>(MedicineModel medicine);
        Task<T> Delete<T>(long? medicineId);
    }
}
