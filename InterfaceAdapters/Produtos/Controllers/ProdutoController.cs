using AutoMapper;
using CasosDeUso.Produtos.Comandos;
using CasosDeUso.Produtos.Interfaces;
using InterfaceAdapters.Produtos.Controllers.Interfaces;
using InterfaceAdapters.Produtos.Presenters.Requests;
using InterfaceAdapters.Produtos.Presenters.Responses;
using InterfaceAdapters.Transactions.Interfaces;

namespace InterfaceAdapters.Produtos.Controllers
{
    public class ProdutoController(IMapper mapper, IProdutoCasosDeUso produtoCasosDeUso, IUnitOfWorks unitOfWorks) : IProdutoController
    {
        public ProdutoResponse Inserir(ProdutoRequest produtoRequest)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var produto = mapper.Map<ProdutoComando>(produtoRequest);

                produto = produtoCasosDeUso.CadastraProduto(produto);

                unitOfWorks.Commit();

                return mapper.Map<ProdutoResponse>(produto);
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }

        public ProdutoResponse Alterar(int codigo, ProdutoRequest produtoRequest)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var produtoComando = mapper.Map<ProdutoComando>(produtoRequest);

                produtoComando.Codigo = codigo;

                produtoComando = produtoCasosDeUso.AlterarProdutos(produtoComando);

                unitOfWorks.Commit();

                return mapper.Map<ProdutoResponse>(produtoComando);

            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }

        }

        public void Excluir(int codigo)
        {
            try
            {
                unitOfWorks.Begintransaction();

                produtoCasosDeUso.Excluir(codigo);

                unitOfWorks.Commit();
            }
            catch
            {
                unitOfWorks.RollBack();

                throw;
            }

        }

        public List<ProdutoResponse> Consultar(ProdutoFiltroRequest produtoFiltroRequest)
        {
            try
            {
                var produto = mapper.Map<ProdutoComando>(produtoFiltroRequest);
                var produtos = produtoCasosDeUso.FiltrarProdutos(produto);

                return mapper.Map<List<ProdutoResponse>>(produtos); ;
            }
            catch
            {
                throw;
            }
        }

        public ProdutoResponse Consultar(int codigo)
        {
            try
            {
                var produto = produtoCasosDeUso.ConsultarPorCodigo(codigo);

                return mapper.Map<ProdutoResponse>(produto);
            }
            catch
            {
                throw;
            }
        }
    }
}
