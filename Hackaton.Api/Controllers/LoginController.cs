using Hackaton.Api.Domain.Commands.Login.Create;
using Hackaton.Api.Domain.Commands.Login.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Hackaton.Api.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("api/Login")]
        public async Task<IActionResult> Login([FromBody] CreateUsuarioCommand usuario, CancellationToken cancellation)
        {
            var result = await _mediator.Send(usuario, cancellation);

            if (result is not null)
            {
                return Ok(result);
            }

            return BadRequest("Erro ao efetuar login!");
        }

        [HttpPost]
        [Route("api/Login/alterar/{id}")]
        public async Task<ActionResult> alterar([FromQuery] int id, [FromBody] UpdateLoginCommand usuario, CancellationToken cancellation)
        {
            usuario.SetId(id);
            var result = await _mediator.Send(usuario, cancellation);

            if (result is not null)
            {
                return Ok("Login Alterado!");
            }

            return BadRequest("Erro ao alterar login!");
        }

    }
}
