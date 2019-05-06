using Microsoft.EntityFrameworkCore;
using ProjectBackendTest.Repository.Context;
using ProjectBackendTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBackendTest.Repository
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly BancoContext _context;

        public PessoaRepository(BancoContext context)
        {
            _context = context;
        }
        public bool Remove(Func<Pessoa, bool> predicate)
        {
            _context.Set<Pessoa>()
            .Where(predicate).ToList()
            .ForEach(del => _context.Set<Pessoa>().Remove(del));
            _context.SaveChanges();
            return true;
        }

        public Pessoa Get(Func<Pessoa, bool> predicate)
        {
            var pessoa = GetAll().Where(predicate).FirstOrDefault();
            if(pessoa != null)
            {
                pessoa.Endereco = _context.Enderecos.FirstOrDefault(x => x.IdPessoaFK == pessoa.Id);
            }
            return pessoa;
            
        }

        public List<Pessoa> GetAll()
        {
            return _context.Pessoas
                           .Include(x => x.Endereco).ToList();
        }

        public bool Save(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            _context.SaveChanges();
            return true;
            
        }

        public bool Update(Pessoa pessoa)
        {
            // _context.Entry(pessoa).State = EntityState.Modified;
            var res = _context.Pessoas.Where(x => x.Id == pessoa.Id)
                                      .Include(x => x.Endereco)
                                      .FirstOrDefault();
            if(res == null)
            {
                return false;
            }
            res.Nome = pessoa.Nome;
            res.Email = pessoa.Email;
            res.Telefone = pessoa.Telefone;
            res.DDD = pessoa.DDD;
            
            if(pessoa.Endereco != null)
            {
                res.Endereco.Rua = pessoa.Endereco.Rua;
                res.Endereco.Bairro = pessoa.Endereco.Bairro;
                res.Endereco.Cidade = pessoa.Endereco.Cidade;
                res.Endereco.UF = pessoa.Endereco.UF;
                res.Endereco.Cep = pessoa.Endereco.Cep;
                res.Endereco.Complemento = pessoa.Endereco.Complemento;
            }    
            _context.SaveChanges();
            return true;
        }
        public Pessoa Find(params object[] key)
        {
            var  pessoa = _context.Set<Pessoa>().Find(key);
            if (pessoa != null)
            {
                pessoa.Endereco = _context.Enderecos.FirstOrDefault(x => x.IdPessoaFK == pessoa.Id);
            }
            return pessoa;
        }
        public Pessoa Find(int key)
        {
            var pessoa = _context.Set<Pessoa>().Find(key);
            if (pessoa != null)
            {
                pessoa.Endereco = _context.Enderecos.FirstOrDefault(x => x.IdPessoaFK == pessoa.Id);
            }
            return pessoa;
        }
    }

}
