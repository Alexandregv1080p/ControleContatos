using ControleContatos.Models;

namespace ControleContatos.Repository
{
    public interface IContatoRepository
    {
        ContatoModel Adicionar(ContatoModel contato);
        List<ContatoModel> BuscarTodos();
    }
}
