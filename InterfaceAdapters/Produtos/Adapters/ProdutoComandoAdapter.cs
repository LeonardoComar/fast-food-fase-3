using AutoMapper;
using CasosDeUso.Produtos.Comandos;
using InterfaceAdapters.Produtos.Presenters.Requests;
using InterfaceAdapters.Produtos.Presenters.Responses;

namespace InterfaceAdapters.Produtos.Adapters
{
    public class ProdutoComandoAdapter : Profile
    {
        public ProdutoComandoAdapter()
        {
            CreateMap<ProdutoComando, ProdutoResponse>();

            CreateMap<ProdutoRequest, ProdutoComando>();

            CreateMap<ProdutoFiltroRequest, ProdutoComando>();
        }
    }
}
