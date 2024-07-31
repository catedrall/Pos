using MediatR;

namespace Hackaton.Api.Domain.Commands.Medico.Delete
{
    public class DeleteMedicoCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
