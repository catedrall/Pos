using Hackaton.Api.Data;
using Hackaton.Models;
using Hackaton.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace Hackaton.Api.Repository
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly DbContextClass _context;

        public MedicoRepository(DbContextClass context)
        {
            _context = context;
        }

        public async Task CreateAsync(Medico medico, CancellationToken cancellation = default)
        {
            _context.Medico.Add(medico);
        }

        public async Task UpdateAsync(Medico medico, CancellationToken cancellationToken = default)
        {
            _context.Medico.Update(medico);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteLogicAsync(Medico medico, CancellationToken cancellationToken = default)
        {
            _context.Medico.Update(medico);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Medico>> GetAsync(int? Id, string? CRM, string? Email, bool? Ativo, DateTime? DataNascimento, CancellationToken cancellationToken = default)
        {
            try
            {
                IQueryable<Medico>? medicos = _context.Medico;

                if (CRM is not null)
                {
                    medicos = medicos.Where(w => w.CRM.Contains(CRM));
                }

                if (Email is not null)
                {
                    medicos = medicos.Where(w => w.Email.Equals(Email));
                }
                if (Ativo is not null)
                {
                    medicos = medicos.Where(w => w.Ativo == Ativo);
                }
                if (DataNascimento is not null)
                {
                    medicos = medicos.Where(w => w.DataNascimento == DataNascimento);
                }

                return await medicos.ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public async Task<Medico?> GetByIdAsync(int Id, CancellationToken cancellationToken = default)
        {
            return await _context.Medico.FirstOrDefaultAsync(w => w.Id == Id);
        }
    }
}