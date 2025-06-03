using InterfaceAdapters.Clientes.Controllers.Interfaces;
using InterfaceAdapters.Clientes.Presenters.Requests;
using InterfaceAdapters.Clientes.Presenters.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Clientes
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController(IClienteController clienteController) : ControllerBase
    {
        [HttpPost]
        public ActionResult<ClienteResponse> Inserir(ClienteRequest clienteRequest)
        {
            return clienteController.Inserir(clienteRequest);
        }

        [HttpGet]
        public ActionResult<List<ClienteResponse>> Consultar()
        {
            return clienteController.Consultar();
        }

        [HttpGet]
        [Route("filtrar")]
        public ActionResult<ClienteResponse> Consultar([FromQuery] ClienteFiltroRequest clienteFiltroRequest)
        {
            return clienteController.FiltrarClientePorCpf(clienteFiltroRequest);
        }

    }

}
