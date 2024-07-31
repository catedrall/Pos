using MediatR;
using System.Security.Cryptography;

namespace Hackaton.Api.Domain.Queries.Medico.Get
{
    public class GetMedicoQuery : IRequest<GetMedicoResult?>
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? CRM { get; set; }
        public string? Especialidade { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool? Ativo { get; set; }
    }
}
