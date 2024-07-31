using System.Reflection;

namespace Hackaton.Api.Domain.Queries.Medico.Get
{
    public class GetMedicoResult
    {
        public IEnumerable<Models.Medico>? Medicos { get; set; }

        public GetMedicoResult()
        {
            Medicos = new List<Models.Medico>();
        }
    }
}
