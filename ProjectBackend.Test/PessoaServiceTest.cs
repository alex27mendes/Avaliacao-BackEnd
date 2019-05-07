using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectBackendTest.DAL;
using ProjectBackendTest.Model;
using ProjectBackendTest.Models.Request;
using ProjectBackendTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBackend.Test
{
    [TestClass]
    public class PessoaServiceTest
    {
        public readonly IPessoaRepository MockPessoaRepository;
        Mock<IPessoaRepository> mockPessoaRepository = new Mock<IPessoaRepository>();

        public PessoaServiceTest()
        {
            List<Pessoa> pessoas = new List<Pessoa>
            {
                new Pessoa
                {
                    Id = 1,
                    Nome = "Joao Pessoa dos Santos",
                    Telefone = "9999-9999",
                    DDD = "41",
                    Email = "email@email.com",
                    Endereco = new Endereco
                    {
                        Id = 1,
                        Rua = "XV de Novembro",
                        Bairro = "Centro",
                        Cidade = "Curitiba",
                        Cep = "81810-999",
                        UF = "PR",
                        Numero = "3",
                        Complemento = "",
                        IdPessoaFK = 1

                    }
                },
                new Pessoa
                {
                    Id = 2,
                    Nome = "Maria da Silva",
                    Telefone = "8888-8888",
                    DDD = "41",
                    Email = "email@email.com",
                    Endereco = new Endereco
                    {
                        Id = 2,
                        Rua = "Marechal de Deodoro",
                        Bairro = "Centro",
                        Cidade = "Curitiba",
                        Cep = "81810-999",
                        UF = "PR",
                        Numero = "3",
                        Complemento = "",
                        IdPessoaFK = 2

                    }
                }
            };

            

            mockPessoaRepository.Setup(mr => mr.GetAll()).Returns(pessoas);

            mockPessoaRepository.Setup(mr => mr.Find(
                It.IsAny<int>())).Returns((int i) => pessoas.Where(
                x => x.Id == i).SingleOrDefault());

            mockPessoaRepository.Setup(mr => mr.Find(
                It.IsAny<string>())).Returns((string s) => pessoas.Where(
                x => x.Nome == s).Single());

            mockPessoaRepository.Setup(r => r.Remove(It.IsAny<Pessoa>()));


            // mockPessoaRepository.Setup(r => r.Update(It.IsAny<Pessoa>()));
            mockPessoaRepository.Setup(mr => mr.Update(It.IsAny<Pessoa>()))
                   .Callback((Pessoa pessoa) =>
                   {
                       var original = pessoas.Where(
                            q => q.Id == pessoa.Id).Single();
                       original = pessoa;

                       //do something
                   });




            mockPessoaRepository.Setup(mr => mr.Save(It.IsAny<Pessoa>())).Returns(
                (Pessoa target) =>
                {

                    if (target.Id.Equals(default(int)))
                    {
                        target.Id = pessoas.Count + 1;
                        pessoas.Add(target);
                    }
                    else
                    {
                        var original = pessoas.Where(
                            q => q.Id == target.Id).Single();

                        if (original == null)
                        {
                            return false;
                        }

                        original.Nome = target.Nome;
                        original.Email = target.Email;
                        original.Telefone = target.Telefone;
                        original.DDD = target.DDD;
                    }

                    return true;
                });

            this.MockPessoaRepository = mockPessoaRepository.Object;
        }
        [TestMethod]
        public void TestSalvarPessoa()
        {
            var _service = new PessoaService(this.MockPessoaRepository); 
            _service.SalvarPessoa(GetDemoPessoa());
            mockPessoaRepository.Verify(r => r.Save(It.IsAny<Pessoa>()), Times.Exactly(1));
            mockPessoaRepository.Verify(r => r.Save(It.IsAny<Pessoa>()), Times.Once());
        }
        //[TestMethod]
        //public void TestUpdatePessoa()
        //{
        //    var _service = new PessoaService(this.MockPessoaRepository);
        //    var pessoa = this.MockPessoaRepository.Find(1);

        //    var request = new PessoaRequest
        //    {
        //        Id = pessoa.Id,
        //        Nome = pessoa.Nome,
        //        Email = pessoa.Email,
        //        Telefone = pessoa.Telefone,
        //        DDD = pessoa.DDD,
        //        Endereco = new EnderecoRequest
        //        {
        //            Id = pessoa.Endereco.Id,
        //            Rua = pessoa.Endereco.Rua,
        //            Numero = pessoa.Endereco.Numero,
        //            Bairro = pessoa.Endereco.Bairro,
        //            Cidade = pessoa.Endereco.Cidade,
        //            Cep = pessoa.Endereco.Cep,
        //            UF = pessoa.Endereco.UF,
        //            Complemento = pessoa.Endereco.Complemento
        //        }

        //    };
        //    request.Telefone = "7777-7777";
        //    request.DDD = "011";
        //    //   _service.AtualizarPessoa(1, request);
        //    _service.SalvarPessoa(request);
        // //   mockPessoaRepository.Verify(r => r.Save(pessoa));

        //    var pos = this.MockPessoaRepository.Find(1);
        //    Assert.AreEqual("7777-7777", pos.Telefone);
        //    Assert.AreEqual("011", pos.Telefone);
        //}
        [TestMethod]
        public void TestDeletePessoa()
        {
            var _service = new PessoaService(this.MockPessoaRepository);
            var pessoa = this.MockPessoaRepository.Find(1);

            _service.RemoverPessoa(1);

            mockPessoaRepository.Verify(r => r.Remove(pessoa));
        }
        [TestMethod]
        public void TestGetPessoa()
        {
            var _service = new PessoaService(this.MockPessoaRepository);
            var pessoaRepository = this.MockPessoaRepository.Find(2);


            var pessoaService =   _service.GetPessoa(2);

            Assert.AreEqual("Maria da Silva", pessoaService.Nome);
        }
        [TestMethod]
        public void TestGetAll()
        {
            var _service = new PessoaService(this.MockPessoaRepository);

            var pessoaService = _service.GetPessoas();

            Assert.AreEqual(2 , pessoaService.Count);
        }
        PessoaRequest GetDemoPessoa()
        {
            return new PessoaRequest
            {
                Id = 1,
                Nome = "Joao Pessoa dos Santos",
                Telefone = "9999-9999",
                DDD = "41",
                Email = "email@email.com",
                Endereco = new EnderecoRequest
                {
                    Id = 1,
                    Rua = "XV de Novembro",
                    Bairro = "Centro",
                    Cidade = "Curitiba",
                    Cep = "81810-999",
                    UF = "PR",
                    Numero = "3",
                    Complemento = ""
                }
            };

        }
    }
}
