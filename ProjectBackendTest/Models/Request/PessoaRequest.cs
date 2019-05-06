using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBackendTest.Models.Request
{
    public class PessoaRequest
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [RegularExpression("^[a-zA-Z0-9àÀèÈìÌòÒùÙáÁéÉíÍóÓúÚâÂêÊîÎôÔûÛãÃõÕ\bçÇ' ]+$", ErrorMessage = "Nome não pode conter caracteres especiais.")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [EmailAddress]
        [RegularExpression("^[ 0-9a-zA-Z\b.@_\\-]+$", ErrorMessage = "E-mail não pode conter caracteres especiais.")]
        [MaxLength(100, ErrorMessage = "O E-email deve ter no máximo {1} caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [RegularExpression(@"^[1-9]\d$", ErrorMessage = "Informe DDD válido.")]
        public string DDD { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [RegularExpression("^(([6-9][0-9]{3,4})|([1-9][0-9]{3}))-([0-9]{4})$", ErrorMessage = "Informe um número de telefone válido.")]
        public string Telefone { get; set; }
        public EnderecoRequest Endereco { get; set; }
    }
}
