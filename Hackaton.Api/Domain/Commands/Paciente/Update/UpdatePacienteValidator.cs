using FluentValidation;
using Hackaton.Api.Domain.Commands.Paciente.Create;
using MediatR;

namespace Hackaton.Api.Domain.Commands.Paciente.Update
{
    public class UpdatePacienteValidator : AbstractValidator<UpdatePacienteCommand>
    {
        public UpdatePacienteValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("O campo Id é obrigatório.");
        }
    }
}
