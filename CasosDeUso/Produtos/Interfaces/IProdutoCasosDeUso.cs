using CasosDeUso.Produtos.Comandos;
using Entidades.Produtos;

namespace CasosDeUso.Produtos.Interfaces
{
    public interface IProdutoCasosDeUso
    {
        Produto RetornarPorCodigo(int codigo);
        ProdutoComando AlterarProdutos(ProdutoComando produtoComando);
        void Excluir(int codigo);
        public List<ProdutoComando> FiltrarProdutos(ProdutoComando produtoComando);
        public ProdutoComando ConsultarPorCodigo(int codigo);
        ProdutoComando CadastraProduto(ProdutoComando produtoComando);
    }
}
