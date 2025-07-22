using MediatR;
using MVC.Shared;

namespace MVC.WebAPI.Commands.PatientCommands
{
    public record class DeletePatientQuery(long? patientId) : IRequest<Result<long?>>;
}
