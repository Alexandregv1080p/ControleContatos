﻿using ControleContatos.Controllers;
using ControleContatos.Data;
using ControleContatos.Models;

namespace ControleContatos.Repository
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly BancoContext _bancoContext;
        public ContatoRepository(BancoContext bancoContext) 
        {
            _bancoContext = bancoContext;
        }
        
        public ContatoModel Adicionar(ContatoModel contato)
        {
            _bancoContext.Contatos.Add(contato);
            _bancoContext.SaveChanges();
            return contato;
        }

        public bool Apagar(int id)
        {
            ContatoModel contatoDB = ListarPorId(id);
            if (contatoDB == null)
            {
                throw new Exception("Houve um erro na deleção do contato");
            }
            _bancoContext.Contatos.Remove(contatoDB);
            _bancoContext.SaveChanges();
            return true;
        }

        public ContatoModel Atualizar(ContatoModel contato)
        {
            ContatoModel contatoDB = ListarPorId(contato.Id);
            if (contatoDB == null) {
                throw new Exception("Houve um erro na atualização do contato");
            }
            contatoDB.Nome = contato.Nome;
            contatoDB.Email = contato.Email;
            contatoDB.Celular = contato.Celular;
            _bancoContext.Contatos.Update(contatoDB);
            _bancoContext.SaveChanges();
            return contatoDB;
        }

        public List<ContatoModel> BuscarTodos()
        {
            return _bancoContext.Contatos.ToList();
        }

        public ContatoModel ListarPorId(int id)
        {
            return _bancoContext.Contatos.FirstOrDefault(x => x.Id == id);
        }
    }
}
