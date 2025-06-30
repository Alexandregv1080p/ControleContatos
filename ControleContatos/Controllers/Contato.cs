using ControleContatos.Models;
using ControleContatos.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ControleContatos.Controllers
{
    public class Contato : Controller
    {
        private readonly IContatoRepository _contatoRepository;
        public Contato(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }
        public IActionResult Index()
        {
            var contatos = _contatoRepository.BuscarTodos();

            return View(contatos);
        }
        public IActionResult Criar()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            ContatoModel contato = _contatoRepository.ListarPorId(id);
            return View(contato);
        }
        public IActionResult ApagarConfirmacao(int id)
        {
            ContatoModel contato = _contatoRepository.ListarPorId(id);
            return View(contato);
        }
        public IActionResult Apagar(int id) 
        {
            _contatoRepository.Apagar(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Criar(ContatoModel contato)
        {
            _contatoRepository.Adicionar(contato);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Alterar(ContatoModel contato)
        {
            _contatoRepository.Atualizar(contato);
            return RedirectToAction("Index");
        }

    }
}