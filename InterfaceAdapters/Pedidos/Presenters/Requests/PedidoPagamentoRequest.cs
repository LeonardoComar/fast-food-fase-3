namespace InterfaceAdapters.Pedidos.Presenters.Requests
{
    public class PedidoPagamentoRequest
    {
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}
