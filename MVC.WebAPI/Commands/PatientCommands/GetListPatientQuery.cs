using MediatR;
using MVC.Domain.Models;
using MVC.Shared;

namespace MVC.WebAPI.Commands.PatientCommands
{
    public record class GetListMedicineQuery() : IRequest<Result<IEnumerable<PatientModel>>>;
}
