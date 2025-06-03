using InterfaceAdapters.Pedidos.Enums;

namespace InterfaceAdapters.Pedidos.Presenters.Responses
{
    public class PedidoStatusResponse
    {
        public StatusPedido Status { get; protected set; }
        public DateTime DataCriacao { get; protected set; }
    }
}
