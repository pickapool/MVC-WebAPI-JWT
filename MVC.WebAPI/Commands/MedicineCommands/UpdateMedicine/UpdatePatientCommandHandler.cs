using MediatR;
using Microsoft.EntityFrameworkCore;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.Shared.Extensions;
using MVC.WebAPI.Commands.MedicineCommands.UpdateMedicine;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.MedicineCommands.UpdateMedicine
{
    public class UpdateMedicineCommandHandler : AppDatabaseBase, IRequestHandler<UpdateMedicineCommand, Result<long?>>
    {
        public UpdateMedicineCommandHandler(AppDbContext context) : base(context)
        {
        }

        public async Task<Result<long?>> Handle(UpdateMedicineCommand request, CancellationToken cancellationToken)
        {
            var existing = await GetDBContext().Medicines.FirstOrDefaultAsync( e => e.MedicineId == request.medicineId );
            if (existing is null)
                return Result.Failure<long?>(new Error(StatusCodes.Status400BadRequest, "Patient not found"));

            existing.MedicineName = request.medicineName;
            existing.QRCode = request.qrCode;

            GetDBContext().Medicines.Update(existing);
            await GetDBContext().SaveChangesAsync();
            return existing.MedicineId;
        }
    }
}
