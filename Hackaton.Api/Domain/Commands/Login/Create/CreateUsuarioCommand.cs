using MediatR;

namespace Hackaton.Api.Domain.Commands.Login.Create
{
    public class CreateUsuarioCommand : IRequest<LoginResult?>
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
