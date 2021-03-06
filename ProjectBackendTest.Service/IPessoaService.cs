﻿using ProjectBackendTest.Model;
using ProjectBackendTest.Models.Request;
using System;
using System.Collections.Generic;

namespace ProjectBackendTest.Services
{
    public interface IPessoaService
    {
        PessoaRequest GetPessoa(int Id);
        List<PessoaRequest> GetPessoas();
        bool SalvarPessoa(PessoaRequest pessoaRequest);
        bool AtualizarPessoa(int Id, PessoaRequest pessoaRequest);
        bool RemoverPessoa(int Id);
        Pessoa Find(int key);
    }
}
