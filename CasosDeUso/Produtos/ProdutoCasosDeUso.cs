using AutoMapper;
using CasosDeUso.Pedidos.Interfaces;
using CasosDeUso.Produtos.Comandos;
using CasosDeUso.Produtos.Interfaces;
using CasosDeUso.Produtos.Interfaces.Gateway;
using Entidades.Produtos;
using Entidades.Util;

namespace CasosDeUso.Produtos
{
    public class ProdutoCasosDeUso(IMapper mapper, IProdutoGateway produtoGateway, IPedidoComboItemCasosDeUso pedidoComboItemCasosDeUso) : IProdutoCasosDeUso
    {
        public ProdutoComando AlterarProdutos(ProdutoComando produtoComando)
        {
            var produto = RetornarPorCodigo(produtoComando.Codigo);

            mapper.Map(produtoComando, produto);

            produtoGateway.Alterar(produto);

            return mapper.Map<ProdutoComando>(produto);
        }

        public void Excluir(int codigo)
        {
            var produto = RetornarPorCodigo(codigo);

            var produtoTemDependencias = pedidoComboItemCasosDeUso.ExisteComboItemComProduto(codigo);

            if (produtoTemDependencias)
                throw new RegraNegocioException("Não é possível excluir o produto, pois ele está sendo usado em pedidos.");

            produtoGateway.Excluir(produto);

            return;
        }

        public List<ProdutoComando> FiltrarProdutos(ProdutoComando produtoComando)
        {

            var produtos = produtoGateway.Query();

            if (!string.IsNullOrWhiteSpace(produtoComando.Nome))
            {
                produtos = produtos.Where(p => p.Nome.ToLower().Contains(produtoComando.Nome.ToLower()));
            }

            if (produtoComando.Categoria.HasValue)
            {
                produtos = produtos.Where(p => p.Categoria == (Entidades.Produtos.Enums.CategoriaEnum)produtoComando.Categoria);
            }

            if (produtoComando.Situacao.HasValue)
            {
                produtos = produtos.Where(p => p.Situacao == (Entidades.Produtos.Enums.AtivoInativoEnum)produtoComando.Situacao);
            }

            var listaProdutos = produtos;

            return mapper.Map<List<ProdutoComando>>(listaProdutos);

        }

        public ProdutoComando CadastraProduto(ProdutoComando produtoComando)
        {
            var produto = mapper.Map<Produto>(produtoComando);

            produtoGateway.Inserir(produto);

            return mapper.Map<ProdutoComando>(produto);
        }

        public Produto RetornarPorCodigo(int codigo)
        {
            var produto = produtoGateway.RetornarPorCodigo(codigo) ??
                throw new RegraNegocioException("Produto não localizado");

            return produto;
        }

        public ProdutoComando ConsultarPorCodigo(int codigo)
        {
            var produto = produtoGateway.RetornarPorCodigo(codigo) ??
                throw new RegraNegocioException("Produto não localizado");

            return mapper.Map<ProdutoComando>(produto);
        }
    }
}
