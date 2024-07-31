using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Hackaton.Api.Domain.Queries.Medico.Get;
using Hackaton.Api.Repository.Interface;
using Hackaton.Models;
using System.Collections.Generic;


namespace Hackaton.Api.Test.Medico
{
    public class GetMedicoHandleTests
    {
        private readonly Mock<IMedicoRepository> _mockMedicoRepository;
        private readonly GetMedicoHandle _handler;

        public GetMedicoHandleTests()
        {
            _mockMedicoRepository = new Mock<IMedicoRepository>();
            _handler = new GetMedicoHandle(_mockMedicoRepository.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarMedicosQuandoEncontrados()
        {
            // Arrange
            var query = new GetMedicoQuery { Id = 1 };
            var medicos = new List<Models.Medico>
        {
            new Models.Medico { Id = 1, Nome = "Dr. João" },
            new Models.Medico { Id = 2, Nome = "Dra. Maria" }
        };
            _mockMedicoRepository.Setup(repo => repo.GetAsync(query.Id, query.CRM, query.Email, query.Ativo, query.DataNascimento, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(medicos);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Medicos.Should().NotBeNull().And.HaveCount(2);
            resultado.Medicos.Should().Contain(m => m.Nome == "Dr. João");
            resultado.Medicos.Should().Contain(m => m.Nome == "Dra. Maria");
        }

        [Fact]
        public async Task Handle_DeveRetornarMedicosVazioQuandoNaoEncontrados()
        {
            // Arrange
            var query = new GetMedicoQuery { Id = 999 }; // ID que não existe
            _mockMedicoRepository.Setup(repo => repo.GetAsync(query.Id, query.CRM, query.Email, query.Ativo, query.DataNascimento, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(new List<Models.Medico>());

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Medicos.Should().NotBeNull().And.BeEmpty();
        }
    }
}