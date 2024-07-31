using Hackaton.Worker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Worker.Repository
{
    public class EmailService : IEmailService
    {
        public void EnviarEmail(string emailPaciente, string nomePaciente, string nomeMedico, DateTime dataConsulta)
        {

            var mensagem = new MailMessage();
            mensagem.To.Add(emailPaciente);
            mensagem.Subject = "Lembrete de Consulta";
            mensagem.Body = $"Olá {nomePaciente},\n\nEste é um lembrete de que você tem uma consulta marcada para o dia {dataConsulta:dd/MM/yyyy} com o Dr(a). {nomeMedico}.\n\nAtenciosamente,\n#NOME DA CLINICA#";

            using (var client = new SmtpClient("smtp.seuservidor.com"))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("seuemail@dominio.com", "suasenha");
                client.Send(mensagem);
            }
        }
    }
}
