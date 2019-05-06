using ProjectBackendTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBackendTest.Repository
{
    public interface IPessoaRepository
    {
        Pessoa Get(Func<Pessoa, bool> predicate);
        List<Pessoa> GetAll();
        bool Save(Pessoa pessoa);
        bool Update(Pessoa pessoa);
        bool Remove(Func<Pessoa, bool> predicate);
        Pessoa Find(params object[] key);
        Pessoa Find(int key);
    }
}
