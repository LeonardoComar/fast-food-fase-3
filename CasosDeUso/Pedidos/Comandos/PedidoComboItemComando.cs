using CasosDeUso.Produtos.Comandos;

namespace CasosDeUso.Pedidos.Comandos
{
    public class PedidoComboItemComando
    {
        public int? Codigo { get; set; }
        public int CodigoProduto { get; set; }
        public required ProdutoComando Produto { get; set; }
        public decimal Preco { get; set; }

    }
}
