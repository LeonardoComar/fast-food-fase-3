using CasosDeUso.Pedidos.Enums;
using CasosDeUso.Pedidos.Interfaces;
using CasosDeUso.Pedidos.Interfaces.Gateway;
using Entidades.Pedidos;

namespace CasosDeUso.Pedidos
{
    public class PedidoStatusCasosDeUso(IPedidoStatusGateway pedidoStatusGateway) : IPedidoStatusCasosDeUso
    {
        public Pedido Inserir(Pedido pedido, StatusPedido statusPedido)
        {
            var pedidoStatus = new PedidoStatus(pedido, (Entidades.Pedidos.Enums.StatusPedido)statusPedido, DateTime.Now);

            pedidoStatusGateway.Inserir(pedidoStatus);

            return pedido;
        }
    }
}
