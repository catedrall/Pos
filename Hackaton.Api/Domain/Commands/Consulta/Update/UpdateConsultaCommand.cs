using MediatR;

namespace Hackaton.Api.Domain.Commands.Consulta.Update
{
    public class UpdateConsultaCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DateTime NovaDataAgendamento { get; set; }
        public int Hora { get; set; }
    }
}
