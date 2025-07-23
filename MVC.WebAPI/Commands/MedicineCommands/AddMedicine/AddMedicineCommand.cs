using MediatR;

namespace MVC.WebAPI.Commands.MedicineCommands.AddMedicine
{
    public record class AddMedicineCommand(
        string medicineName,
        string qrCode) : IRequest<long?>;
}
