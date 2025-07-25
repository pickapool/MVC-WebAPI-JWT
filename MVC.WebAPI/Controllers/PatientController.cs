﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.WebAPI.Commands.PatientCommands;
using MVC.WebAPI.Commands.PatientCommands.AddPatient;
using MVC.WebAPI.Commands.PatientCommands.DeletePatient;
using MVC.WebAPI.Commands.PatientCommands.PatientGetByID;
using MVC.WebAPI.Commands.PatientCommands.UpdateCommand;

namespace MVC.WebAPI.Controllers
{
    [Route("api/patient/")]
    [ApiController]
    [AllowAnonymous]
    public class PatientController : ControllerBase
    {
        private readonly ISender _sender;
        public PatientController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddPatientCommand command)
        {
            var result = await _sender.Send(command);
            return Ok(result.Value);
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdatePatientCommand command)
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
        public async Task<IActionResult> Delete([FromBody] DeletePatientQuery query)
        {
            var result = await _sender.Send(query);
            if(result.Error.Code == StatusCodes.Status400BadRequest)
                return BadRequest(result.Error.Description);
            return Ok(result.Value);
        }
        [HttpGet]
        [Route("getid/{patientId}")]
        public async Task<IActionResult> GetById(long? patientId)
        {
            GetPatientByIDQuery query = new(patientId);

            var result = await _sender.Send(query);
            if (result.IsFailure)
                return BadRequest(result.Error.Description);
            return Ok(result.Value);
        }
        [HttpGet("getmedicines/{qr}")]
        public async Task<IActionResult> GetMedicines(string qr)
        {
            GetPatientsMedicineQuery query = new(qr);

            var result = await _sender.Send(query);
            return Ok(result.Value);
        }

    }
}
