using MediatR;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.WebAPI.Interfaces;

namespace MVC.WebAPI.Commands.PatientCommands.AddPatient
{
    public class AddPatientCommandHandler : AppDatabaseBase, IRequestHandler<AddPatientCommand, long?>
    {
        public AddPatientCommandHandler(AppDbContext context) : base(context)
        {
        }

        public async Task<long?> Handle(AddPatientCommand request, CancellationToken cancellationToken)
        {
            PatientModel patientModel = new PatientModel
            {
                PatientName = request.patientName,
                RoomName = request.roomName,
                BedNumber = request.bedNumber,
                CreatedDate = DateTime.UtcNow
            };
            GetDBContext().Patients.Add(patientModel);
            await GetDBContext().SaveChangesAsync();
            return patientModel.PatientId;
        }
    }
}
