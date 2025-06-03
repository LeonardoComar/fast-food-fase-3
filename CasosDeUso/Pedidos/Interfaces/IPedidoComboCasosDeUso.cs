using Entidades.Pedidos;

namespace CasosDeUso.Pedidos.Interfaces
{
    public interface IPedidoComboCasosDeUso
    {
        PedidoCombo Inserir(PedidoCombo pedidoCombo);
        PedidoCombo Inserir(int codigo, PedidoCombo pedidoCombo);
        void Remover(int pedidoCombo);
    }
}