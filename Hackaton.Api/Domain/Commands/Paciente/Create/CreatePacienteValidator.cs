using FluentValidation;
using Hackaton.Api.Domain.Commands.Login.Create;

namespace Hackaton.Api.Domain.Commands.Paciente.Create
{
    public class CreatePacienteValidator : AbstractValidator<CreatePacienteCommand>
    {
        public CreatePacienteValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("O campo Email é obrigatório.");

            RuleFor(command => command.DataNascimento)
                .NotEmpty().WithMessage("O campo Data de nascimento é obrigatória.");

            RuleFor(command => command.Senha)
                .NotEmpty().WithMessage("O campo Senha é obrigatório.");

            RuleFor(command => command.Nome)
                .NotEmpty().WithMessage("O campo Nome é obrigatória.");
        }
    }
}
