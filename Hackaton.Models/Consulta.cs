using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Hackaton.Models
{
    [Table(name: "Consulta", Schema = "dbo")]
    public class Consulta
    {
        [Key, Column("Id")]
        public int Id { get; set; }

        [Column("PacienteId")]
        public int PacienteId { get; set; }
        
        [Column("MedicoId")]
        public int MedicoId { get; set; }
        
        [Column("DataAgendamento")]
        public DateTime DataAgendamento { get; set; }
        
        [Column("Ativo")]
        public bool Ativo { get; set; }

        public virtual Medico Medico { get; set; }
        public virtual Paciente Paciente { get; set; }

        public Consulta(int pacienteId, int medicoId, DateTime dataAgendamento)
        {
            this.PacienteId = pacienteId;
            this.MedicoId = medicoId;
            this.DataAgendamento = dataAgendamento;
            this.Ativo = true;
        }
    }
}