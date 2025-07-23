using MediatR;
using Microsoft.EntityFrameworkCore;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.PatientCommands.PatientGetByID
{
    public class GetPatientByIDQueryHandler : AppDatabaseBase, IRequestHandler<GetPatientByIDQuery, Result<PatientModel>>
    {
        public GetPatientByIDQueryHandler(AppDbContext context) : base(context) { }

        public async Task<Result<PatientModel>> Handle(GetPatientByIDQuery request, CancellationToken cancellationToken)
        {
            var patient = await GetDBContext().Patients
                .Include( e => e.Medicines)
                .ThenInclude(e => e.Medicine)
                .FirstOrDefaultAsync(e => e.PatientId == request.patientId);
            if (patient is null)
                return Result.Failure<PatientModel>(new Error(StatusCodes.Status400BadRequest, "Not Found"));
            return Result.Success(patient);
        }
    }
}
