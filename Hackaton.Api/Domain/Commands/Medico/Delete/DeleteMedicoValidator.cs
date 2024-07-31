using FluentValidation;

namespace Hackaton.Api.Domain.Commands.Medico.Delete
{
    public class DeleteMedicoValidator : AbstractValidator<DeleteMedicoCommand>
    {
        public DeleteMedicoValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("O Código é obrigatório.");

        }
    }
}
