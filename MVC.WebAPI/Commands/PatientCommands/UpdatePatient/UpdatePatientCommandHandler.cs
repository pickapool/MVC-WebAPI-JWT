using MediatR;
using Microsoft.EntityFrameworkCore;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.Shared.Extensions;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.PatientCommands.UpdateCommand
{
    public class UpdatePatientCommandHandler : AppDatabaseBase, IRequestHandler<UpdatePatientCommand, Result<long?>>
    {
        public UpdatePatientCommandHandler(AppDbContext context) : base(context)
        {
        }

        public async Task<Result<long?>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var existinPatient = await GetDBContext().Patients.FirstOrDefaultAsync( e => e.PatientId == request.patientId );
            if (existinPatient is null)
                return Result.Failure<long?>(new Error(StatusCodes.Status400BadRequest, "Patient not found"));

            existinPatient.MapToPatient(request);

            GetDBContext().Patients.Update(existinPatient);
            await GetDBContext().SaveChangesAsync();
            return existinPatient.PatientId;
        }
    }
}
