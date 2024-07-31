using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Hackaton.Api.Domain.Queries.Paciente.Get;
using Hackaton.Api.Repository.Interface;
using Hackaton.Models;
using System.Collections.Generic;

namespace Hackaton.Api.Test.Paciente
{
    public class GetPacienteHandleTests
    {
        private readonly Mock<IPacienteRepository> _mockPacienteRepository;
        private readonly GetPacienteHandle _handler;

        public GetPacienteHandleTests()
        {
            _mockPacienteRepository = new Mock<IPacienteRepository>();
            _handler = new GetPacienteHandle(_mockPacienteRepository.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarPaciente_QuandoEncontrado()
        {
            // Arrange
            var query = new GetPacienteQuery { Id = 1 };
            var pacienteRetornado = new Models.Paciente("João Silva", "joao@example.com", new DateTime(1985, 3, 10));
            pacienteRetornado.Id = 1;
            var listaPacientes = new List<Models.Paciente> { pacienteRetornado };
            _mockPacienteRepository.Setup(repo => repo.GetByIdAsync(query.Id.Value, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(listaPacientes);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Pacientes.Should().ContainSingle();
            resultado.Pacientes.Should().ContainEquivalentOf(pacienteRetornado);
        }

        [Fact]
        public async Task Handle_DeveRetornarNull_QuandoNaoEncontrado()
        {
            // Arrange
            var query = new GetPacienteQuery { Id = 999 }; // ID que não existe
            _mockPacienteRepository.Setup(repo => repo.GetByIdAsync(query.Id.Value, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync((List<Models.Paciente>)null);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(resultado.Pacientes.Count(), 0);
        }
    }
}