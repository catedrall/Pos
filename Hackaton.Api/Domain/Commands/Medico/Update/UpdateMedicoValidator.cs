using FluentValidation;
using Hackaton.Api.Domain.Commands.Medico.Create;

namespace Hackaton.Api.Domain.Commands.Medico.Update
{
    public class UpdateMedicoValidator : AbstractValidator<UpdateMedicoCommand>
    {
        public UpdateMedicoValidator()
        {

            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("O campo Id é obrigatório.");

            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("O campo Email é obrigatório.");

            RuleFor(command => command.DataNascimento)
                .NotEmpty().WithMessage("O campo Data de nascimento é obrigatória.");

            RuleFor(command => command.Nome)
                .NotEmpty().WithMessage("O campo Nome é obrigatória.");


            RuleFor(command => command.CRM)
                .NotEmpty().WithMessage("O campo CRM é obrigatória.");

            RuleFor(command => command.Especialidade)
                .NotEmpty().WithMessage("O campo Especialidade é obrigatório.");

        }
    }
}
