using MediatR;

namespace Hackaton.Api.Domain.Commands.Paciente.Update
{
    public class UpdatePacienteCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
