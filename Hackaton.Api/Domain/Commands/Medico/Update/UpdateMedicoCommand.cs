using MediatR;

namespace Hackaton.Api.Domain.Commands.Medico.Update
{
    public class UpdateMedicoCommand : IRequest<bool>
    {
        public int Id { get; set;  }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CRM { get; set; }
        public string Especialidade { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
