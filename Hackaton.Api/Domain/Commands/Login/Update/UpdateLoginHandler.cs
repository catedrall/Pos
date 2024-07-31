using FluentValidation;
using Hackaton.Api.Repository.Interface;
using MediatR;

namespace Hackaton.Api.Domain.Commands.Login.Update
{
    public class UpdateLoginHandler : IRequestHandler<UpdateLoginCommand, bool?>
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IValidator<UpdateLoginCommand> _validator;

        public UpdateLoginHandler(ILoginRepository loginRepository, IValidator<UpdateLoginCommand> validator)
        {
            _loginRepository = loginRepository;
            _validator = validator;
        }

        public async Task<bool?> Handle(UpdateLoginCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var login = await _loginRepository.GetAsync(command.Id, cancellationToken);
            if (login is null)
                return false;

            if (command.Senha is not null)
                login.Senha = command.Senha;

            if (command.Ativo is not null)
                login.Ativo = command.Ativo.Value;

            if (command.Email is not null)
                login.Email = command.Email;

            await _loginRepository.UpdateAsync(login, cancellationToken);

            return true;
        }
    }
}
