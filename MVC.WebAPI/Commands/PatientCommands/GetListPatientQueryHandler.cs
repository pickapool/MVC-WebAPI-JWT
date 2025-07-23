using MediatR;
using Microsoft.EntityFrameworkCore;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.PatientCommands
{
    public class GetListMedicineQueryHandler : AppDatabaseBase, IRequestHandler<GetListMedicineQuery, Result<IEnumerable<PatientModel>>>
    {
        public GetListMedicineQueryHandler(AppDbContext context) : base(context)
        {
        }

        public async Task<Result<IEnumerable<PatientModel>>> Handle(GetListMedicineQuery request, CancellationToken cancellationToken)
        {
            var list = await GetDBContext().Patients.ToListAsync();
            return Result.Success(list.AsEnumerable());
        }
    }
}
