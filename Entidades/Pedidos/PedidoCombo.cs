namespace Entidades.Pedidos
{
    public class PedidoCombo
    {
        public virtual int Codigo { get; protected set; }
        public virtual Pedido Pedido { get; protected set; }
        public virtual IList<PedidoComboItem> PedidoComboItem { get; protected set; }
        public virtual decimal Preco { get; protected set; }

        protected PedidoCombo()
        {
        }

        public PedidoCombo(Pedido pedido)
        {
            Pedido = pedido;
        }

        public virtual void SetPedido(Pedido pedido)
        {
            Pedido = pedido;
        }

        public virtual decimal CalcularPreco()
        {
            var valor = PedidoComboItem.Sum(x => x.Preco);

            return valor;
        }

        public virtual bool IsValid()
        {
            return PedidoComboItem.Count() > 1;
        }
    }
}
