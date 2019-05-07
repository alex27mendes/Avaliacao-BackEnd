using System;
using System.Collections.Generic;
using System.Text;
using ProjectBackendTest.DAL;
using ProjectBackendTest.Model;
using ProjectBackendTest.Models.Request;

namespace ProjectBackendTest.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaService(IPessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }
        public bool AtualizarPessoa(int Id, PessoaRequest request)
        {
            var _pessoa = new Pessoa
            {
                Id = Id,
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
            return result;
        }

        public PessoaRequest GetPessoa(int Id)
        {
            var pessoa = _pessoaRepository.Find(Id);

            if (pessoa == null)
            {
                return null;
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
            return response;
        }

        public List<PessoaRequest> GetPessoas()
        {
            List<PessoaRequest> responseList = new List<PessoaRequest>();
            var result = _pessoaRepository.GetAll();

            foreach(Pessoa pessoa in result){
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
                responseList.Add(response);

            }

            return responseList;
        }

        public bool RemoverPessoa(int Id)
        {
            var pessoa = _pessoaRepository.Find(Id);
            if (pessoa == null)
            {
                return true;
            }
            var result = _pessoaRepository.Remove(pessoa);
            return result;
        }

        public bool SalvarPessoa(PessoaRequest pessoaRequest)
        {
            var _pessoa = new Pessoa
            {
                Nome = pessoaRequest.Nome,
                Email = pessoaRequest.Email,
                Telefone = pessoaRequest.Telefone,
                DDD = pessoaRequest.DDD
            };
            if (pessoaRequest.Endereco != null)
            {
                _pessoa.Endereco = new Model.Endereco
                {
                    Rua = pessoaRequest.Endereco.Rua,
                    Numero = pessoaRequest.Endereco.Numero,
                    Bairro = pessoaRequest.Endereco.Bairro,
                    Cidade = pessoaRequest.Endereco.Cidade,
                    Cep = pessoaRequest.Endereco.Cep,
                    UF = pessoaRequest.Endereco.UF,
                    Complemento = pessoaRequest.Endereco.Complemento
                };
            }
            return _pessoaRepository.Save(_pessoa);

        }
        public Pessoa Find(int key)
        {
            return _pessoaRepository.Find(key);
        }
    }
}
