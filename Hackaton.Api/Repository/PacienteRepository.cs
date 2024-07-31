using Hackaton.Api.Data;
using Hackaton.Api.Repository.Interface;
using Hackaton.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Hackaton.Api.Repository
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly DbContextClass _context;

        public PacienteRepository(DbContextClass context)
        {
            _context = context;
        }

        public async Task CreateAsync(Paciente paciente, CancellationToken cancellation = default)
        {
            _context.Paciente.Add(paciente);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(Paciente paciente, CancellationToken cancellationToken = default)
        {
            _context.Paciente.Update(paciente);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Paciente>> GetByIdAsync(int? Id, CancellationToken cancellationToken = default)
        {
            if (Id is not null)
                return await _context.Paciente.Where(w => w.Id == Id).ToListAsync();

            return await _context.Paciente.ToListAsync();
        }
    }
}