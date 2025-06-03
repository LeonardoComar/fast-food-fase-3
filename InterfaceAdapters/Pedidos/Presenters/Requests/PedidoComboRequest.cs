namespace InterfaceAdapters.Pedidos.Presenters.Requests
{
    public class PedidoComboRequest
    {
        public required IList<PedidoComboItemRequest> PedidoComboItems { get; set; }
    }
}
