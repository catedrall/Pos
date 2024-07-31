using Hackaton.Models;
using System.Threading;

namespace Hackaton.Api.Repository.Interface
{
    public interface IAgendaRepository
    {
        Task<IEnumerable<Consulta>?> GetByIdMedicoAsync(int idMedico, CancellationToken cancellationToken = default);
        Task<Consulta?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteLogicAsync(Consulta client, CancellationToken cancellationToken = default);
        Task<IEnumerable<Consulta>?> GetAsync(int? Id, int? IdMedico, int? IdPaciente, CancellationToken cancellationToken = default);
        Task<IEnumerable<Consulta>?> GetAsyncMedico(int? Id, int? IdMedico, int? IdPaciente, CancellationToken cancellationToken = default);
        Task CreateAsync(Consulta agenda, CancellationToken cancellation = default);
        Task UpdateAsync(Consulta agenda, CancellationToken cancellationToken = default);
        Task<Consulta?> GetAgendamentoAsync(int? Id, int? IdMedico, int? IdPaciente, DateTime? DataAgendamento, CancellationToken cancellationToken = default);
    }
}
