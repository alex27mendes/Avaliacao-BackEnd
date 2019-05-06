using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectBackendTest.Repository;
using ProjectBackendTest.Model;
using ProjectBackendTest.Models.Request;

namespace ProjectBackendTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoasController(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }

        // GET: api/Pessoas
        [HttpGet]
        public IEnumerable<Pessoa> Getpessoas()
        {
            var result = _pessoaRepository.GetAll();
            return result;
        }

        //GET: api/Pessoas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPessoa([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pessoa = _pessoaRepository.Find(id);



            if (pessoa == null)
            {
                return NotFound();
            }
            var response = new PessoaRequest
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Email = pessoa.Email,
                Telefone = pessoa.Telefone,
                DDD = pessoa.DDD,
                Endereco = new EnderecoRequest
                {
                    Id = pessoa.Endereco.Id,
                    Rua = pessoa.Endereco.Rua,
                    Numero = pessoa.Endereco.Numero,
                    Bairro = pessoa.Endereco.Bairro,
                    Cidade = pessoa.Endereco.Cidade,
                    Cep = pessoa.Endereco.Cep,
                    UF = pessoa.Endereco.UF,
                    Complemento = pessoa.Endereco.Complemento
                }

            };

            return Ok(response);
        }

        // PUT: api/Pessoas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa([FromRoute] int id, [FromBody] PessoaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id.Value)
            {
                return BadRequest("Id para atualizar diferente id da Pessoa");
            }

            try
            {
                var _pessoa = new Pessoa
                {
                    Id = id,
                    Nome = request.Nome,
                    Email = request.Email,
                    Telefone = request.Telefone,
                    DDD = request.DDD
                };
                if (request.Endereco != null)
                {
                    _pessoa.Endereco = new Model.Endereco
                    {
                        Rua = request.Endereco.Rua,
                        Numero = request.Endereco.Numero,
                        Bairro = request.Endereco.Bairro,
                        Cidade = request.Endereco.Cidade,
                        Cep = request.Endereco.Cep,
                        UF = request.Endereco.UF,
                        Complemento = request.Endereco.Complemento

                    };
                }

                var result = _pessoaRepository.Update(_pessoa);
                return Ok(_pessoa);
            }
            catch (Exception e)
            {
                return BadRequest();

            }

            return NoContent();
        }

        // POST: api/Pessoas
        [HttpPost]
        public IActionResult PostPessoa([FromBody] PessoaRequest request)
         {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var _pessoa = new Pessoa
                {
                    Nome = request.Nome,
                    Email = request.Email,
                    Telefone = request.Telefone,
                    DDD = request.DDD
                };
                if (request.Endereco != null)
                {
                    _pessoa.Endereco = new Model.Endereco
                    {
                        Rua = request.Endereco.Rua,
                        Numero = request.Endereco.Numero,
                        Bairro = request.Endereco.Bairro,
                        Cidade = request.Endereco.Cidade,
                        Cep = request.Endereco.Cep,
                        UF = request.Endereco.UF,
                        Complemento = request.Endereco.Complemento
                    };
                }
                _pessoaRepository.Save(_pessoa);
                return CreatedAtAction("GetPessoa", new { id = _pessoa.Id }, _pessoa);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return BadRequest(ModelState);
            }

        }

        // DELETE: api/Pessoas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pessoa = _pessoaRepository.Find(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            var result =_pessoaRepository.Remove(c => c == pessoa);

            return Ok(pessoa);
        }
    }
}