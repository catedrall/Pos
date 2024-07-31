using FluentValidation;
using Hackaton.Api.Domain.Commands.Medico.Delete;

namespace Hackaton.Api.Domain.Commands.Consulta.Delete
{
    public class DeleteAgendamentoValidator : AbstractValidator<DeleteAgendamentoCommand>
    {
        public DeleteAgendamentoValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("O Código é obrigatório.");

        }
    }
}
