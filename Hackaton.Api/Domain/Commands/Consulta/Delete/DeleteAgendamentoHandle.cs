using FluentValidation;
using Hackaton.Api.Domain.Commands.Medico.Delete;
using Hackaton.Api.Repository;
using Hackaton.Api.Repository.Interface;
using Hackaton.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Hackaton.Api.Domain.Commands.Consulta.Delete
{
    public class DeleteAgendamentoHandle : IRequestHandler<DeleteAgendamentoCommand, bool>
    {
        private readonly IAgendaRepository _agendamentoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IValidator<DeleteAgendamentoCommand> _validator;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DeleteAgendamentoHandle(IAgendaRepository agendamentoRepository, IValidator<DeleteAgendamentoCommand> validator, HttpClient httpClient, IConfiguration configuration, IPacienteRepository pacienteRepository)
        {
            _agendamentoRepository = agendamentoRepository;
            _validator = validator;
            _httpClient = httpClient;
            _configuration = configuration;
            _pacienteRepository = pacienteRepository;
        }

        public async Task<bool> Handle(DeleteAgendamentoCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var agendamento = await _agendamentoRepository.GetByIdAsync(command.Id, cancellationToken);
            if (agendamento == null)
            {
                return false;
            }

            agendamento.Ativo = false;

            await _agendamentoRepository.DeleteLogicAsync(agendamento, cancellationToken);
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
                Tipo = 2
            };

            var jsonContent = JsonConvert.SerializeObject(aviso);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await _httpClient.PostAsync(url, contentString);
        }
    }
}
