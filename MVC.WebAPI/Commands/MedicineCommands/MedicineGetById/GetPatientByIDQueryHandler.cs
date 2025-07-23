using MediatR;
using Microsoft.EntityFrameworkCore;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.MedicineGetById.PatientGetByID
{
    public class GetMedicineByIDQueryHandler : AppDatabaseBase, IRequestHandler<GetMedicineByIDQuery, Result<MedicineModel>>
    {
        public GetMedicineByIDQueryHandler(AppDbContext context) : base(context) { }

        public async Task<Result<MedicineModel>> Handle(GetMedicineByIDQuery request, CancellationToken cancellationToken)
        {
            var medicine = await GetDBContext().Medicines
                .FirstOrDefaultAsync(e => e.MedicineId == request.medicineId);
            if (medicine is null)
                return Result.Failure<MedicineModel>(new Error(StatusCodes.Status400BadRequest, "Not Found"));
            return Result.Success(medicine);
        }
    }
}
