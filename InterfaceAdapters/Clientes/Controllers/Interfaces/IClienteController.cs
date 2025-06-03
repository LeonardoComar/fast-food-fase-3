using InterfaceAdapters.Clientes.Presenters.Requests;
using InterfaceAdapters.Clientes.Presenters.Responses;

namespace InterfaceAdapters.Clientes.Controllers.Interfaces
{
    public interface IClienteController
    {
        ClienteResponse Inserir(ClienteRequest clienteRequest);
        List<ClienteResponse> Consultar();
        ClienteResponse FiltrarClientePorCpf(ClienteFiltroRequest clienteFiltroRequest);
    }
}
