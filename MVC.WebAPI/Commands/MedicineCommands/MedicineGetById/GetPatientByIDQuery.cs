using MediatR;
using MVC.Domain.Models;
using MVC.Shared;

namespace MVC.WebAPI.Commands.MedicineGetById.PatientGetByID
{
    public record class GetMedicineByIDQuery(long? medicineId) : IRequest<Result<MedicineModel>>;
}
