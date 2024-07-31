using FluentValidation;
using Hackaton.Api.Domain.Commands.Paciente.Update;
using Hackaton.Api.Repository.Interface;
using MediatR;
using System.Security.Cryptography;

namespace Hackaton.Api.Domain.Commands.Medico.Update
{
    public class UpdateMedicoHandle : IRequestHandler<UpdateMedicoCommand, bool>
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IValidator<UpdateMedicoCommand> _validator;

        public UpdateMedicoHandle(IMedicoRepository medicoRepository, IValidator<UpdateMedicoCommand> validator)
        {
            _medicoRepository = medicoRepository;
            _validator = validator;
        }

        public async Task<bool> Handle(UpdateMedicoCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var medico = await _medicoRepository.GetByIdAsync(command.Id, cancellationToken);
            if (medico == null)
            {
                return false;
            }

            medico.Nome = command.Nome;
            medico.Email = command.Email;
            medico.DataNascimento = command.DataNascimento;
            medico.CRM = command.CRM;
            medico.Especialidade = command.Especialidade;

            await _medicoRepository.UpdateAsync(medico, cancellationToken);

            return true;
        }
    }
}
