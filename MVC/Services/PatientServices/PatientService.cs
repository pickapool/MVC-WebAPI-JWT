
using MVC.Domain;
using MVC.Domain.Models;
using MVC.Services.BaseService;

namespace MVC.Services.PatientServices
{
    public class PatientService : IPatientService
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseService _baseService;
        private readonly string defaultRequestUrl;
        private RequestModel request = new();
        public PatientService(IConfiguration configuration, IBaseService baseService)
        {
            _configuration = configuration;
            _baseService = baseService;
            defaultRequestUrl = $"{_configuration["BaseAPIUrls:UserAPI"]}/api/patient";
        }
        public async Task<T> Create<T>(PatientModel patient)
        {
            request = new();
            request.RequestUrl = $"{defaultRequestUrl}/add";
            request.Data = new {
                patientName = patient.PatientName,
                roomName = patient.RoomName,
                bedNumber = patient.BedNumber,
            };
            request.RequestType = Enums.RequestType.POST;
            return await _baseService.SendAsync<T>(request);
        }

        public async Task<T> Delete<T>(long? p)
        {
            request = new();
            request.RequestUrl = $"{defaultRequestUrl}/delete";
            request.Data = new { patientId = p};
            request.RequestType = Enums.RequestType.DELETE;
            return await _baseService.SendAsync<T>(request);
        }

        public async Task<T> GetAll<T>()
        {
            request = new();
            request.RequestUrl = $"{defaultRequestUrl}/";
            request.Data = null;
            request.RequestType = Enums.RequestType.GET;
            return await _baseService.SendAsync<T>(request);
        }

        public async Task<T> GetById<T>(long? id)
        {
            request = new();
            request.RequestUrl = $"{defaultRequestUrl}/getid/{id}";
            request.Data = null;
            request.RequestType = Enums.RequestType.GET;
            return await _baseService.SendAsync<T>(request);
        }

        public async Task<T> Update<T>(PatientModel patient)
        {
            request = new();
            request.RequestUrl = $"{defaultRequestUrl}/update";
            request.Data = new
            {
                patientId = patient.PatientId,
                patientName = patient.PatientName,
                roomName = patient.RoomName,
                bedNumber = patient.BedNumber,
            };
            request.RequestType = Enums.RequestType.PUT;
            return await _baseService.SendAsync<T>(request);
        }
    }
}
