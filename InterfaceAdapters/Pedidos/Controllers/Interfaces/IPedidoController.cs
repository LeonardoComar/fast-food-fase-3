using InterfaceAdapters.Pedidos.Presenters.Requests;
using InterfaceAdapters.Pedidos.Presenters.Responses;
using MetodoPagamentoEnum = InterfaceAdapters.Pedidos.Enums.MetodoPagamentoEnum;
using StatusPedido = InterfaceAdapters.Pedidos.Enums.StatusPedido;

namespace InterfaceAdapters.Pedidos.Controllers.Interfaces
{
    public interface IPedidoController
    {
        List<PedidoResponse> Consultar();
        PedidoResponse Consultar(int codigo);
        PedidoStatusResponse ConsultarStatus(int codigo);
        PedidoResponse Inserir(PedidoRequest pedidoRequest, string? token);
        PedidoResponse AlterarStatus(int codigo, StatusPedido statusPedido);
        Task<int> ConfirmarPagamento(string codigoPagamento, MetodoPagamentoEnum metodoPagamento);
        Task<PedidoPagamentoResponse> ConfirmarPedido(int codigo, PedidoPagamentoRequest pedidoPagamentoRequest, MetodoPagamentoEnum metodoPagamento);
        PedidoResponse InserirCombo(int codigo, PedidoComboRequest pedidoComboRequest);
        PedidoResponse RemoverCombo(int codigo, int codigoCombo);
        List<PedidoCozinhaResponse> ObterPedidosCozinha();
        List<PedidoStatusMonitorResponse> ObterPedidosMonitor();
    }

}

