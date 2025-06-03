using InterfaceAdapters.Clientes.Presenters.Responses;

namespace InterfaceAdapters.Pedidos.Presenters.Responses
{
    public class PedidoResponse
    {
        public int Codigo { get; set; }
        public ClienteResponse? Cliente { get; set; }
        public IList<PedidoComboResponse>? PedidoCombo { get; set; }
        public required PedidoStatusResponse PedidoStatus { get; set; }
        public PedidoPagamentoResponse? PedidoPagamento { get; set; }
    }
}
