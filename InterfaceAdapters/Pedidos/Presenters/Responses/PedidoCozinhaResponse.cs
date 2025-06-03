using InterfaceAdapters.Pedidos.Enums;

namespace InterfaceAdapters.Pedidos.Presenters.Responses
{
    public class PedidoCozinhaResponse
    {
        public int Pedido { get; set; }
        public StatusPedido StatusPedido { get; set; }
        public DateTime DataCriacao { get; set; }
        public TimeSpan TempoEmPreparo { get; set; }
        public required IList<PedidoComboCozinhaResponse> PedidoCombo { get; set; }
    }
}
