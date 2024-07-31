using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hackaton.Models
{
    [Table(name: "Login", Schema = "dbo")]
    public class Login
    {
        [Key, Column("Id")]
        public int Id { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Senha")]
        public string Senha { get; set; }
        [Column("Ativo")]
        public bool Ativo { get; set; }
        [Column("Medico")]
        public bool Medico { get; set; }
        [Column("DataCadastro", TypeName = "datetime")]
        public DateTime DataCadastro { get; set; }
        [Column("DataUltimoLogin", TypeName = "datetime")]
        public DateTime DataUltimoLogin { get; set; }
    }
}
