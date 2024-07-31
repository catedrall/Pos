using MediatR;

namespace Hackaton.Api.Domain.Queries.Agenda.Get
{
    public class GetConsultaQuery : IRequest<GetAgendaResult?>
    {
        public int? Id { get; set; }
        public int? IdMedico { get; set; }
        public int? IdPaciente { get; set; }
    }
}
