using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Worker.Repository.Interface
{
    public interface IEmailService
    {
        void EnviarEmail(string emailPaciente, string nomePaciente, string nomeMedico, DateTime dataConsulta);
    }
}
