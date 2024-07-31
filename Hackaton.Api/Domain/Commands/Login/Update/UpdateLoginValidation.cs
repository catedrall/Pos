using FluentValidation;

namespace Hackaton.Api.Domain.Commands.Login.Update
{
    public class UpdateLoginValidation : AbstractValidator<UpdateLoginCommand>
    {
        public UpdateLoginValidation()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("O Id Login é obrigatório.");
        }
    }
}
