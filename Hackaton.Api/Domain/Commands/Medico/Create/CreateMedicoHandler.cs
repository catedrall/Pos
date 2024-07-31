using FluentValidation;
using Hackaton.Api.Domain.Commands.Paciente.Create;
using Hackaton.Api.Repository;
using Hackaton.Api.Repository.Interface;
using MediatR;
using System.Reflection;

namespace Hackaton.Api.Domain.Commands.Medico.Create
{
    public class CreateMedicoHandler : IRequestHandler<CreateMedicoCommand, bool>
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly IValidator<CreateMedicoCommand> _validator;

        public CreateMedicoHandler(IMedicoRepository medicoRepository, IValidator<CreateMedicoCommand> validator, ILoginRepository loginRepository)
        {
            _medicoRepository = medicoRepository;
            _validator = validator;
            _loginRepository = loginRepository;
        }

        public async Task<bool> Handle(CreateMedicoCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var medico = new Models.Medico(command.Nome, command.Email, command.DataNascimento, command.CRM, command.Especialidade);

            await _medicoRepository.CreateAsync(medico, cancellationToken);

            var Login = new Models.Login
            {
                Email = command.Email,
                Senha = command.Senha,
                Ativo = true,
                DataCadastro = DateTime.Now,
                DataUltimoLogin = DateTime.Now,
                Medico = true
            };

            await _loginRepository.CreateAsync(Login, cancellationToken);
            await _loginRepository.SalvarAsync(cancellationToken);

            return true;
        }
    }
}
