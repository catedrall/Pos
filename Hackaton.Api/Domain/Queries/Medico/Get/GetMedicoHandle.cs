using Hackaton.Api.Repository.Interface;
using MediatR;

namespace Hackaton.Api.Domain.Queries.Medico.Get
{
    public class GetMedicoHandle : IRequestHandler<GetMedicoQuery, GetMedicoResult>
    {
        private readonly IMedicoRepository _medicoRepository;

        public GetMedicoHandle(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        public async Task<GetMedicoResult> Handle(GetMedicoQuery query, CancellationToken cancellationToken)
        {
            var result = await _medicoRepository.GetAsync(query.Id, query.CRM, query.Email, query.Ativo, query.DataNascimento, cancellationToken);
            var clients = new GetMedicoResult();
            clients.Medicos = result;

            return clients;
        }
    }
}
