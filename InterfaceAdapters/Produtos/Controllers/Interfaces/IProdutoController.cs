using InterfaceAdapters.Produtos.Presenters.Requests;
using InterfaceAdapters.Produtos.Presenters.Responses;

namespace InterfaceAdapters.Produtos.Controllers.Interfaces
{
    public interface IProdutoController
    {
        ProdutoResponse Inserir(ProdutoRequest produtoRequest);
        ProdutoResponse Alterar(int codigo, ProdutoRequest produtoRequest);
        void Excluir(int codigo);
        List<ProdutoResponse> Consultar(ProdutoFiltroRequest produtoFiltroRequest);
        ProdutoResponse Consultar(int codigo);
    }
}