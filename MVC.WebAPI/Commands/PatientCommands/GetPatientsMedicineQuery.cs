using MediatR;
using MVC.Domain.Models;
using MVC.Shared;

namespace MVC.WebAPI.Commands.PatientCommands
{
    public record class GetPatientsMedicineQuery(string? qrCode) : IRequest<Result<List<PatientModel>>>;
}
