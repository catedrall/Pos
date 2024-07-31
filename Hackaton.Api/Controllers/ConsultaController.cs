using Hackaton.Api.Domain.Commands.Consulta.Create;
using Hackaton.Api.Domain.Commands.Consulta.Delete;
using Hackaton.Api.Domain.Commands.Consulta.Update;
using Hackaton.Api.Domain.Commands.Medico.Delete;
using Hackaton.Api.Domain.Queries.Agenda.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hackaton.Api.Controllers
{
    public class ConsultaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConsultaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("api/agenda/agendar")]
        public async Task<IActionResult> Criar([FromBody] CreateConsultaCommand agenda, CancellationToken cancellation)
        {
            var result = await _mediator.Send(agenda, cancellation);

            if (result)
            {
                return Created();
            }

            return BadRequest("Erro ao cadastrar o Agendamento!");

        }

        [HttpPut]
        [Route("api/agenda/reagendar")]
        public async Task<IActionResult> Alterar([FromBody] UpdateConsultaCommand agenda, CancellationToken cancellation)
        {
            var result = await _mediator.Send(agenda, cancellation);

            if (result != null)
            {
                return NoContent();
            }

            return BadRequest("Erro ao atualizar o Agendamento!");

        }

        [HttpGet]
        [Route("api/agenda/agenda")]
        public async Task<IActionResult> Agendas([FromQuery] GetConsultaQuery agenda, CancellationToken cancellation)
        {
            var result = await _mediator.Send(agenda, cancellation);

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return Ok(JsonConvert.SerializeObject(result, jsonSettings));

        }

        [HttpDelete]
        [Route("api/agenda/remover/{id}")]
        public async Task<IActionResult> Remover(int id, CancellationToken cancellation)
        {
            var command = new DeleteAgendamentoCommand { Id = id };
            await _mediator.Send(command, cancellation);
            return Ok("Agendamento apagado com sucesso!");
        }
    }
}
