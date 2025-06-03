using CasosDeUso.Clientes.Comandos;

namespace CasosDeUso.Clientes.Interfaces
{
    public interface IClienteCasosDeUso
    {
        public ClienteComando CadastrarCliente(ClienteComando clienteComando);
        public List<ClienteComando> Consultar();
        public ClienteComando ConsultarPorCodigo(int codigo);
        public ClienteComando ConsultarPorToken(string token);
        public ClienteComando? FiltrarClientePorCpf(string cpf);
    }
}
