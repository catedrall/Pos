using Hackaton.Api.Data;
using Hackaton.Worker.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace Hackaton.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailService _emailService;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger, IEmailService emailService)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<DbContextClass>();

                        var proximoDiaUtil = GetProximoDiaUtil(DateTime.Now);

                        var agendamentos = await dbContext.Agenda
                            .Where(a => a.DataAgendamento.Date == proximoDiaUtil.Date)
                            .ToListAsync();

                        foreach (var agendamento in agendamentos)
                        {
                            var paciente = await dbContext.Paciente.FindAsync(agendamento.IdPaciente);
                            var medico = await dbContext.Medico.FindAsync(agendamento.IdMedico);

                            if (paciente != null && medico != null)
                            {
                                _emailService.EnviarEmail(paciente.Email, paciente.Nome, medico.Nome, proximoDiaUtil);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ocorreu um erro ao executar o worker.");
                }

                // Espera 24 horas antes de executar novamente
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }

        private DateTime GetProximoDiaUtil(DateTime data)
        {
            do
            {
                data = data.AddDays(1);
            } while (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday);

            return data;
        }
    }
}
