using CasosDeUso.Clientes.Interfaces.Gateway;
using Entidades.Clientes;
using InterfaceAdapters.Autenticacao.Gateway.Interfaces;
using InterfaceAdapters.Bases.Gateway;
using NHibernate;

namespace InterfaceAdapters.Clientes.Gateway
{
    public class ClienteGateway(IAutenticacaoGateway autenticacaoGateway, ISession session) : BaseGateway<Cliente>(session), IClienteGateway
    {
        public Cliente? RetornarPorToken(string token)
        {
            var cpf = autenticacaoGateway.ObterNomePorTokenAsync(token).Result;
            return Consultar().FirstOrDefault(x => x.Cpf == cpf);
        }
    }
}
