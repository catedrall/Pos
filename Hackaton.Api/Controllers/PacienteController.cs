using Hackaton.Api.Domain.Commands.Paciente.Create;
using Hackaton.Api.Domain.Commands.Paciente.Update;
using Hackaton.Api.Domain.Queries.Paciente.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hackaton.Api.Controllers
{
    public class PacienteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PacienteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("api/paciente/criar")]
        public async Task<IActionResult> Criar([FromBody] CreatePacienteCommand paciente, CancellationToken cancellation)
        {
            var result = await _mediator.Send(paciente, cancellation);

            if (result)
            {
                return Created();
            }

            return BadRequest("Erro ao cadastrar o paciente!");

        }

        [HttpPut]
        [Route("api/paciente/atualizar")]
        public async Task<IActionResult> Alterar([FromBody] UpdatePacienteCommand paciente, CancellationToken cancellation)
        {
            var result = await _mediator.Send(paciente, cancellation);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Erro ao atualizar o paciente!");

        }

        [HttpGet]
        [Route("api/paciente/pacientes")]
        public async Task<IActionResult> Pacientes([FromQuery] GetPacienteQuery paciente, CancellationToken cancellation)
        {
            var result = await _mediator.Send(paciente, cancellation);

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return Ok(JsonConvert.SerializeObject(result, jsonSettings));

        }
    }

}
