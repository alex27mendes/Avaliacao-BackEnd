using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectBackendTest.Controllers;
using ProjectBackendTest.Repository;
using ProjectBackendTest.Model;
using ProjectBackendTest.Models.Request;
using ProjectBackendTest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBackend.Test
{
    [TestClass]
    public class PessoaControllerTest
    {
        public readonly IPessoaRepository MockPessoaRepository;

        public PessoaControllerTest()
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

            Mock<IPessoaRepository> mockPessoaRepository = new Mock<IPessoaRepository>();

            mockPessoaRepository.Setup(mr => mr.GetAll()).Returns(pessoas);

            mockPessoaRepository.Setup(mr => mr.Find(
                It.IsAny<int>())).Returns((int i) => pessoas.Where(
                x => x.Id == i).SingleOrDefault());

            mockPessoaRepository.Setup(mr => mr.Find(
                It.IsAny<string>())).Returns((string s) => pessoas.Where(
                x => x.Nome == s).Single());

            mockPessoaRepository.Setup(r => r.Remove(It.IsAny<Func<Pessoa, bool>>()));

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
        public void TestPostPessoa()
        {
           // Mock<IPessoaRepository> mockPessoaRepository = new Mock<IPessoaRepository>();

           // var _pessoaRepository = new Mock<IPessoaRepository>(mockPessoaRepository);
            var controller = new PessoasController(this.MockPessoaRepository);
            var item = GetDemoPessoa();
            var result = controller.PostPessoa(item) as CreatedAtActionResult;
            //  var result = controller.PostPessoa(item) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
            Assert.AreEqual(result.ActionName, "GetPessoa");
            //Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
            //Assert.AreEqual(result.Content.Nome, item.Nome);
            //  Assert.Pass();
        }
        
        [TestMethod]
        public async Task PutPessoa_ReturnOK()
        { 
            var controller = new PessoasController(this.MockPessoaRepository);

           // var item = GetDemoPessoa();
            var pessoa  = this.MockPessoaRepository.Find(1);
            var request = new PessoaRequest
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

            request.Telefone = "7777-7777";
            request.DDD = "011";

            var result = await controller.PutPessoa(1, request) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetPessoaById()
        {
            var controller = new PessoasController(this.MockPessoaRepository);
            var result = await controller.GetPessoa(1) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void TestGetPessoas()
        {
            var pessoas = this.MockPessoaRepository.GetAll();
            var total = pessoas.Count();

            var controller = new PessoasController(this.MockPessoaRepository);
            var result = controller.Getpessoas() as IEnumerable<Pessoa>;

            Assert.IsNotNull(result);
            Assert.AreEqual(total, result.Count());
        }

        [TestMethod]
        public async Task DeletePessoa_ReturnOK()
        {
            var item = this.MockPessoaRepository.Find(1);

            var controller = new PessoasController(this.MockPessoaRepository);
            var result = await controller.DeletePessoa(item.Id) as OkObjectResult;
            var _pessoa = result.Value as Pessoa;

            Assert.IsNotNull(result);
            Assert.AreEqual(item.Id, _pessoa.Id);

            var itemDeletado = this.MockPessoaRepository.Find(1);
            Assert.IsNotNull(itemDeletado);
        }
        [TestMethod]
        public async Task TestPutPessoaNOK_IdInexistente()
        {
            var controller = new PessoasController(this.MockPessoaRepository);

            // var item = GetDemoPessoa();
            var pessoa = this.MockPessoaRepository.Find(1);
            var request = new PessoaRequest
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

            request.Telefone = "7777-7777";
            request.DDD = "011";

            var result = await controller.PutPessoa(10, request) as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetPessoaById_NOK()
        {
            var controller = new PessoasController(this.MockPessoaRepository);
            var result = await controller.GetPessoa(30) as NotFoundResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
        [TestMethod]
        public async Task TesteNome_Pessoa_Vazio_NOK()
        {

            var request = new PessoaRequest { Nome = "" };

            var controller = new PessoasController(this.MockPessoaRepository);
            controller.ModelState.AddModelError("Nome", "O nome é obrigatório.");
            var result = controller.PostPessoa(request) as BadRequestObjectResult;
            Assert.AreEqual(400, result.StatusCode);

        }
        [TestMethod]
        public async Task TesteNome_Pessoa_Invalido_NOK()
        {

            var request = new PessoaRequest { Nome = "Inválido" };

            var controller = new PessoasController(this.MockPessoaRepository);
            controller.ModelState.AddModelError("Nome", "O nome é invalido.");
            var result = controller.PostPessoa(request) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

        }
        [TestMethod]
        public async Task TesteEmail_Vazio_NOK()
        {

            var request = new PessoaRequest { Email = "" };

            var controller = new PessoasController(this.MockPessoaRepository);
            controller.ModelState.AddModelError("Email", "O email é obrigatório.");
            var result = controller.PostPessoa(request) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            
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
