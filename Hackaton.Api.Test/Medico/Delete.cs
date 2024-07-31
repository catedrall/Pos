using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Hackaton.Api.Domain.Commands.Medico.Delete;
using Hackaton.Api.Repository.Interface;
using Hackaton.Models;


namespace Hackaton.Api.Test.Medico
{
    public class DeleteMedicoHandlerTests
    {
        private readonly Mock<IMedicoRepository> _mockMedicoRepository;
        private readonly Mock<IValidator<DeleteMedicoCommand>> _mockValidator;
        private readonly DeleteMedicoHandler _handler;

        public DeleteMedicoHandlerTests()
        {
            _mockMedicoRepository = new Mock<IMedicoRepository>();
            _mockValidator = new Mock<IValidator<DeleteMedicoCommand>>();
            _handler = new DeleteMedicoHandler(_mockMedicoRepository.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoValidacaoFalhar()
        {
            // Arrange
            var command = new DeleteMedicoCommand { Id = 1 };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Médico não encontrado") }));

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mockMedicoRepository.Verify(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Never);
            _mockMedicoRepository.Verify(repo => repo.DeleteLogicAsync(It.IsAny<Models.Medico>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoMedicoNaoExistir()
        {
            // Arrange
            var command = new DeleteMedicoCommand { Id = 1 };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockMedicoRepository.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((Models.Medico)null);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mockMedicoRepository.Verify(repo => repo.DeleteLogicAsync(It.IsAny<Models.Medico>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveMarcarMedicoComoInativoEChamarDeleteLogicAsync()
        {
            // Arrange
            var medico = new Models.Medico { Id = 1, Nome = "Dr. José", Ativo = true };
            var command = new DeleteMedicoCommand { Id = 1 };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockMedicoRepository.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(medico);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            medico.Ativo.Should().BeFalse();
            _mockMedicoRepository.Verify(repo => repo.DeleteLogicAsync(medico, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}