using FluentValidation;

namespace Hackaton.Api.Domain.Commands.Consulta.Create
{
    public class CreateAgendaValidator : AbstractValidator<CreateConsultaCommand>
    {
        public CreateAgendaValidator()
        {
            RuleFor(command => command.IdMedico)
                .NotEmpty().WithMessage("O campo Médico é obrigatório.");

            RuleFor(command => command.IdPaciente)
                .NotEmpty().WithMessage("O campo Paciente é obrigatória.");

            RuleFor(command => command.DataAgendamento)
                .NotEmpty().WithMessage("O campo Data do agendamento é obrigatória.");
        }
    }
}
