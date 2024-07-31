using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Hackaton.Api.Domain.Commands.Paciente.Create;
using Hackaton.Api.Repository.Interface;
using Hackaton.Models;


namespace Hackaton.Api.Test.Paciente
{
    public class CreatePacienteHandlerTests
    {
        private readonly Mock<IPacienteRepository> _mockPacienteRepository;
        private readonly Mock<ILoginRepository> _mockLoginRepository;
        private readonly Mock<IValidator<CreatePacienteCommand>> _mockValidator;
        private readonly CreatePacienteHandler _handler;

        public CreatePacienteHandlerTests()
        {
            _mockPacienteRepository = new Mock<IPacienteRepository>();
            _mockLoginRepository = new Mock<ILoginRepository>();
            _mockValidator = new Mock<IValidator<CreatePacienteCommand>>();
            _handler = new CreatePacienteHandler(_mockPacienteRepository.Object, _mockValidator.Object, _mockLoginRepository.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoValidacaoFalhar()
        {
            // Arrange
            var command = new CreatePacienteCommand();
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Nome", "Nome inválido") }));

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mockPacienteRepository.Verify(repo => repo.CreateAsync(It.IsAny<Models.Paciente>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockLoginRepository.Verify(repo => repo.CreateAsync(It.IsAny<Models.Login>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveCriarPacienteELoginCorretamente()
        {
            // Arrange
            var command = new CreatePacienteCommand
            {
                Nome = "Maria Silva",
                Email = "maria@example.com",
                DataNascimento = new DateTime(1990, 5, 15),
                Senha = "senha123"
            };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _mockPacienteRepository.Setup(repo => repo.CreateAsync(It.IsAny<Models.Paciente>(), It.IsAny<CancellationToken>()))
                                   .Returns(Task.CompletedTask);
            _mockLoginRepository.Setup(repo => repo.CreateAsync(It.IsAny<Models.Login>(), It.IsAny<CancellationToken>()))
                                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            _mockPacienteRepository.Verify(repo => repo.CreateAsync(It.IsAny<Models.Paciente>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockLoginRepository.Verify(repo => repo.CreateAsync(It.Is<Models.Login>(l => l.Email == command.Email && l.Senha == command.Senha), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
