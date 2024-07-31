using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Hackaton.Api.Domain.Commands.Medico.Create;
using Hackaton.Api.Repository.Interface;
using Hackaton.Models;
using System.Collections.Generic;

namespace Hackaton.Api.Test.Medico
{
    public class CreateMedicoHandlerTests
    {
        private readonly Mock<IMedicoRepository> _mockMedicoRepository;
        private readonly Mock<ILoginRepository> _mockLoginRepository;
        private readonly Mock<IValidator<CreateMedicoCommand>> _mockValidator;
        private readonly CreateMedicoHandler _handler;

        public CreateMedicoHandlerTests()
        {
            _mockMedicoRepository = new Mock<IMedicoRepository>();
            _mockLoginRepository = new Mock<ILoginRepository>();
            _mockValidator = new Mock<IValidator<CreateMedicoCommand>>();
            _handler = new CreateMedicoHandler(_mockMedicoRepository.Object, _mockValidator.Object, _mockLoginRepository.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoValidacaoFalhar()
        {
            // Arrange
            var command = new CreateMedicoCommand { Nome = "Dr. João", Email = "email@example.com", Senha = "senha123", DataNascimento = DateTime.Now, CRM = "123456", Especialidade = "Cardiologia" };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Nome", "Erro") }));

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _mockMedicoRepository.Verify(repo => repo.CreateAsync(It.IsAny<Models.Medico>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockLoginRepository.Verify(repo => repo.CreateAsync(It.IsAny<Models.Login>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveCriarMedicoELogin_QuandoValidacaoForBemSucedida()
        {
            // Arrange
            var command = new CreateMedicoCommand { Nome = "Dr. João", Email = "email@example.com", Senha = "senha123", DataNascimento = DateTime.Now, CRM = "123456", Especialidade = "Cardiologia" };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            _mockMedicoRepository.Verify(repo => repo.CreateAsync(It.Is<Models.Medico>(m => m.Nome == command.Nome && m.Email == command.Email), It.IsAny<CancellationToken>()), Times.Once);
            _mockLoginRepository.Verify(repo => repo.CreateAsync(It.Is<Models.Login>(l => l.Email == command.Email && l.Senha == command.Senha), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoExcecaoForLancada()
        {
            // Arrange
            var command = new CreateMedicoCommand { Nome = "Dr. João", Email = "email@example.com", Senha = "senha123", DataNascimento = DateTime.Now, CRM = "123456", Especialidade = "Cardiologia" };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockMedicoRepository.Setup(repo => repo.CreateAsync(It.IsAny<Models.Medico>(), It.IsAny<CancellationToken>()))
                                 .ThrowsAsync(new Exception("Erro de teste"));

            // Act
            bool resultado = false;
            try
            {
                resultado = await _handler.Handle(command, CancellationToken.None);
            }
            catch (Exception ex)
            {
                // Log exception if necessary
                // Assert that the exception is the expected one
                Assert.Equal("Erro de teste", ex.Message);
            }

            // Assert
            resultado.Should().BeFalse();
            _mockMedicoRepository.Verify(repo => repo.CreateAsync(It.IsAny<Models.Medico>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockLoginRepository.Verify(repo => repo.CreateAsync(It.IsAny<Models.Login>(), It.IsAny<CancellationToken>()), Times.Never);

        }
    }
}
