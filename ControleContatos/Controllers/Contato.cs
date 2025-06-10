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
        public IActionResult Editar()
        {
            return View();
        }
        public IActionResult ApagarConfirmacao()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Criar(ContatoModel contato)
        {
            _contatoRepository.Adicionar(contato);
            return RedirectToAction("Index");
        }
       
    }
}
