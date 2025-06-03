using CasosDeUso.Produtos.Interfaces.Gateway;
using Entidades.Produtos;
using InterfaceAdapters.Bases.Gateway;
using NHibernate;

namespace InterfaceAdapters.Produtos.Gateway
{
    public class ProdutoGateway(ISession session) : BaseGateway<Produto>(session), IProdutoGateway
    {
    }
}