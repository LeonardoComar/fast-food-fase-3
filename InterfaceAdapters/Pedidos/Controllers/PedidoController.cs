using AutoMapper;
using CasosDeUso.Pedidos.Comandos;
using CasosDeUso.Pedidos.Interfaces;
using InterfaceAdapters.Pedidos.Controllers.Interfaces;
using InterfaceAdapters.Pedidos.Enums;
using InterfaceAdapters.Pedidos.Presenters.Requests;
using InterfaceAdapters.Pedidos.Presenters.Responses;
using InterfaceAdapters.Transactions.Interfaces;
using StatusPedido = InterfaceAdapters.Pedidos.Enums.StatusPedido;

namespace InterfaceAdapters.Pedidos.Controllers
{
    public class PedidoController(IMapper mapper, IUnitOfWorks unitOfWorks, IPedidoCasosDeUso pedidoCasosDeUso) : IPedidoController
    {
        public List<PedidoResponse> Consultar()
        {
            try
            {
                var pedidos = pedidoCasosDeUso.Consultar();

                var pedidoResponse = mapper.Map<List<PedidoResponse>>(pedidos);

                return pedidoResponse;
            }
            catch
            {
                throw;
            }
        }

        public PedidoResponse Consultar(int codigo)
        {
            try
            {
                var pedido = pedidoCasosDeUso.ConsultarPorCodigo(codigo);

                var pedidoResponse = mapper.Map<PedidoResponse>(pedido);

                return pedidoResponse;
            }
            catch
            {
                throw;
            }
        }

        public PedidoStatusResponse ConsultarStatus(int codigo)
        {
            try
            {
                var pedidoStatus = pedidoCasosDeUso.ConsultarStatus(codigo);

                var pedidoStatusResponse = mapper.Map<PedidoStatusResponse>(pedidoStatus);

                return pedidoStatusResponse;
            }
            catch
            {
                throw;
            }
        }

        public List<PedidoCozinhaResponse> ObterPedidosCozinha()
        {
            var pedidosComando = pedidoCasosDeUso.ConsultarPedidosCozinha();

            if (pedidosComando.Count > 0)
            {
                return mapper.Map<List<PedidoCozinhaResponse>>(pedidosComando);
            }

            return [];
        }

        public List<PedidoStatusMonitorResponse> ObterPedidosMonitor()
        {
            var pedidos = pedidoCasosDeUso.ConsultarPedidosMonitor();

            if (pedidos.Count > 0)
            {
                return mapper.Map<List<PedidoStatusMonitorResponse>>(pedidos);
            }

            return [];
        }

        public PedidoResponse Inserir(PedidoRequest pedidoRequest, string? token)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var pedido = mapper.Map<PedidoComando>(pedidoRequest);


                pedido = pedidoCasosDeUso.CadastrarPedido(pedido, token);

                unitOfWorks.Commit();

                var pedidoResponse = mapper.Map<PedidoResponse>(pedido);

                return pedidoResponse;
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }

        public PedidoResponse AlterarStatus(int codigo, StatusPedido statusPedido)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var pedido = pedidoCasosDeUso.AlterarStatus(codigo, (CasosDeUso.Pedidos.Enums.StatusPedido)statusPedido);

                unitOfWorks.Commit();

                return mapper.Map<PedidoResponse>(pedido);
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }

        public PedidoResponse InserirCombo(int codigo, PedidoComboRequest pedidoComboRequest)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var pedidoComboComando = mapper.Map<PedidoComboComando>(pedidoComboRequest);

                var pedido = pedidoCasosDeUso.InserirCombo(codigo, pedidoComboComando);

                unitOfWorks.Commit();

                return mapper.Map<PedidoResponse>(pedido);
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }

        public PedidoResponse RemoverCombo(int codigo, int codigoCombo)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var pedido = pedidoCasosDeUso.RemoverCombo(codigo, codigoCombo);

                unitOfWorks.Commit();

                return mapper.Map<PedidoResponse>(pedido);
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }

        public async Task<int> ConfirmarPagamento(string codigoPagamento, MetodoPagamentoEnum metodoPagamento)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var pedido = await pedidoCasosDeUso.ConfirmarPagamento(codigoPagamento, (CasosDeUso.Pedidos.Enums.MetodoPagamentoEnum)metodoPagamento);

                unitOfWorks.Commit();

                return pedido.Codigo;
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }

        public async Task<PedidoPagamentoResponse> ConfirmarPedido(int codigo, PedidoPagamentoRequest pedidoPagamentoRequest, MetodoPagamentoEnum metodoPagamento)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var pedidoPagamentoComando = mapper.Map<PedidoPagamentoComando>(pedidoPagamentoRequest);

                var QrCode = await pedidoCasosDeUso.ConfirmarPedido(codigo, pedidoPagamentoComando, (CasosDeUso.Pedidos.Enums.MetodoPagamentoEnum)metodoPagamento);

                unitOfWorks.Commit();

                var pedidoPagamentoResponse = mapper.Map<PedidoPagamentoResponse>(pedidoPagamentoComando);

                pedidoPagamentoResponse.Metodo = metodoPagamento;

                pedidoPagamentoResponse.QrCode = QrCode;

                return pedidoPagamentoResponse;
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }
        }
    }
}
