using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using System.Collections.Generic;
using Hackaton.Api.Domain.Commands.Paciente.Update;
using Hackaton.Api.Repository.Interface;
using Hackaton.Models;


namespace Hackaton.Api.Test.Paciente
{
    public class UpdatePacienteHandlerTests
{
    private readonly Mock<IPacienteRepository> _mockPacienteRepository;
    private readonly Mock<IValidator<UpdatePacienteCommand>> _mockValidator;
    private readonly UpdatePacienteHandler _handler;

    public UpdatePacienteHandlerTests()
    {
        _mockPacienteRepository = new Mock<IPacienteRepository>();
        _mockValidator = new Mock<IValidator<UpdatePacienteCommand>>();
        _handler = new UpdatePacienteHandler(_mockPacienteRepository.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalso_QuandoValidacaoFalhar()
    {
        // Arrange
        var command = new UpdatePacienteCommand();
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("Nome", "Nome inválido") }));

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
        _mockPacienteRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Models.Paciente>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveAtualizarPacienteCorretamente()
    {
        // Arrange
        var command = new UpdatePacienteCommand
        {
            Id = 1,
            Nome = "João Silva",
            Email = "joao@example.com",
            DataNascimento = new DateTime(1985, 3, 10)
        };
        var pacienteExistente = new Models.Paciente("Antônio Santos", "antonio@example.com", new DateTime(1990, 6, 20));
        pacienteExistente.Id = 1;
        var listaPacientes = new List<Models.Paciente> { pacienteExistente };
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPacienteRepository.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(listaPacientes);
        _mockPacienteRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Models.Paciente>(), It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
        _mockPacienteRepository.Verify(repo => repo.UpdateAsync(It.Is<Models.Paciente>(p => p.Id == command.Id && p.Nome == command.Nome && p.Email == command.Email && p.DataNascimento == command.DataNascimento), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalso_QuandoPacienteNaoEncontrado()
    {
        // Arrange
        var command = new UpdatePacienteCommand { Id = 999 }; // ID que não existe
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPacienteRepository.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((List<Models.Paciente>)null);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse();
        _mockPacienteRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Models.Paciente>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
}