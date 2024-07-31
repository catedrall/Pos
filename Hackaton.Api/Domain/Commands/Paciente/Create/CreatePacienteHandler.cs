using FluentValidation;
using Hackaton.Api.Repository.Interface;
using MediatR;
using System.Reflection;

namespace Hackaton.Api.Domain.Commands.Paciente.Create
{
    public class CreatePacienteHandler : IRequestHandler<CreatePacienteCommand, bool>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly IValidator<CreatePacienteCommand> _validator;

        public CreatePacienteHandler(IPacienteRepository pacienteRepository, IValidator<CreatePacienteCommand> validator, ILoginRepository loginRepository)
        {
            _pacienteRepository = pacienteRepository;
            _validator = validator;
            _loginRepository = loginRepository;
        }

        public async Task<bool> Handle(CreatePacienteCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return false;
                }

                var paciente = new Models.Paciente(command.Nome, command.Email, command.DataNascimento);

                await _pacienteRepository.CreateAsync(paciente, cancellationToken);

                var Login = new Models.Login
                {
                    Email = command.Email,
                    Senha = command.Senha,
                    Ativo = true,
                    DataCadastro = DateTime.Now,
                    DataUltimoLogin = DateTime.Now,
                    Medico = false
                };

                await _loginRepository.CreateAsync(Login, cancellationToken);

                await _loginRepository.SalvarAsync(cancellationToken);

                return true;
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }
    }
}
