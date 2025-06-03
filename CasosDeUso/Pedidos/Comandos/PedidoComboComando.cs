namespace CasosDeUso.Pedidos.Comandos
{
    public class PedidoComboComando
    {
        public int Codigo { get; set; }
        public required IList<PedidoComboItemComando> PedidoComboItem { get; set; }
        public decimal Preco { get; set; }
    }
}
