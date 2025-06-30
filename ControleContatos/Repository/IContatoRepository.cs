using ControleContatos.Models;

namespace ControleContatos.Repository
{
    public interface IContatoRepository
    {
        ContatoModel Adicionar(ContatoModel contato);
        ContatoModel Atualizar(ContatoModel contato);
        bool Apagar(int id);
        ContatoModel ListarPorId(int id);
        List<ContatoModel> BuscarTodos();
    }
}
