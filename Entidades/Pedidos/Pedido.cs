using Entidades.Clientes;

namespace Entidades.Pedidos
{
    public class Pedido
    {
        public virtual int Codigo { get; protected set; }
        public virtual Cliente? Cliente { get; protected set; }
        public virtual IList<PedidoCombo>? PedidoCombo { get; protected set; }
        public virtual IList<PedidoStatus> PedidoStatus { get; protected set; }
        public virtual PedidoPagamento? PedidoPagamento { get; protected set; }

        protected Pedido()
        {
        }

        public Pedido(Cliente? cliente)
        {
            if (cliente != null)
                SetCliente(cliente);
        }
        public virtual void SetCliente(Cliente cliente)
        {
            Cliente = cliente;
        }

        public virtual decimal CalcularTotal()
        {
            if (PedidoCombo != null)
            {
                var valor = PedidoCombo.Sum(c => c.CalcularPreco());

                return valor;
            }

            return 0;
        }

    }
}
