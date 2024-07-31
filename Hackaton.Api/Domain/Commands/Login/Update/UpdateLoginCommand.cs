using MediatR;

namespace Hackaton.Api.Domain.Commands.Login.Update
{
    public class UpdateLoginCommand : IRequest<bool?>
    {
        public int Id { get; private set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public bool? Ativo { get; set; }

        public void SetId(int id) => Id = id;
    }
}
