using AutoMapper;
using CasosDeUso.Pedidos.Comandos;
using Entidades.Pedidos;

namespace CasosDeUso.Pedidos.Adapters
{
    public class PedidoAdapter : Profile
    {
        public PedidoAdapter()
        {
            CreateMap<Pedido, PedidoComando>().ReverseMap();

            CreateMap<PedidoCombo, PedidoComboComando>().ReverseMap();

            CreateMap<PedidoComboItem, PedidoComboItemComando>().ReverseMap();

            CreateMap<PedidoStatus, PedidoStatusComando>().ReverseMap();

            CreateMap<PedidoPagamento, PedidoPagamentoComando>().ReverseMap();
        }
    }
}
