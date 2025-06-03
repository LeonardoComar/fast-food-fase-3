using CasosDeUso.Bases.Interfaces.Gateway;
using CasosDeUso.Pedidos.Comandos;
using CasosDeUso.Pedidos.Enums;
using Entidades.Pedidos;

namespace CasosDeUso.Pedidos.Interfaces.Gateway
{
    public interface IPedidoPagamentoGateway : IBaseGateway<PedidoPagamento>
    {
        Task<string> RealizarPagamento(PedidoComando pedidoComando, MetodoPagamentoEnum metodoPagamento);
        Task<string> ObterPagamento(string codigoPagamento, MetodoPagamentoEnum metodoPagamento);
    }
}
