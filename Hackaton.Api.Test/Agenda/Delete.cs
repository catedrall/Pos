using Moq;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Hackaton.Api.Repository.Interface;
using Hackaton.Api.Domain.Commands.Consulta.Delete;
using Microsoft.Extensions.Configuration;
using Hackaton.Api.Domain.Commands.Consulta.Update;

namespace Hackaton.Api.Test.Agenda
{

    
    public class DeleteAgendamentoHandleTests
    {
        
        private readonly Mock<IAgendaRepository> _mockRepositorioAgenda;
        private readonly Mock<IPacienteRepository> _mockPacienteRepository;
        private readonly Mock<IValidator<DeleteAgendamentoCommand>> _mockValidador;
        private readonly DeleteAgendamentoHandle _manipulador;
        private readonly Mock<HttpClient> _mockHttpClient;



        public DeleteAgendamentoHandleTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                    {"Aviso:EndPoint", "https://avisopostec20240724012435.azurewebsites.net/api/Aviso?code=I2cn4oIbkD5WZlVRnad05ita8g2WyDMsWcsTyC_h5AIEAzFupn2N0g%3D%3D"},
                };

            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            _mockRepositorioAgenda = new Mock<IAgendaRepository>();
            _mockValidador = new Mock<IValidator<DeleteAgendamentoCommand>>();
            _mockHttpClient = new Mock<HttpClient>();
            _mockPacienteRepository = new Mock<IPacienteRepository>();
            _manipulador = new DeleteAgendamentoHandle(_mockRepositorioAgenda.Object, _mockValidador.Object, _mockHttpClient.Object, configuration, _mockPacienteRepository.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoValidacaoFalhar()
        {
            // Arrange
            var comando = new DeleteAgendamentoCommand { Id = 1 };
            _mockValidador.Setup(v => v.ValidateAsync(comando, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Erro") }));

            // Act
            var resultado = await _manipulador.Handle(comando);

            // Assert
            resultado.Should().BeFalse();
            _mockRepositorioAgenda.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockRepositorioAgenda.Verify(repo => repo.DeleteLogicAsync(It.IsAny<Models.Consulta>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoAgendamentoNaoForEncontrado()
        {
            // Arrange
            var comando = new DeleteAgendamentoCommand { Id = 1 };
            _mockValidador.Setup(v => v.ValidateAsync(comando, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockRepositorioAgenda.Setup(repo => repo.GetByIdAsync(comando.Id, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync((Models.Consulta)null);

            // Act
            var resultado = await _manipulador.Handle(comando);

            // Assert
            resultado.Should().BeFalse();
            _mockRepositorioAgenda.Verify(repo => repo.GetByIdAsync(comando.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mockRepositorioAgenda.Verify(repo => repo.DeleteLogicAsync(It.IsAny<Models.Consulta>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveRetornarVerdadeiro_QuandoAgendamentoForEncontradoEDeletado()
        {
            // Arrange
            var comando = new DeleteAgendamentoCommand { Id = 1 };
            var agendamento = new Models.Consulta(1, 1, DateTime.Now) { Id = comando.Id, Ativo = true };
            var paciente = new Models.Paciente("Fulano", "email.@email.com", DateTime.Now);
            List<Models.Paciente> lista = new List<Models.Paciente>();
            lista.Add(paciente);

            _mockPacienteRepository.Setup(x => x.GetByIdAsync(It.IsAny<int?>(), It.IsAny<CancellationToken>())).ReturnsAsync(lista);
            _mockValidador.Setup(v => v.ValidateAsync(comando, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockRepositorioAgenda.Setup(repo => repo.GetByIdAsync(comando.Id, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(agendamento);
            _mockRepositorioAgenda.Setup(repo => repo.DeleteLogicAsync(agendamento, It.IsAny<CancellationToken>()))
                                  .Returns(Task.CompletedTask);

            // Act
            var resultado = await _manipulador.Handle(comando);

            // Assert
            resultado.Should().BeTrue();
            agendamento.Ativo.Should().BeFalse();
            _mockRepositorioAgenda.Verify(repo => repo.GetByIdAsync(comando.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mockRepositorioAgenda.Verify(repo => repo.DeleteLogicAsync(agendamento, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}