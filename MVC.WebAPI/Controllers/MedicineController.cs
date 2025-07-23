using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.WebAPI.Commands.MedicineCommands;
using MVC.WebAPI.Commands.MedicineCommands.AddMedicine;
using MVC.WebAPI.Commands.MedicineCommands.DeletMedicine;
using MVC.WebAPI.Commands.MedicineCommands.UpdateMedicine;
using MVC.WebAPI.Commands.MedicineGetById.PatientGetByID;

namespace MVC.WebAPI.Controllers
{
    [Route("api/medicine/")]
    [ApiController]
    [AllowAnonymous]
    public class MedicineController : ControllerBase
    {
        private readonly ISender _sender;
        public MedicineController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddMedicineCommand command)
        {
            var result = await _sender.Send(command);
            return Ok(result.Value);
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateMedicineCommand command)
        {
            var result = await _sender.Send(command);
            return Ok(result.Value);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            GetListMedicineQuery qeury = new();
            var result = await _sender.Send(qeury);
            return Ok(result.Value);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteMedicineQuery query)
        {
            var result = await _sender.Send(query);
            if(result.Error.Code == StatusCodes.Status400BadRequest)
                return BadRequest(result.Error.Description);
            return Ok(result.Value);
        }
        [HttpGet]
        [Route("getid/{MedicineId}")]
        public async Task<IActionResult> GetById(long? MedicineId)
        {
            GetMedicineByIDQuery query = new(MedicineId);

            var result = await _sender.Send(query);
            if (result.IsFailure)
                return BadRequest(result.Error.Description);
            return Ok(result.Value);
        }
    }
}
