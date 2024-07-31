using FluentValidation;

namespace Hackaton.Api.Domain.Commands.Login.Create
{
    public class CreateUsuarioValidator : AbstractValidator<CreateUsuarioCommand>
    {
        public CreateUsuarioValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("O campo Email é obrigatório.");

            RuleFor(command => command.Senha)
                .NotEmpty().WithMessage("O campo Senha é obrigatória.");
        }
    }
}
