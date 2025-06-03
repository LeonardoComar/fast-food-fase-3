using Entidades.Pedidos.Enums;

namespace Entidades.Pedidos
{
    public class PedidoPagamento
    {
        public virtual int Codigo { get; protected set; }
        public virtual Pedido Pedido { get; protected set; }
        public virtual decimal Valor { get; protected set; }
        public virtual DateTime DataPagamento { get; protected set; }
        public virtual MetodoPagamentoEnum Metodo { get; protected set; } = MetodoPagamentoEnum.Pix;

        protected PedidoPagamento()
        {
        }

        public PedidoPagamento(Pedido pedido, decimal valor, DateTime dataPagamento, MetodoPagamentoEnum metodo)
        {
            Pedido = pedido;
            Valor = valor;
            DataPagamento = dataPagamento;
            Metodo = metodo;
        }

        public virtual void SetPedido(Pedido pedido)
        {
            Pedido = pedido;
        }
    }
}
