using MediatR;
using Microsoft.EntityFrameworkCore;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.MedicineCommands.DeletMedicine
{
    public class DeleteMedicineQueryHandler : AppDatabaseBase, IRequestHandler<DeleteMedicineQuery, Result<long?>>
    {
        public DeleteMedicineQueryHandler(AppDbContext context) : base(context)
        {
        }

        public async Task<Result<long?>> Handle(DeleteMedicineQuery request, CancellationToken cancellationToken)
        {
            var existing = await GetDBContext().Medicines.FirstOrDefaultAsync(e => e.MedicineId == request.medicineId);
            if (existing is null)
                return Result.Failure<long?>(new Error(StatusCodes.Status400BadRequest, "Medicine not found"));
            GetDBContext().Medicines.Remove(existing);
            await GetDBContext().SaveChangesAsync();
            return Result.Success(existing.MedicineId);
        }
    }
}
