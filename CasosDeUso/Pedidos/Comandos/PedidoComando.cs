using CasosDeUso.Clientes.Comandos;

namespace CasosDeUso.Pedidos.Comandos
{
    public class PedidoComando
    {
        public int Codigo { get; set; }
        public ClienteComando? Cliente { get; set; }
        public IList<PedidoComboComando>? PedidoCombo { get; set; }
        public required IList<PedidoStatusComando> PedidoStatus { get; set; }
        public PedidoPagamentoComando? PedidoPagamento { get; set; }

    }
}
