using MediatR;
using Microsoft.EntityFrameworkCore;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.Shared;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.PatientCommands
{
    public class GetPatientsMedicineQueryHandler : AppDatabaseBase, IRequestHandler<GetPatientsMedicineQuery, Result<List<PatientModel>>>
    {
        public GetPatientsMedicineQueryHandler(AppDbContext context) : base(context) { }

        public async Task<Result<List<PatientModel>>> Handle(GetPatientsMedicineQuery request, CancellationToken cancellationToken)
        {
            var list = await GetDBContext()
                .Patients.Include( e => e.Medicines?? new())
                .ThenInclude( e => e.Medicine)
                .Where( e => (e.Medicines?? new()).Any( c => (c.Medicine?? new()).QRCode == request.qrCode))
                .ToListAsync();
            if (list is null)
                return Result.Success(new List<PatientModel>());
            return Result.Success(list);
        }
    }
}
