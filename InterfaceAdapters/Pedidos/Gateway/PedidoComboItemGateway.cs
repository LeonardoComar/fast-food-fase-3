using CasosDeUso.Pedidos.Interfaces.Gateway;
using Entidades.Pedidos;
using InterfaceAdapters.Bases.Gateway;
using NHibernate;

namespace InterfaceAdapters.Pedidos.Gateway
{

    public class PedidoComboItemGateway(ISession session) : BaseGateway<PedidoComboItem>(session), IPedidoComboItemGateway
    {
    }

}
