using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectBackendTest.Controllers;
using ProjectBackendTest.Model;
using ProjectBackendTest.Models.Request;
using ProjectBackendTest.Repository;
using ProjectBackendTest.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBackend.Test
{
    [TestClass]
    public class PessoaControllerTest
    {
        public readonly IPessoaRepository MockPessoaRepository;

        Mock<IPessoaRepository> mockPessoaRepository = new Mock<IPessoaRepository>();
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


            mockPessoaRepository.Setup(mr => mr.GetAll()).Returns(pessoas);

            mockPessoaRepository.Setup(mr => mr.Find(
                It.IsAny<int>())).Returns((int i) => pessoas.Where(
                x => x.Id == i).SingleOrDefault());

            mockPessoaRepository.Setup(mr => mr.Find(
                It.IsAny<string>())).Returns((string s) => pessoas.Where(
                x => x.Nome == s).Single());

            mockPessoaRepository.Setup(r => r.Remove(It.IsAny<Pessoa>()));

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

            Mock<IPessoaService> mockPessoaService = new Mock<IPessoaService>();
            var controller = new PessoasController(mockPessoaService.Object);
            var item = GetDemoPessoa();
            var result = controller.PostPessoa(item) as CreatedAtActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
            Assert.AreEqual(result.ActionName, "GetPessoa");

        }
        
        [TestMethod]
        public async Task PutPessoa_ReturnOK()
        {
            Mock<IPessoaService> mockPessoaService = new Mock<IPessoaService>();
            var controller = new PessoasController(mockPessoaService.Object);

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
        //[TestMethod]
        //public async Task TestGetPessoaById()
        //{
        //    var mockService = new Mock<IPessoaService>();
        //    var controller = new PessoasController(mockService.Object);

        //    var pessoa = this.MockPessoaRepository.Find(1);
        //    var result = await controller.GetPessoa(1) as OkObjectResult;
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(200, result.StatusCode);
        //}

      // [TestMethod]
        //public void TestGetPessoas()
        //{
        //    var pessoas = this.MockPessoaRepository.GetAll();
        //    var total = pessoas.Count();

        //    Mock<IPessoaService> mockPessoaService = new Mock<IPessoaService>(MockPessoaRepository);
        //    var controller = new PessoasController(mockPessoaService.Object);
        //    var result = controller.Getpessoas() as List<Pessoa>;

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(total, result.Value.Count());
        //}
        [TestMethod]
        public async Task TestPutPessoaNOK_IdInexistente()
        {
            Mock<IPessoaService> mockPessoaService = new Mock<IPessoaService>();
            var controller = new PessoasController(mockPessoaService.Object);
            
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
        public async Task GetReturnsNotFound()
        {
            // Arrange
            Mock<IPessoaService> mockPessoaService = new Mock<IPessoaService>();
            var controller = new PessoasController(mockPessoaService.Object);

            // Act
            var result = await controller.GetPessoa(10) as NotFoundResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task TestGetPessoaById_NOK()
        {
            Mock<IPessoaService> mockPessoaService = new Mock<IPessoaService>();
            var controller = new PessoasController(mockPessoaService.Object);
            var result = await controller.GetPessoa(30) as NotFoundResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
        [TestMethod]
        public async Task TestDeleteReturnsOk()
        {
            // Arrange
            var mockService = new Mock<IPessoaService>();
            var controller = new PessoasController(mockService.Object);

            var pessoa = this.MockPessoaRepository.Find(1);
            // Act
            var result = await controller.DeletePessoa(1) as OkObjectResult;

            mockService.Verify(r => r.RemoverPessoa(pessoa.Id));

            // Assert
            //Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void TestPostMethod()
        {
            // Arrange
            var mockRepository = new Mock<IPessoaService>();
            var controller = new PessoasController(mockRepository.Object);

            // Act
            var result = controller.PostPessoa(GetDemoPessoa())  as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
            Assert.AreEqual(result.ActionName, "GetPessoa");

            
        }

        [TestMethod]
        public async Task TesteNome_Pessoa_Vazio_NOK()
        {

            var request = new PessoaRequest { Nome = "" };

            Mock<IPessoaService> mockPessoaService = new Mock<IPessoaService>();
            var controller = new PessoasController(mockPessoaService.Object);
            controller.ModelState.AddModelError("Nome", "O nome é obrigatório.");
            var result =  controller.PostPessoa(request) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

        }
        [TestMethod]
        public async Task TesteNome_Pessoa_Invalido_NOK()
        {

            var request = new PessoaRequest { Nome = "Inválido" };

            Mock<IPessoaService> mockPessoaService = new Mock<IPessoaService>();
            var controller = new PessoasController(mockPessoaService.Object);
            controller.ModelState.AddModelError("Nome", "O nome é invalido.");
            var result = controller.PostPessoa(request) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

        }
        public async Task TesteNome_Email_Vazio_NOK()
        {

            var request = new PessoaRequest { Email = "" };

            Mock<IPessoaService> mockPessoaService = new Mock<IPessoaService>();
            var controller = new PessoasController(mockPessoaService.Object);
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
