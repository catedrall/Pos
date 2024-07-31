using FluentValidation;
using Hackaton.Api.Domain.Commands.Consulta.Create;

namespace Hackaton.Api.Domain.Commands.Consulta.Update
{
    public class UpdateAgendaValidation : AbstractValidator<UpdateConsultaCommand>
    {
        public UpdateAgendaValidation()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("O agendamento é obrigatório.");
            RuleFor(command => command.NovaDataAgendamento)
                .NotEmpty().WithMessage("O campo Nova Data do agendamento é obrigatória.");
        }
    }
}
