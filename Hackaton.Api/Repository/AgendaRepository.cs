using Hackaton.Api.Data;
using Hackaton.Api.Repository.Interface;
using Hackaton.Models;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Api.Repository
{
    public class AgendaRepository : IAgendaRepository
    {
        private readonly DbContextClass _context;

        public AgendaRepository(DbContextClass context)
        {
            _context = context;
        }

        public async Task DeleteLogicAsync(Consulta client, CancellationToken cancellationToken = default)
        {
            _context.Agenda.Update(client);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Consulta>?> GetAsync(int? Id, int? IdMedico, int? IdPaciente, CancellationToken cancellationToken = default)
        {
            
                List<Consulta> agenda = null;

                if (IdPaciente.Value > 0 && IdMedico.Value > 0)
                {
                    agenda = await _context.Agenda.AsNoTracking().Include(k => k.Medico).AsNoTracking().Where(k => k.PacienteId == IdPaciente && k.MedicoId == IdMedico && k.Ativo == true).ToListAsync();
                }

                if (IdPaciente.Value == 0 && IdMedico.Value > 0)
                {
                    agenda = await _context.Agenda.AsNoTracking().Include(k => k.Medico).AsNoTracking().Where(k => k.MedicoId == IdMedico && k.Ativo == true).ToListAsync();
                }

                if (IdPaciente.Value > 0 && IdMedico.Value == 0)
                {
                    agenda = await _context.Agenda.AsNoTracking().Include(k => k.Medico).AsNoTracking().Where(k => k.PacienteId == IdPaciente && k.Ativo == true).ToListAsync();
                }
              
                return agenda;
            
        }

        public async Task<IEnumerable<Consulta>?> GetAsyncMedico(int? Id, int? IdMedico, int? IdPaciente, CancellationToken cancellationToken = default)
        {
            List<Consulta> agenda = null;

            if (IdPaciente.Value > 0 && IdMedico.Value > 0)
            {
                agenda = await _context.Agenda.AsNoTracking().Include(k => k.Paciente).AsNoTracking().Where(k => k.PacienteId == IdPaciente && k.MedicoId == IdMedico && k.Ativo == true).ToListAsync();
            }

            if (IdPaciente.Value == 0 && IdMedico.Value > 0)
            {
                agenda = await _context.Agenda.AsNoTracking().Include(k => k.Paciente).AsNoTracking().Where(k => k.MedicoId == IdMedico && k.Ativo == true).ToListAsync();
            }

            if (IdPaciente.Value > 0 && IdMedico.Value == 0)
            {
                agenda = await _context.Agenda.AsNoTracking().Include(k => k.Paciente).AsNoTracking().Where(k => k.PacienteId == IdPaciente && k.Ativo == true).ToListAsync();
            }

            return agenda;
        }

        public async Task CreateAsync(Consulta agenda, CancellationToken cancellation = default)
        {
            try
            {
                _context.Agenda.Add(agenda);
                await _context.SaveChangesAsync(cancellation);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<Consulta>?> GetByIdMedicoAsync(int idMedico, CancellationToken cancellationToken = default)
        {
            return await _context.Agenda.Where(w => w.MedicoId == idMedico).ToListAsync();
        }

        public async Task<Consulta?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Agenda.Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Consulta agenda, CancellationToken cancellationToken = default)
        {
            _context.Agenda.Update(agenda);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Consulta?> GetAgendamentoAsync(int? Id, int? IdMedico, int? IdPaciente, DateTime? DataAgendamento, CancellationToken cancellationToken = default)
        {
            IQueryable<Consulta>? agendas = _context.Agenda;

            if (Id is not null)
            {
                agendas = agendas.Where(w => w.Id == Id);
            }

            if (IdMedico is not null)
            {
                agendas = agendas.Where(w => w.MedicoId == IdMedico);
            }

            if (IdPaciente is not null)
            {
                agendas = agendas.Where(w => w.PacienteId == IdPaciente);
            }

            if (DataAgendamento is not null)
            {
                agendas = agendas.Where(w => w.DataAgendamento == DataAgendamento);
            }

            return await agendas.FirstOrDefaultAsync(cancellationToken);
        }

    }
}