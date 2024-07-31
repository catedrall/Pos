using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Hackaton.Api.Domain.Commands.Login.Create;
using Hackaton.Api.Repository.Interface;


namespace Hackaton.Api.Test.Login
{
    public class CreateUsuarioHandleTests
    {
        private readonly Mock<ILoginRepository> _mockLoginRepository;
        private readonly Mock<IValidator<CreateUsuarioCommand>> _mockValidator;
        private readonly CreateUsuarioHandle _handler;

        public CreateUsuarioHandleTests()
        {
            _mockLoginRepository = new Mock<ILoginRepository>();
            _mockValidator = new Mock<IValidator<CreateUsuarioCommand>>();
            _handler = new CreateUsuarioHandle(_mockLoginRepository.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalso_QuandoValidacaoFalhar()
        {
            // Arrange
            var command = new CreateUsuarioCommand { Email = "email@example.com", Senha = "senha123" };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Email", "Erro") }));

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            bool resultadoBool = resultado == null;
            resultadoBool.Should().BeTrue();
            _mockLoginRepository.Verify(repo => repo.LoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveRetornarVerdadeiro_QuandoLoginForBemSucedido()
        {
            // Arrange
            var command = new CreateUsuarioCommand { Email = "email@example.com", Senha = "senha123" };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockLoginRepository.Setup(repo => repo.LoginAsync(command.Email, command.Senha, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(new Models.Login { Id = 1 });

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            bool resultadoBool = resultado.Id > 0 && resultado.Medico == false;
            resultadoBool.Should().BeTrue();
            _mockLoginRepository.Verify(repo => repo.LoginAsync(command.Email, command.Senha, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarNulo_QuandoLoginForMalSucedido()
        {
            // Arrange
            var command = new CreateUsuarioCommand { Email = "email@example.com", Senha = "senha123" };
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _mockLoginRepository.Setup(repo => repo.LoginAsync(command.Email, command.Senha, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Models.Login)null);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            bool resultadoBool = resultado == null;
            resultadoBool.Should().BeTrue();
            _mockLoginRepository.Verify(repo => repo.LoginAsync(command.Email, command.Senha, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}