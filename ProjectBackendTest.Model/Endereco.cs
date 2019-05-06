using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectBackendTest.Model
{
    public class Endereco
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Rua { get; set; }
        [MaxLength(10)]
        public string Numero { get; set; }
        [MaxLength(10)]
        public string Cep { get; set; }
        [MaxLength(100)]
        public string Bairro { get; set; }
        [MaxLength(50)]
        public string Cidade { get; set; }
        [MaxLength(2)]
        public string UF { get; set; }
        [MaxLength(100)]
        public string Complemento { get; set; }
        public int IdPessoaFK { get; set; }
        [ForeignKey("IdPessoaFK")]
        public Pessoa Pessoa { get; set; }


    }
}
