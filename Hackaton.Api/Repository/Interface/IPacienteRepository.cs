using Hackaton.Models;

namespace Hackaton.Api.Repository.Interface
{
    public interface IPacienteRepository
    {
        Task CreateAsync(Paciente paciente, CancellationToken cancellation = default);
        Task UpdateAsync(Paciente paciente, CancellationToken cancellationToken = default);
        Task<IEnumerable<Paciente>?> GetByIdAsync(int? Id, CancellationToken cancellationToken = default);
    }
}
