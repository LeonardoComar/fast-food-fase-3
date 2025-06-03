using CasosDeUso.Pedidos.Enums;

namespace CasosDeUso.Pedidos.Comandos
{
    public class PedidoPagamentoComando
    {
        public int Codigo { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public MetodoPagamentoEnum Metodo { get; set; }
    }
}
