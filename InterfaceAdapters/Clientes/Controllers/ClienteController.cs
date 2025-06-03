using AutoMapper;
using CasosDeUso.Clientes.Comandos;
using CasosDeUso.Clientes.Interfaces;
using InterfaceAdapters.Clientes.Controllers.Interfaces;
using InterfaceAdapters.Clientes.Presenters.Requests;
using InterfaceAdapters.Clientes.Presenters.Responses;
using InterfaceAdapters.Transactions.Interfaces;
using NHibernate;

namespace InterfaceAdapters.Clientes.Controllers
{
    public class ClienteController(IMapper mapper, ISession session, IUnitOfWorks unitOfWorks, IClienteCasosDeUso clienteCasosDeUso) : IClienteController
    {
        public ClienteResponse Inserir(ClienteRequest clienteRequest)
        {
            try
            {
                unitOfWorks.Begintransaction();

                var clienteComando = mapper.Map<ClienteComando>(clienteRequest);

                var cliente = clienteCasosDeUso.CadastrarCliente(clienteComando);

                unitOfWorks.Commit();

                return mapper.Map<ClienteResponse>(cliente);
            }
            catch
            {
                unitOfWorks.RollBack();
                throw;
            }

        }

        public List<ClienteResponse> Consultar()
        {
            try
            {
                var clientes = clienteCasosDeUso.Consultar();

                var clienteResponse = mapper.Map<List<ClienteResponse>>(clientes);

                return clienteResponse;
            }
            catch
            {
                throw;
            }
        }

        public ClienteResponse FiltrarClientePorCpf(ClienteFiltroRequest clienteFiltroRequest)
        {
            try
            {
                var cliente = clienteCasosDeUso.FiltrarClientePorCpf(clienteFiltroRequest.Cpf);

                var clienteResponse = mapper.Map<ClienteResponse>(cliente);

                return clienteResponse;
            }
            catch
            {
                throw;
            }
        }
    }
}
