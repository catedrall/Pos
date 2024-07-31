using MediatR;

namespace Hackaton.Api.Domain.Queries.Paciente.Get
{
    public class GetPacienteQuery : IRequest<GetPacienteResult?>
    {
        public int? Id { get; set; }
    }
}
