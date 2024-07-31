using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Hackaton.Api.Repository.Interface;
using Hackaton.Api.Domain.Commands.Consulta.Create;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace Hackaton.Api.Test.Agenda
{
    public class CreateAgendaHandleTests
    {
        private readonly Mock<IAgendaRepository> _mockRepositorioAgenda;
        private readonly Mock<IValidator<CreateConsultaCommand>> _mockValidador;
        private readonly CreateAgendaHandle _manipulador;
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IPacienteRepository> _mockPacienteRepository;

        public CreateAgendaHandleTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                    {"Aviso:EndPoint", "https://avisopostec20240724012435.azurewebsites.net/api/Aviso?code=I2cn4oIbkD5WZlVRnad05ita8g2WyDMsWcsTyC_h5AIEAzFupn2N0g%3D%3D"},
                };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            _mockRepositorioAgenda = new Mock<IAgendaRepository>();
            _mockValidador = new Mock<IValidator<CreateConsultaCommand>>();
            _mockHttpClient = new Mock<HttpClient>();
            _mockPacienteRepository = new Mock<IPacienteRepository>();
            _manipulador = new CreateAgendaHandle(_mockRepositorioAgenda.Object, _mockValidador.Object, _mockHttpClient.Object, configuration, _mockPacienteRepository.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoValidacaoFalhar()
        {
            // Arrange
            var paciente = new Models.Paciente("Fulano", "email.@email.com", DateTime.Now);
            List<Models.Paciente> lista = new List<Models.Paciente>();
            lista.Add(paciente);

            _mockPacienteRepository.Setup(x => x.GetByIdAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>())).ReturnsAsync(lista);

            var comando = new CreateConsultaCommand { IdPaciente = 1, IdMedico = 1, DataAgendamento = DateTime.Now };
            _mockValidador.Setup(v => v.ValidateAsync(comando, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("IdPaciente", "Erro") }));

            // Act
            var resultado = await _manipulador.Handle(comando);

            // Assert
            resultado.Should().BeFalse();
            _mockRepositorioAgenda.Verify(repo => repo.CreateAsync(It.IsAny<Models.Consulta>(), It.IsAny<CancellationToken>()), Times.Never); ;
        }

        [Fact]
        public async Task Handle_DeveRetornarVerdadeiro_QuandoValidacaoForBemSucedida()
        {
            // Arrange
            var paciente = new Models.Paciente("Fulano", "email.@email.com", DateTime.Now);
            List<Models.Paciente> lista = new List<Models.Paciente>();
            lista.Add(paciente);

            _mockPacienteRepository.Setup(x => x.GetByIdAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>())).ReturnsAsync(lista);

            var comando = new CreateConsultaCommand { IdPaciente = 1, IdMedico = 1, DataAgendamento = DateTime.Now };
            _mockValidador.Setup(v => v.ValidateAsync(comando, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockRepositorioAgenda.Setup(repo => repo.CreateAsync(It.IsAny<Models.Consulta>(), It.IsAny<CancellationToken>()))
                                 .Returns(Task.CompletedTask);

            // Act
            var resultado = await _manipulador.Handle(comando);

            // Assert
            resultado.Should().BeTrue();
            _mockRepositorioAgenda.Verify(repo => repo.CreateAsync(It.IsAny<Models.Consulta>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
