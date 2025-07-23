using MediatR;
using MVC.Domain.Models;
using Results = MVC.Shared.Result;

namespace MVC.WebAPI.Commands.PatientCommands.AddPatient
{
    public record class AddPatientCommand(
        string patientName,
        string roomName,
        string bedNumber) : IRequest<long?>;
}
