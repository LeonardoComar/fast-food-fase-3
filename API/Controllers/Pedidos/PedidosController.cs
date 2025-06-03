using InterfaceAdapters.Pedidos.Controllers.Interfaces;
using InterfaceAdapters.Pedidos.Enums;
using InterfaceAdapters.Pedidos.Presenters.Requests;
using InterfaceAdapters.Pedidos.Presenters.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Pedidos
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController(IPedidoController pedidoController) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IList<PedidoResponse>> Consultar()
        {
            return pedidoController.Consultar();
        }

        [HttpGet]
        [Route("{codigo}")]
        public ActionResult<PedidoResponse> Consultar(int codigo)
        {
            return pedidoController.Consultar(codigo);
        }

        [HttpGet]
        [Route("Status/{codigo}")]
        public ActionResult<PedidoStatusResponse> ConsultarStatus(int codigo)
        {
            return pedidoController.ConsultarStatus(codigo);
        }

        [HttpGet]
        [Route("Preparar")]
        public ActionResult<IList<PedidoCozinhaResponse>> ConsultarPedidoCozinha()
        {
            return pedidoController.ObterPedidosCozinha();
        }

        [HttpGet]
        [Route("Monitor")]
        public ActionResult<IList<PedidoStatusMonitorResponse>> ConsultarPedidoMonitor()
        {
            return pedidoController.ObterPedidosMonitor();
        }

        [HttpPost]
        public ActionResult<int> Inserir(PedidoRequest pedidoRequest)
        {
            string? token = null;

            if (Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues value))
            {
                token = value.ToString().Replace("Bearer ", "");
            }

            return pedidoController.Inserir(pedidoRequest, token).Codigo;
        }

        [HttpPut]
        [Route("Cancelar/{codigo}")]
        public ActionResult<PedidoResponse> Cancelar(int codigo)
        {
            return pedidoController.AlterarStatus(codigo, StatusPedido.Cancelado);
        }

        [HttpPut]
        [Route("AlterarStatus/{codigo}")]
        public ActionResult<PedidoResponse> AlterarStatus(int codigo, StatusPedido statusPedido)
        {
            return pedidoController.AlterarStatus(codigo, statusPedido);
        }

        [HttpPut]
        [Route("Finalizar/{codigo}")]
        public ActionResult<PedidoResponse> Finalizar(int codigo)
        {

            return pedidoController.AlterarStatus(codigo, StatusPedido.Finalizado);
        }

        [HttpPut]
        [Route("InserirCombo/{codigo}")]
        public ActionResult<PedidoResponse> InserirCombo(int codigo, PedidoComboRequest pedidoComboRequest)
        {
            return pedidoController.InserirCombo(codigo, pedidoComboRequest);
        }

        [HttpDelete]
        [Route("RemoverCombo/{codigo}/{CodigoCombo}")]
        public ActionResult<PedidoResponse> RemoverCombo(int codigo, int codigoCombo)
        {

            return pedidoController.RemoverCombo(codigo, codigoCombo);
        }

        [HttpPost]
        [Route("ConfirmarPedido/{codigo}")]
        public async Task<ActionResult<PedidoPagamentoResponse>> ConfirmarPedido(int codigo, PedidoPagamentoRequest pedidoPagamentoRequest, bool usarCartao = false)
        {
            return await pedidoController.ConfirmarPedido(codigo, pedidoPagamentoRequest, usarCartao ? MetodoPagamentoEnum.CartaoCredito : MetodoPagamentoEnum.Pix);
        }

        [HttpPost]
        [Route("webhook/ConfirmarPagamento")]
        public async Task<int> ConfirmarPagamento([FromBody] dynamic pagamentoRequest, bool usarCartao = false)
        {
            // Extrair o ID do pedido da solicitação
            string id = pagamentoRequest.GetProperty("id").GetString();
            return await pedidoController.ConfirmarPagamento(id, usarCartao ? MetodoPagamentoEnum.CartaoCredito : MetodoPagamentoEnum.Pix);
        }
    }
}
