using MediatR;
using MVC.Shared;

namespace MVC.WebAPI.Commands.PatientCommands.DeletePatient
{
    public record class DeletePatientQuery(long? patientId) : IRequest<Result<long?>>;
}
