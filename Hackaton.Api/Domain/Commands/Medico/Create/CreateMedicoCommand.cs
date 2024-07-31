using MediatR;

namespace Hackaton.Api.Domain.Commands.Medico.Create
{
    public class CreateMedicoCommand : IRequest<bool>
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string CRM { get; set; }
        public string Especialidade { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
