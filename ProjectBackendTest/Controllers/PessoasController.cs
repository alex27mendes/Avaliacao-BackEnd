using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectBackendTest.DAL;
using ProjectBackendTest.Model;
using ProjectBackendTest.Models.Request;
using ProjectBackendTest.Services;

namespace ProjectBackendTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;
   

        public PessoasController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        // GET: api/Pessoas
        [HttpGet]
        public  List<PessoaRequest> Getpessoas()
        {
            var reponse = _pessoaService.GetPessoas();
            return reponse;
        }

        //GET: api/Pessoas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPessoa([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = _pessoaService.GetPessoa(id);
            
            if(response == null)
            {
                return NotFound();
            }
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
                var response = _pessoaService.AtualizarPessoa(id, request);
                return Ok(response);
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
                _pessoaService.SalvarPessoa(request);

                return CreatedAtAction("GetPessoa", new { id = request.Id }, request);
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

            var result = _pessoaService.RemoverPessoa(id);

            if (!result)
            {
                return NoContent();
            }

            return Ok();
        }
    }
}