using CasosDeUso.Pedidos.Comandos;
using CasosDeUso.Pedidos.Enums;

namespace CasosDeUso.Pedidos.Interfaces
{
    public interface IPedidoCasosDeUso
    {
        List<PedidoComando> Consultar();
        PedidoComando ConsultarPorCodigo(int codigo);
        List<PedidoComando> ConsultarPedidosCozinha();
        List<PedidoStatusComando> ConsultarPedidosMonitor();
        PedidoComando CadastrarPedido(PedidoComando pedido, string? token);
        PedidoStatusComando ConsultarStatus(int codigo);
        PedidoComando AlterarStatus(int codigo, StatusPedido statusPedido);
        PedidoComando InserirCombo(int codigo, PedidoComboComando pedidoComboComando);
        PedidoComando RemoverCombo(int codigo, int pedidoCombo);
        Task<PedidoComando> ConfirmarPagamento(string codigoPagamento, MetodoPagamentoEnum metodoPagamento);
        Task<string> ConfirmarPedido(int codigo, PedidoPagamentoComando pedidoPagamentoComando, MetodoPagamentoEnum metodoPagamento);
    }
}
