using CasosDeUso.Pedidos.Enums;
using Entidades.Pedidos;

namespace CasosDeUso.Pedidos.Interfaces
{
    public interface IPedidoStatusCasosDeUso
    {
        Pedido Inserir(Pedido pedido, StatusPedido statusPedido);
    }
}