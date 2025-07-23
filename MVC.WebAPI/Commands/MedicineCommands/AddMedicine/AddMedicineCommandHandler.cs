using MediatR;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.MedicineCommands.AddMedicine
{
    public class AddMedicineCommandHandler : AppDatabaseBase, IRequestHandler<AddMedicineCommand, long?>
    {
        public AddMedicineCommandHandler(AppDbContext context) : base(context)
        {
        }

        public async Task<long?> Handle(AddMedicineCommand request, CancellationToken cancellationToken)
        {
            MedicineModel medicine = new MedicineModel
            {
                MedicineName = request.medicineName,
                QRCode = request.qrCode
            };
            GetDBContext().Medicines.Add(medicine);
            await GetDBContext().SaveChangesAsync();
            return medicine.MedicineId;
        }
    }
}
