using MediatR;

namespace Hackaton.Api.Domain.Commands.Paciente.Create
{
    public class CreatePacienteCommand : IRequest<bool>
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
