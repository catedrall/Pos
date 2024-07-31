using Hackaton.Api.Repository.Interface;
using MediatR;

namespace Hackaton.Api.Domain.Queries.Paciente.Get
{
    public class GetPacienteHandle : IRequestHandler<GetPacienteQuery, GetPacienteResult?>
    {
        private readonly IPacienteRepository _pacienteRepository;

        public GetPacienteHandle(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async Task<GetPacienteResult?> Handle(GetPacienteQuery query, CancellationToken cancellationToken)
        {   
            var result = await _pacienteRepository.GetByIdAsync(query.Id,cancellationToken);
            var paciente = new GetPacienteResult();

            if(result is not null)
                paciente.Pacientes.AddRange(result);

            return paciente;
        }
    }
}
