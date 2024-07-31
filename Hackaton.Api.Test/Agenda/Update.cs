using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Hackaton.Api.Repository.Interface;
using Hackaton.Api.Domain.Commands.Consulta.Update;
using Microsoft.Extensions.Configuration;
using Castle.Core.Resource;

namespace Hackaton.Api.Test.Agenda
{
    public class UpdateAgendaHandlerTests
    {
        private readonly Mock<IMedicoRepository> _mockRepositorioMedico;
        private readonly Mock<IPacienteRepository> _mockRepositorioPaciente;
        private readonly Mock<IAgendaRepository> _mockRepositorioAgenda;
        private readonly Mock<IValidator<UpdateConsultaCommand>> _mockValidador;
        private readonly Mock<IPacienteRepository> _mockPacienteRepository;
        private readonly UpdateAgendaHandler _manipulador;
        private readonly Mock<HttpClient> _mockHttpClient;

        public UpdateAgendaHandlerTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                    {"Aviso:EndPoint", "https://avisopostec20240724012435.azurewebsites.net/api/Aviso?code=I2cn4oIbkD5WZlVRnad05ita8g2WyDMsWcsTyC_h5AIEAzFupn2N0g%3D%3D"},
                };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            _mockRepositorioMedico = new Mock<IMedicoRepository>();
            _mockRepositorioPaciente = new Mock<IPacienteRepository>();
            _mockRepositorioAgenda = new Mock<IAgendaRepository>();
            _mockValidador = new Mock<IValidator<UpdateConsultaCommand>>();
            _mockHttpClient = new Mock<HttpClient>();
            _manipulador = new UpdateAgendaHandler(_mockRepositorioMedico.Object, _mockRepositorioAgenda.Object, _mockRepositorioPaciente.Object, _mockValidador.Object, _mockHttpClient.Object, configuration);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoValidacaoFalhar()
        {
            // Arrange
            var comando = new UpdateConsultaCommand { Id = 1, NovaDataAgendamento = DateTime.Now.AddDays(1) };
            _mockValidador.Setup(v => v.ValidateAsync(comando, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Erro") }));

            // Act
            var resultado = await _manipulador.Handle(comando);

            // Assert
            resultado.Should().BeFalse();
            _mockRepositorioAgenda.Verify(repo => repo.GetAgendamentoAsync(It.IsAny<int>(), null, null, null, It.IsAny<CancellationToken>()), Times.Never);
            _mockRepositorioAgenda.Verify(repo => repo.UpdateAsync(It.IsAny<Models.Consulta>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoAgendamentoNaoForEncontrado()
        {
            // Arrange
            var comando = new UpdateConsultaCommand { Id = 1, NovaDataAgendamento = DateTime.Now.AddDays(1) };
            var paciente = new Models.Paciente("Fulano", "email.@email.com", comando.NovaDataAgendamento);
            List<Models.Paciente> lista = new List<Models.Paciente>();
            lista.Add(paciente);


            _mockRepositorioPaciente.Setup(x => x.GetByIdAsync(It.IsAny<int?>(),It.IsAny<CancellationToken>())).ReturnsAsync(lista);
            _mockValidador.Setup(v => v.ValidateAsync(comando, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockRepositorioAgenda.Setup(repo => repo.GetAgendamentoAsync(comando.Id, null, null, null, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync((Models.Consulta)null);

            // Act
            var resultado = await _manipulador.Handle(comando);

            // Assert
            resultado.Should().BeFalse();
            _mockRepositorioAgenda.Verify(repo => repo.GetAgendamentoAsync(comando.Id, null, null, null, It.IsAny<CancellationToken>()), Times.Once);
            _mockRepositorioAgenda.Verify(repo => repo.UpdateAsync(It.IsAny<Models.Consulta>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveRetornarVerdadeiro_QuandoAgendamentoForEncontradoEAtualizado()
        {
            // Arrange
            var data = DateTime.Now;
            var outraData = new DateTime(data.Year, data.Month, data.Day, 15, 0, 0);
            var comando = new UpdateConsultaCommand { Id = 1, NovaDataAgendamento = data, Hora=15 };
            var agendamento = new Models.Consulta(1, 1, DateTime.Now) { Id = comando.Id };
            var paciente = new Models.Paciente("Fulano", "email.@email.com", comando.NovaDataAgendamento);
            List<Models.Paciente> lista = new List<Models.Paciente>();
            lista.Add(paciente);

            _mockRepositorioPaciente.Setup(x => x.GetByIdAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>())).ReturnsAsync(lista);
            _mockValidador.Setup(v => v.ValidateAsync(comando, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockRepositorioAgenda.Setup(repo => repo.GetAgendamentoAsync(comando.Id, null, null, null, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(agendamento);
            _mockRepositorioAgenda.Setup(repo => repo.UpdateAsync(agendamento, It.IsAny<CancellationToken>()))
                                  .Returns(Task.CompletedTask);

            // Act
            var resultado = await _manipulador.Handle(comando);

            // Assert
            resultado.Should().BeTrue();
            agendamento.DataAgendamento.Should().Be(outraData);
            _mockRepositorioAgenda.Verify(repo => repo.GetAgendamentoAsync(comando.Id, null, null, null, It.IsAny<CancellationToken>()), Times.Once);
            _mockRepositorioAgenda.Verify(repo => repo.UpdateAsync(agendamento, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}