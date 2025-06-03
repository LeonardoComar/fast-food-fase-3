namespace InterfaceAdapters.Pedidos.Presenters.Responses
{
    public class PedidoComboResponse
    {
        public int Codigo { get; set; }
        public required IList<PedidoComboItemResponse> PedidoComboItem { get; set; }
        public decimal Preco { get; set; }
    }
}
