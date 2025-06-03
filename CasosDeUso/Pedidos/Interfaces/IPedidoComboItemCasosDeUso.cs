using Entidades.Pedidos;

namespace CasosDeUso.Pedidos.Interfaces
{
    public interface IPedidoComboItemCasosDeUso
    {
        bool ExisteComboItemComProduto(int codigo);
        PedidoComboItem Inserir(PedidoComboItem pedidoComboItem);
    }
}