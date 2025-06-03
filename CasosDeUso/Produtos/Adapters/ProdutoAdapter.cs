using AutoMapper;
using CasosDeUso.Produtos.Comandos;
using Entidades.Produtos;

namespace CasosDeUso.Produtos.Adapters
{
    public class ProdutoAdapter : Profile
    {
        public ProdutoAdapter()
        {
            CreateMap<ProdutoComando, Produto>().ReverseMap();
        }
    }
}
