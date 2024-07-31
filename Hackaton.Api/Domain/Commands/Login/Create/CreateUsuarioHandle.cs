using FluentValidation;
using Hackaton.Api.Repository.Interface;
using MediatR;
using System.Reflection;

namespace Hackaton.Api.Domain.Commands.Login.Create
{
    public class CreateUsuarioHandle : IRequestHandler<CreateUsuarioCommand, LoginResult?>
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IValidator<CreateUsuarioCommand> _validator;

        public CreateUsuarioHandle(ILoginRepository loginRepository, IValidator<CreateUsuarioCommand> validator)
        {
            _loginRepository = loginRepository;
            _validator = validator;
        }

        public async Task<LoginResult?> Handle(CreateUsuarioCommand command, CancellationToken cancellationToken = default)
        {
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
            LoginResult result = new LoginResult
            {
                Id = login.Id,
                Medico = login.Medico,
            };
            return result;
        }
    }
}
