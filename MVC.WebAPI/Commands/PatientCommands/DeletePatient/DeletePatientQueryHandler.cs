using MediatR;
using Microsoft.EntityFrameworkCore;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.PatientCommands.DeletePatient
{
    public class DeletePatientQueryHandler : AppDatabaseBase, IRequestHandler<DeletePatientQuery, Result<long?>>
    {
        public DeletePatientQueryHandler(AppDbContext context) : base(context)
        {
        }

        public async Task<Result<long?>> Handle(DeletePatientQuery request, CancellationToken cancellationToken)
        {
            var existingPatient = await GetDBContext().Patients.FirstOrDefaultAsync(e => e.PatientId == request.patientId);
            if (existingPatient is null)
                return Result.Failure<long?>(new Error(StatusCodes.Status400BadRequest, "Patient not found"));
            GetDBContext().Patients.Remove(existingPatient);
            await GetDBContext().SaveChangesAsync();
            return Result.Success(existingPatient.PatientId);
        }
    }
}
