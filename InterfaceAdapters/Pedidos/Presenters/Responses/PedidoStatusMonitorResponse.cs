using InterfaceAdapters.Pedidos.Enums;

namespace InterfaceAdapters.Pedidos.Presenters.Responses
{
    public class PedidoStatusMonitorResponse
    {
        public int PedidoCodigo { get; set; }
        public StatusPedido Status { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
