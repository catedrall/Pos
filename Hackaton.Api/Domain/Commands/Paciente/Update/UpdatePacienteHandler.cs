using FluentValidation;
using Hackaton.Api.Repository.Interface;
using MediatR;

namespace Hackaton.Api.Domain.Commands.Paciente.Update
{
    public class UpdatePacienteHandler : IRequestHandler<UpdatePacienteCommand, bool>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IValidator<UpdatePacienteCommand> _validator;

        public UpdatePacienteHandler(IPacienteRepository pacienteRepository, IValidator<UpdatePacienteCommand> validator)
        {
            _pacienteRepository = pacienteRepository;
            _validator = validator;
        }

        public async Task<bool> Handle(UpdatePacienteCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var pacientes = await _pacienteRepository.GetByIdAsync(command.Id, cancellationToken);
            if (pacientes is null)
            {
                return false;
            }

            var paciente = pacientes.FirstOrDefault();

            paciente.Nome = command.Nome;
            paciente.Email = command.Email;
            paciente.DataNascimento = command.DataNascimento;

            await _pacienteRepository.UpdateAsync(paciente, cancellationToken);

            return true;
        }
    }
}
