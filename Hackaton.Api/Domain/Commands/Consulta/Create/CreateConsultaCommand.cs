using MediatR;

namespace Hackaton.Api.Domain.Commands.Consulta.Create
{
    public class CreateConsultaCommand : IRequest<bool>
    {
        public int IdMedico { get; set; }
        public int IdPaciente { get; set; }
        public DateTime DataAgendamento { get; set; }
    }
}
