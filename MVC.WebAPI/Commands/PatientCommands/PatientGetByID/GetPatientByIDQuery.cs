using MediatR;
using MVC.Domain.Models;
using MVC.Shared;

namespace MVC.WebAPI.Commands.PatientCommands.PatientGetByID
{
    public record class GetPatientByIDQuery(long? patientId) : IRequest<Result<PatientModel>>;
}
