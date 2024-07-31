using FluentValidation;
using Hackaton.Api.Repository.Interface;
using MediatR;

namespace Hackaton.Api.Domain.Commands.Medico.Delete
{
    public class DeleteMedicoHandler : IRequestHandler<DeleteMedicoCommand, bool>
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IValidator<DeleteMedicoCommand> _validator;

        public DeleteMedicoHandler(IMedicoRepository medicoRepository, IValidator<DeleteMedicoCommand> validator)
        {
            _medicoRepository = medicoRepository;
            _validator = validator;
        }

        public async Task<bool> Handle(DeleteMedicoCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var medico = await _medicoRepository.GetByIdAsync(command.Id, cancellationToken);
            if (medico is null)
            {
                return false;
            }

            medico.Ativo = false;

            await _medicoRepository.DeleteLogicAsync(medico, cancellationToken);

            return true;
        }
    }
}
