using MediatR;
using MVC.Shared;

namespace MVC.WebAPI.Commands.PatientCommands.UpdateCommand
{
    public record class UpdatePatientCommand(
        long? patientId,
        string patientName,
        string roomName,
        string bedNumber) : IRequest<Result<long?>>;
}
