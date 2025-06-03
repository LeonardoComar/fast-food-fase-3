using InterfaceAdapters.Pedidos.Enums;

namespace InterfaceAdapters.Pedidos.Presenters.Responses
{
    public class PedidoPagamentoResponse
    {
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public required MetodoPagamentoEnum Metodo { get; set; }
        public string? QrCode { get; set; }
    }
}
