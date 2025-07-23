using MediatR;
using Microsoft.EntityFrameworkCore;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.MedicineCommands
{
    public class GetListMedicineQueryHandler : AppDatabaseBase, IRequestHandler<GetListMedicineQuery, Result<IEnumerable<MedicineModel>>>
    {
        public GetListMedicineQueryHandler(AppDbContext context) : base(context)
        {
        }

        public async Task<Result<IEnumerable<MedicineModel>>> Handle(GetListMedicineQuery request, CancellationToken cancellationToken)
        {
            var list = await GetDBContext().Medicines.ToListAsync();
            return Result.Success(list.AsEnumerable());
        }
    }
}
