using CasosDeUso.Pedidos.Comandos;
using CasosDeUso.Pedidos.Enums;
using CasosDeUso.Pedidos.Interfaces.Gateway;
using Entidades.Pedidos;
using Entidades.Util;
using InterfaceAdapters.Bases.Gateway;
using NHibernate;

namespace InterfaceAdapters.Pedidos.Gateway
{
    public class PedidoPagamentoGateway(ISession session) : BaseGateway<PedidoPagamento>(session), IPedidoPagamentoGateway
    {
        public async Task<string> RealizarPagamento(PedidoComando pedidoComando, MetodoPagamentoEnum metodoPagamento)
        {
            // Implementação simplificada que retorna um código QR simulado
            return await Task.FromResult($"QRCODE-SIMULADO-{pedidoComando.Codigo}-{DateTime.Now:yyyyMMddHHmmss}");
        }

        public async Task<string> ObterPagamento(string codigoPagamento, MetodoPagamentoEnum metodoPagamento)
        {
            // Implementação simplificada que retorna o código de pedido extraído do QR code
            return await Task.FromResult(codigoPagamento.Split('-')[2]);
        }
    }
}
