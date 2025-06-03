using AutoMapper;
using CasosDeUso.Pedidos.Comandos;
using CasosDeUso.Produtos.Comandos;
using Entidades.Clientes;
using Entidades.Produtos;
using InterfaceAdapters.Pedidos.Presenters.Requests;
using InterfaceAdapters.Pedidos.Presenters.Responses;

namespace InterfaceAdapters.Pedidos.Adapters
{
    public class PedidoComandoAdapter : Profile
    {
        public PedidoComandoAdapter()
        {

            CreateMap<PedidoComando, PedidoResponse>()
                .ForMember(dest => dest.PedidoStatus, opt => opt.MapFrom(src => src.PedidoStatus
                                                                .OrderByDescending(ps => ps.DataCriacao)
                                                                .FirstOrDefault()));

            CreateMap<PedidoStatusComando, PedidoStatusResponse>();

            CreateMap<PedidoComboComando, PedidoComboResponse>();

            CreateMap<PedidoPagamentoComando, PedidoPagamentoResponse>();

            CreateMap<PedidoStatusComando, PedidoStatusResponse>();

            CreateMap<PedidoStatusComando, PedidoStatusMonitorResponse>();

            CreateMap<PedidoRequest, PedidoComando>()
                .ForMember(dest => dest.PedidoCombo, opt => opt.MapFrom(src => src.PedidoCombo));

            CreateMap<PedidoComboRequest, PedidoComboComando>()
                .ForMember(dest => dest.PedidoComboItem, opt => opt.MapFrom(src => src.PedidoComboItems));

            CreateMap<PedidoComboItemRequest, PedidoComboItemComando>()
                .ForMember(dest => dest.CodigoProduto, opt => opt.MapFrom(src => src.ProdutoCodigo));

            CreateMap<PedidoComboItemComando, PedidoComboItemResponse>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Produto.Nome))
                .ForMember(dest => dest.preco, opt => opt.MapFrom(src => src.Preco));

            CreateMap<PedidoPagamentoRequest, PedidoPagamentoComando>();

            CreateMap<PedidoComando, PedidoCozinhaResponse>()
                .ForMember(dest => dest.Pedido, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.StatusPedido, opt => opt.MapFrom(src => src.PedidoStatus.Max(ps => ps.Status)))
                .ForMember(dest => dest.DataCriacao, opt => opt.MapFrom(src => src.PedidoStatus.Max(ps => ps.DataCriacao)))
                .ForMember(dest => dest.TempoEmPreparo, opt => opt.MapFrom(src => src.PedidoStatus.Any(ps => ps.Status == CasosDeUso.Pedidos.Enums.StatusPedido.EmPreparo)
                                                                                                      ? (DateTime.Now - src.PedidoStatus.Where(ps => ps.Status == CasosDeUso.Pedidos.Enums.StatusPedido.EmPreparo)
                                                                                                      .Max(ps => ps.DataCriacao)) : (TimeSpan?)null))
                .ForMember(dest => dest.PedidoCombo, opt => opt.MapFrom(src => src.PedidoCombo));

            CreateMap<PedidoComboComando, PedidoComboCozinhaResponse>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.PedidoComboItem, opt => opt.MapFrom(src => src.PedidoComboItem));

            CreateMap<PedidoComboItemComando, PedidoComboItemCozinhaResponse>()
                .ForMember(dest => dest.ProdutoCodigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Produto.Nome));

        }
    }
}
