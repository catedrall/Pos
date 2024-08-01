using FluentValidation;
using Hackaton.Api.Repository.Interface;
using Hackaton.Models;
using MediatR;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

namespace Hackaton.Api.Domain.Commands.Consulta.Create
{
    public class CreateAgendaHandle : IRequestHandler<CreateConsultaCommand, bool>
    {
        private readonly IAgendaRepository _agendamentoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IValidator<CreateConsultaCommand> _validator;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CreateAgendaHandle(IAgendaRepository agendamentoRepository, IValidator<CreateConsultaCommand> validator, HttpClient httpClient, IConfiguration configuration, IPacienteRepository pacienteRepository)
        {
            _agendamentoRepository = agendamentoRepository;
            _validator = validator;
            _httpClient = httpClient;
            _configuration = configuration;
            _pacienteRepository = pacienteRepository;
        }

        public async Task<bool> Handle(CreateConsultaCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var agendamento = new Models.Consulta(command.IdPaciente, command.IdMedico, command.DataAgendamento);

            await _agendamentoRepository.CreateAsync(agendamento, cancellationToken);
            await EnviaEmai(command.IdPaciente, command.DataAgendamento);

            return true;
        }

        private async Task EnviaEmai(int idPaciente, DateTime dataConsulta)
        {
            try
            {
                string url = _configuration.GetSection("Aviso:EndPoint").Value;
                var result = await _pacienteRepository.GetByIdAsync(idPaciente);
                var paciente = result.FirstOrDefault();

                Aviso aviso = new Aviso
                {
                    Data = dataConsulta,
                    Email = paciente.Email,
                    Nome = paciente.Nome,
                    Tipo = 1
                };

                var jsonContent = JsonConvert.SerializeObject(aviso);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await _httpClient.PostAsync(url, contentString);
            }
            catch (Exception e)
            {

                throw;
            }
        }
            
    }
}
