using MediatR;
using MVC.Shared;

namespace MVC.WebAPI.Commands.MedicineCommands.DeletMedicine
{
    public record class DeleteMedicineQuery(long? medicineId) : IRequest<Result<long?>>;
}
