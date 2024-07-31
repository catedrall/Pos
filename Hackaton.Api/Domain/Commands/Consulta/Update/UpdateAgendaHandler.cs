using FluentValidation;
using Hackaton.Api.Repository.Interface;
using Hackaton.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Hackaton.Api.Domain.Commands.Consulta.Update
{
    public class UpdateAgendaHandler : IRequestHandler<UpdateConsultaCommand, bool>
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IAgendaRepository _agendamentoRepository;
        private readonly IValidator<UpdateConsultaCommand> _validator;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UpdateAgendaHandler(IMedicoRepository medicoRepository, IAgendaRepository agendamentoRepository
            ,IPacienteRepository pacienteRepository, IValidator<UpdateConsultaCommand> validator, HttpClient httpClient, IConfiguration configuration)
        {
            _medicoRepository = medicoRepository;
            _pacienteRepository = pacienteRepository;
            _agendamentoRepository = agendamentoRepository;
            _validator = validator;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> Handle(UpdateConsultaCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var agendamento = await _agendamentoRepository.GetAgendamentoAsync(command.Id, null, null, null, cancellationToken);
            if (agendamento is null)
                return false;

            agendamento.DataAgendamento = new DateTime(command.NovaDataAgendamento.Year, command.NovaDataAgendamento.Month, command.NovaDataAgendamento.Day, command.Hora, 0, 0);
            
            await _agendamentoRepository.UpdateAsync(agendamento, cancellationToken);

            await EnviaEmai(agendamento.PacienteId, agendamento.DataAgendamento);

            return true;
        }

        private async Task EnviaEmai(int idPaciente, DateTime dataConsulta)
        {
            string url = _configuration.GetSection("Aviso:EndPoint").Value;
            var result = await _pacienteRepository.GetByIdAsync(idPaciente);
            var paciente = result.FirstOrDefault();
            Aviso aviso = new Aviso
            {
                Data = dataConsulta,
                Email = paciente.Email,
                Nome = paciente.Nome,
                Tipo = 3
            };

            var jsonContent = JsonConvert.SerializeObject(aviso);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await _httpClient.PostAsync(url, contentString);
        }
    }
}
