using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Hackaton.Api.Domain.Commands.Medico.Update;
using Hackaton.Api.Repository.Interface;


namespace Hackaton.Api.Test.Medico
{
    public class UpdateMedicoHandleTests
    {
        private readonly Mock<IMedicoRepository> _mockMedicoRepository;
        private readonly Mock<IValidator<UpdateMedicoCommand>> _mockValidator;
        private readonly UpdateMedicoHandle _handler;

        public UpdateMedicoHandleTests()
        {
            _mockMedicoRepository = new Mock<IMedicoRepository>();
            _mockValidator = new Mock<IValidator<UpdateMedicoCommand>>();
            _handler = new UpdateMedicoHandle(_mockMedicoRepository.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoValidacaoFalhar()
        {
            // Arrange
            var command = new UpdateMedicoCommand { Id = 1 };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Nome", "Nome inválido") }));

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mockMedicoRepository.Verify(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Never);
            _mockMedicoRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Models.Medico>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoMedicoNaoExistir()
        {
            // Arrange
            var command = new UpdateMedicoCommand { Id = 1 };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockMedicoRepository.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync((Models.Medico)null);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mockMedicoRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Models.Medico>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveAtualizarMedicoCorretamente()
        {
            // Arrange
            var command = new UpdateMedicoCommand
            {
                Id = 1,
                Nome = "Dr. João",
                Email = "joao@example.com",
                DataNascimento = DateTime.Now,
                CRM = "123456",
                Especialidade = "Cardiologia"
            };
            var medico = new Models.Medico
            {
                Id = command.Id,
                Nome = "Antônio",
                Email = "antonio@example.com",
                DataNascimento = DateTime.Now.AddYears(-30),
                CRM = "654321",
                Especialidade = "Pediatria"
            };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockMedicoRepository.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(medico);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            medico.Nome.Should().Be(command.Nome);
            medico.Email.Should().Be(command.Email);
            medico.DataNascimento.Should().Be(command.DataNascimento);
            medico.CRM.Should().Be(command.CRM);
            medico.Especialidade.Should().Be(command.Especialidade);
            _mockMedicoRepository.Verify(repo => repo.UpdateAsync(medico, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}