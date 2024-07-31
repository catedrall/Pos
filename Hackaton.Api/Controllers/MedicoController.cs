using Hackaton.Api.Domain.Commands.Medico.Create;
using Hackaton.Api.Domain.Commands.Medico.Delete;
using Hackaton.Api.Domain.Commands.Medico.Update;
using Hackaton.Api.Domain.Queries.Medico.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading;

namespace Hackaton.Api.Controllers
{
    public class MedicoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("api/medico/criar")]
        public async Task<IActionResult> Criar([FromBody]CreateMedicoCommand medico, CancellationToken cancellation)
        {
            var result = await _mediator.Send(medico, cancellation);

            if (result != null)
            {
                return Created();
            }

            return BadRequest("Erro ao cadastrar o Médico!");

        }

        [HttpPut]
        [Route("api/medico/atualizar")]
        public async Task<IActionResult> Alterar([FromBody] UpdateMedicoCommand medico, CancellationToken cancellation)
        {
            var result = await _mediator.Send(medico, cancellation);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Erro ao atualizar o médico!");

        }

        [HttpGet]
        [Route("api/medico/medicos")]
        public async Task<IActionResult> Medicos(GetMedicoQuery medico, CancellationToken cancellation)
        {
            var result = await _mediator.Send(medico, cancellation);

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return Ok(JsonConvert.SerializeObject(result, jsonSettings));

        }

        [HttpDelete]
        [Route("api/medico/remover")]
        public async Task<IActionResult> Remover([FromQuery] DeleteMedicoCommand command, CancellationToken cancellation)
        {
            await _mediator.Send(command, cancellation);
            return Ok("Médico apagado com sucesso!");
        }
    }
}
