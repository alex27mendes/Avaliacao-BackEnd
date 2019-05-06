using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectBackendTest.Repository;
using ProjectBackendTest.Repository.Context;
using ProjectBackendTest.Model;
using System.Collections.Generic;
using System.Linq;

namespace ProjectBackend.Test
{
    [TestClass]
    public class PessoaRepositoryTest
    {
        public PessoaRepositoryTest()
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
                x => x.Id == i).Single());

            mockPessoaRepository.Setup(mr => mr.Find(
                It.IsAny<string>())).Returns((string s) => pessoas.Where(
                x => x.Nome == s).Single());

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
        public BancoContext context;

        public readonly IPessoaRepository MockPessoaRepository;
        

        [TestMethod]
        public void TestConsultaIdPessoa()
        {
            int id = 2;
            Pessoa testPessoa = this.MockPessoaRepository.Find(id);

            Assert.IsNotNull(testPessoa); 
            Assert.IsInstanceOfType(testPessoa, typeof(Pessoa));
            Assert.AreEqual("Maria da Silva", testPessoa.Nome); 
        }
        [TestMethod]
        public void TestReturnAllPessoas()
        {
            // Try finding all products
            IList<Pessoa> testProducts = this.MockPessoaRepository.GetAll();

            Assert.IsNotNull(testProducts); 
            Assert.AreEqual(2, testProducts.Count); 
        }
        [TestMethod]
        public void TestInsertPessoa()
        {
            Pessoa newPessoa = new Pessoa
            {
 //               Id = 3,
                Nome = "Fulano da Silva",
                Telefone = "7777-9999",
                DDD = "41",
                Email = "fulano@email.com",
                Endereco = new Endereco
                {
 //                   Id = 3,
                    Rua = "Marechal Floriano",
                    Bairro = "Centro",
                    Cidade = "Curitiba",
                    Cep = "81810-777",
                    UF = "PR",
                    Numero = "3",
                    Complemento = "",
//                    IdPessoaFK = 3

                }
            };

            int pessoasCount = this.MockPessoaRepository.GetAll().Count;
            Assert.AreEqual(2, pessoasCount);


            this.MockPessoaRepository.Save(newPessoa);

            pessoasCount = this.MockPessoaRepository.GetAll().Count;
            Assert.AreEqual(3, pessoasCount); 

            Pessoa testPessoa = this.MockPessoaRepository.Find(3);
            Assert.IsNotNull(testPessoa); 
            Assert.IsInstanceOfType(testPessoa, typeof(Pessoa)); 
            Assert.AreEqual(3, testPessoa.Id); 
        }
        [TestMethod]
        public void TestUpdatePessoa()
        {
            Pessoa testPessoa = this.MockPessoaRepository.Find(1);

            testPessoa.Telefone = "7777-7777";
            testPessoa.DDD = "011";

            this.MockPessoaRepository.Save(testPessoa);

            Assert.AreEqual("7777-7777", this.MockPessoaRepository.Find(1).Telefone);
            Assert.AreEqual("011", this.MockPessoaRepository.Find(1).DDD);
        }

    }
}
