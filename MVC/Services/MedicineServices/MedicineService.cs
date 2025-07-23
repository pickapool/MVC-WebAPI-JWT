using MVC.Domain;
using MVC.Domain.Models;
using MVC.Services.BaseService;

namespace MVC.Services.MedicineServices
{
    public class MedicineService : IMedicineService
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseService _baseService;
        private readonly string defaultRequestUrl;
        private RequestModel request = new();
        public MedicineService(IConfiguration configuration, IBaseService baseService)
        {
            _configuration = configuration;
            _baseService = baseService;
            defaultRequestUrl = $"{_configuration["BaseAPIUrls:UserAPI"]}/api/medicine";
        }
        public async Task<T> Create<T>(MedicineModel medicine)
        {
            request = new()
            {
                RequestUrl = $"{defaultRequestUrl}/add",
                Data = new
                {
                    medicineName = medicine.MedicineName,
                    qrCode = medicine.QRCode
                },
                RequestType = Enums.RequestType.POST
            };
            return await _baseService.SendAsync<T>(request);
        }

        public async Task<T> Delete<T>(long? medicineId)
        {
            request = new()
            {
                RequestUrl = $"{defaultRequestUrl}/delete",
                Data = new { medicineId = medicineId },
                RequestType = Enums.RequestType.DELETE
            };
            return await _baseService.SendAsync<T>(request);
        }

        public async Task<T> GetAll<T>()
        {
            request = new()
            {
                RequestUrl = $"{defaultRequestUrl}/",
                Data = null,
                RequestType = Enums.RequestType.GET
            };
            return await _baseService.SendAsync<T>(request);
        }

        public async Task<T> GetById<T>(long? medicineId)
        {
            request = new()
            {
                RequestUrl = $"{defaultRequestUrl}/getid/{medicineId}",
                Data = null,
                RequestType = Enums.RequestType.GET
            };
            return await _baseService.SendAsync<T>(request);
        }

        public async Task<T> Update<T>(MedicineModel medicine)
        {
            request = new()
            {
                RequestUrl = $"{defaultRequestUrl}/update",
                Data = new
                {
                    medicineId = medicine.MedicineId,
                    medicineName = medicine.MedicineName,
                    qrCode = medicine.QRCode
                },
                RequestType = Enums.RequestType.PUT
            };
            return await _baseService.SendAsync<T>(request);
        }
    }
}
