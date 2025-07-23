using MediatR;
using MVC.Shared;

namespace MVC.WebAPI.Commands.MedicineCommands.UpdateMedicine
{
    public record class UpdateMedicineCommand(
        long? medicineId,
        string medicineName,
        string qrCode) : IRequest<Result<long?>>;
}
