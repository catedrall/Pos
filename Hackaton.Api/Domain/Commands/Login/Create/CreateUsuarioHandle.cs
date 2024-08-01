using FluentValidation;
using Hackaton.Api.Repository;
using Hackaton.Api.Repository.Interface;
using MediatR;
using System.Reflection;

namespace Hackaton.Api.Domain.Commands.Login.Create
{
    public class CreateUsuarioHandle : IRequestHandler<CreateUsuarioCommand, LoginResult?>
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IMedicoRepository _medicoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IValidator<CreateUsuarioCommand> _validator;

        public CreateUsuarioHandle(ILoginRepository loginRepository, IValidator<CreateUsuarioCommand> validator, IMedicoRepository medicoRepository, IPacienteRepository pacienteRepository)
        {
            _loginRepository = loginRepository;
            _medicoRepository = medicoRepository;
            _validator = validator;
            _pacienteRepository = pacienteRepository;
        }

        public async Task<LoginResult?> Handle(CreateUsuarioCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                int medicoId = 0;
                LoginResult result = new LoginResult();
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return null;
                }

                var login = await _loginRepository.LoginAsync(command.Email, command.Senha, cancellationToken);


                if (login == null)
                {
                    return null;
                }
                else
                {
                    if (login.Medico)
                    {
                        var medicos = await _medicoRepository.GetAsync(null, null, login.Email, null, null);
                        medicoId = medicos.FirstOrDefault().Id;
                        result.Id = medicoId;
                    }
                    else
                    {
                        var paciente = await _pacienteRepository.GetByEmailAsync(command.Email);
                        result.Id = paciente.FirstOrDefault().Id;
                    }
                    result.Medico = login.Medico;
                    return result;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }
    }
}
