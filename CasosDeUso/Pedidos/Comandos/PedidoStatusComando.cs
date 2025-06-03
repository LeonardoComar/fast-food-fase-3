using CasosDeUso.Pedidos.Enums;

namespace CasosDeUso.Pedidos.Comandos
{
    public class PedidoStatusComando
    {
        public int Codigo { get; set; }
        public int PedidoCodigo { get; set; }
        public StatusPedido Status { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
