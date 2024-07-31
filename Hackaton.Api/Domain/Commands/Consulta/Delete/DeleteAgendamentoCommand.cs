using MediatR;

namespace Hackaton.Api.Domain.Commands.Consulta.Delete
{
    public class DeleteAgendamentoCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
