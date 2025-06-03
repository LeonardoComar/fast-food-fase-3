using CasosDeUso.Pedidos.Interfaces;
using CasosDeUso.Pedidos.Interfaces.Gateway;
using Entidades.Pedidos;

namespace CasosDeUso.Pedidos
{
    public class PedidoComboItemCasosDeUso(IPedidoComboItemGateway pedidoComboItemGateway) : IPedidoComboItemCasosDeUso
    {
        public bool ExisteComboItemComProduto(int codigo)
        {
            var existePedido = pedidoComboItemGateway.Query()
                                                     .Any(p => p.Produto.Codigo == codigo);

            return existePedido;
        }

        public PedidoComboItem Inserir(PedidoComboItem pedidoComboItem)
        {
            pedidoComboItemGateway.Inserir(pedidoComboItem);

            return pedidoComboItem;
        }
    }
}
