using Hackaton.Models;
using System.Threading;

namespace Hackaton.Api.Repository.Interface
{
    public interface IMedicoRepository
    {
        Task<IEnumerable<Medico>> GetAsync(int? Id, string? CRM, string? Email, bool? Ativo, DateTime? DataNascimento, CancellationToken cancellationToken = default);
        Task<Medico> GetByIdAsync(int Id, CancellationToken cancellationToken = default);
        Task DeleteLogicAsync(Medico medico, CancellationToken cancellationToken = default);
        Task CreateAsync(Medico medico, CancellationToken cancellation = default);
        Task UpdateAsync(Medico medico, CancellationToken cancellationToken = default);
    }
}
