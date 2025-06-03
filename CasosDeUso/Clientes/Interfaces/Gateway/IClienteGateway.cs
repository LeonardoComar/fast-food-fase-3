using CasosDeUso.Bases.Interfaces.Gateway;
using Entidades.Clientes;

namespace CasosDeUso.Clientes.Interfaces.Gateway
{
    public interface IClienteGateway : IBaseGateway<Cliente>
    {
        Cliente RetornarPorToken(string token);
    }
}
