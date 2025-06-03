using AutoMapper;
using CasosDeUso.Clientes.Comandos;
using Entidades.Clientes;

namespace CasosDeUso.Clientes.Adapters
{
    public class ClienteAdapter : Profile
    {
        public ClienteAdapter()
        {
            CreateMap<Cliente, ClienteComando>().ReverseMap();
        }
    }
}
