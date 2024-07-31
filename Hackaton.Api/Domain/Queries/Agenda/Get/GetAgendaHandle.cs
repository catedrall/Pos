using Hackaton.Api.Repository.Interface;
using MediatR;

namespace Hackaton.Api.Domain.Queries.Agenda.Get
{
    public class GetAgendaHandle : IRequestHandler<GetConsultaQuery, GetAgendaResult>
    {
        private readonly IAgendaRepository _agendaRepository;
        

        public GetAgendaHandle(IAgendaRepository agendaRepository)
        {
            _agendaRepository = agendaRepository;
        }

        public async Task<GetAgendaResult> Handle(GetConsultaQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Models.Consulta> result;
            if (query.IdMedico > 0)
            {
                result = await _agendaRepository.GetAsyncMedico(query.Id, query.IdMedico, query.IdPaciente);
            }
            else
            {
                result = await _agendaRepository.GetAsync(query.Id, query.IdMedico, query.IdPaciente);
            }
            var agenda = new GetAgendaResult();
            agenda.Agendamentos = result;

            return agenda;
        }
    }
}
