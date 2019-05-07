using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBackendTest.Models.Request
{
    public class EnderecoRequest
    {
        public int Id { get; set; }
        [MaxLength(100, ErrorMessage = "O nome da rua deve ter no máximo {1} caracteres.")]
        public string Rua { get; set; }
        [MaxLength(10, ErrorMessage = "O Numero deve ter no máximo {1} caracteres.")]
        public string Numero { get; set; }
        [Required(ErrorMessage = "{0} é obrigatório")]
        [RegularExpression("[0-9]{5}-[0-9]{3}", ErrorMessage = "Informe um Cep válido.")]
        public string Cep { get; set; }
        [MaxLength(100, ErrorMessage = "O Bairro deve ter no máximo {1} caracteres.")]
        public string Bairro { get; set; }
        [MaxLength(50, ErrorMessage = "O Nome da cidade deve ter no máximo {1} caracteres.")]
        public string Cidade { get; set; }
        [MaxLength(2, ErrorMessage = "O UF do estado deve ter no máximo {1} caracteres.")]
        public string UF { get; set; }
        [MaxLength(100, ErrorMessage = "O Complemento deve ter no máximo {1} caracteres.")]
        public string Complemento { get; set; }
    }
}
