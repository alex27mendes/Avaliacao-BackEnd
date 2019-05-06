using ProjectBackendTest.Model;
using ProjectBackendTest.Repository.Context;
using System.Linq;


namespace ProjectBackendTest.Repository
{
    public static class DbInitializer
    {
        public static void Initialize(BancoContext context)
        {
           context.Database.EnsureCreated();

            if (context.Pessoas.Any())
            {
                return;
            }

            var pessoas = new Pessoa[]
            {
                new Pessoa{Nome = "Nelson Pereira",
                           Telefone = "9999-9999",
                           DDD = "41",
                           Email = "email@email.com",
                           Endereco = new Endereco{
                               Rua = "XV de Novembro",
                               Bairro = "Centro",
                               Cidade = "Curitiba",
                               Cep = "81810-999",
                               UF = "PR",
                               Numero = "3",
                               Complemento = ""
                           }
                },
                new Pessoa{
                    Nome = "Mariana Sousa",
                    Telefone = "9999-9999",
                    DDD = "41",
                    Email = "maria@email.com",
                    Endereco = new Endereco{
                        Rua = "Marechal Deodoro",
                        Bairro = "Centro",
                        Cidade = "Curitiba",
                        Cep = "81810-888",
                        UF = "PR",
                        Numero = "3",
                        Complemento = "Predio 1"
                    }
                },
                new Pessoa{
                    Nome = "Joao da Silva",
                    Telefone = "9999-9999",
                    DDD = "41",
                    Email = "maria@email.com",
                    Endereco = new Endereco{
                        Rua = "Marechal Deodoro",
                        Bairro = "Centro",
                        Cidade = "Curitiba",
                        Cep = "81810-888",
                        UF = "PR",
                        Numero = "3",
                        Complemento = "Predio 1"
                    }
                }

            };
            foreach (Pessoa p in pessoas)
            {
                context.Add(p);
            }
            context.SaveChanges();

        }
    }
}
