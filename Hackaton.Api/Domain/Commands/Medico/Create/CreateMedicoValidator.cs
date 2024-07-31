using FluentValidation;
using Hackaton.Api.Domain.Commands.Paciente.Create;
using System.Security.Cryptography;

namespace Hackaton.Api.Domain.Commands.Medico.Create
{
    public class CreateMedicoValidator : AbstractValidator<CreateMedicoCommand>
    {
        public CreateMedicoValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("O campo Email é obrigatório.");

            RuleFor(command => command.DataNascimento)
                .NotEmpty().WithMessage("O campo Data de nascimento é obrigatória.");

            RuleFor(command => command.Senha)
                .NotEmpty().WithMessage("O campo Senha é obrigatório.");

            RuleFor(command => command.Nome)
                .NotEmpty().WithMessage("O campo Nome é obrigatória.");


            RuleFor(command => command.CRM)
                .NotEmpty().WithMessage("O campo CRM é obrigatória.");

            RuleFor(command => command.Especialidade)
                .NotEmpty().WithMessage("O campo Especialidade é obrigatório.");

        }
    }
}
