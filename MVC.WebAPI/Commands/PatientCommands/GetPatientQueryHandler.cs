using MediatR;
using Microsoft.EntityFrameworkCore;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.PatientCommands
{
    public class GetPatientQueryhandler : AppDatabaseBase, IRequestHandler<GetPatientQuery, Result<IEnumerable<PatientModel>>>
    {
        public GetPatientQueryhandler(AppDbContext context) : base(context)
        {
        }

        public async Task<Result<IEnumerable<PatientModel>>> Handle(GetPatientQuery request, CancellationToken cancellationToken)
        {
            var list = await GetDBContext().Patients.ToListAsync();
            return Result.Success(list.AsEnumerable());
        }
    }
}
