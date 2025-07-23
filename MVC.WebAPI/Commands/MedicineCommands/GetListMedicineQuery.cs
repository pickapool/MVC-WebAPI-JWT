using MediatR;
using MVC.Domain.Models;
using MVC.Shared;

namespace MVC.WebAPI.Commands.MedicineCommands
{
    public record class GetListMedicineQuery() : IRequest<Result<IEnumerable<MedicineModel>>>;
}
