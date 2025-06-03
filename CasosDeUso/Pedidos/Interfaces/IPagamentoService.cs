using CasosDeUso.Pedidos.Comandos;
using CasosDeUso.Pedidos.Enums;

namespace CasosDeUso.Pedidos.Interfaces
{
    public interface IPagamentoService
    {
        Task<string> GerarCodigoPagamento(PedidoComando pedidoComando, MetodoPagamentoEnum metodoPagamento);
        Task<string> ValidarPagamento(string codigoPagamento, MetodoPagamentoEnum metodoPagamento);
    }
}
