using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBackendTest.Model
{
    public class Pessoa
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Obrigatório Nome")]
        [MaxLength(100)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Obrigatório Email")]
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(3)]
        public string DDD { get; set; }
        [MaxLength(20)]
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }
    }
}
