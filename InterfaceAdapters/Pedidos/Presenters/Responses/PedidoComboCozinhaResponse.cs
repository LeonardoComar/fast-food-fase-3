namespace InterfaceAdapters.Pedidos.Presenters.Responses
{
    public class PedidoComboCozinhaResponse
    {
        public int Codigo { get; set; }
        public required IList<PedidoComboItemCozinhaResponse> PedidoComboItem { get; set; }
    }
}
