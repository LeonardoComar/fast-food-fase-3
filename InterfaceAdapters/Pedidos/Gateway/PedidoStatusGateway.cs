using CasosDeUso.Pedidos.Interfaces.Gateway;
using Entidades.Pedidos;
using InterfaceAdapters.Bases.Gateway;
using NHibernate;

namespace InterfaceAdapters.Pedidos.Gateway
{

    public class PedidoStatusGateway(ISession session) : BaseGateway<PedidoStatus>(session), IPedidoStatusGateway
    {
    }

}
