using AutoMapper;
using CasosDeUso.Clientes.Comandos;
using InterfaceAdapters.Clientes.Presenters.Requests;
using InterfaceAdapters.Clientes.Presenters.Responses;

namespace InterfaceAdapters.Clientes.Adapters
{
    public class ClienteComandoAdapter : Profile
    {
        public ClienteComandoAdapter()
        {
            CreateMap<ClienteComando, ClienteResponse>();

            CreateMap<ClienteRequest, ClienteComando>();

        }
    }
}
