using AutoMapper;
using CasosDeUso.Clientes.Comandos;
using CasosDeUso.Clientes.Interfaces;
using CasosDeUso.Clientes.Interfaces.Gateway;
using Entidades.Clientes;
using Entidades.Util;

namespace CasosDeUso.Clientes
{
    public class ClienteCasoDeUso(IClienteGateway clienteGateway, IMapper mapper) : IClienteCasosDeUso
    {
        public ClienteComando CadastrarCliente(ClienteComando clienteComando)
        {
            var cliente = new Cliente(clienteComando.Cpf, clienteComando.Email, clienteComando.Nome);

            ValidarCliente(cliente);

            clienteGateway.Inserir(cliente);

            return mapper.Map<ClienteComando>(cliente);
        }

        public List<ClienteComando> Consultar()
        {
            var clientes = clienteGateway.Consultar();

            var clientesComando = mapper.Map<List<ClienteComando>>(clientes);

            return clientesComando;
        }

        public ClienteComando ConsultarPorCodigo(int codigo)
        {
            var cliente = clienteGateway.RetornarPorCodigo(codigo) ??
                throw new RegraNegocioException("Cliente não encontrado");

            return mapper.Map<ClienteComando>(cliente); 
        }

        public ClienteComando ConsultarPorToken(string token)
        {
            var cliente = clienteGateway.RetornarPorToken(token);
            return mapper.Map<ClienteComando>(cliente); ;
        }

        public ClienteComando? FiltrarClientePorCpf(string cpf)
        {
            var cliente = clienteGateway.Consultar()
                                         .Where(x => x.Cpf == cpf)
                                         .FirstOrDefault();

            return mapper.Map<ClienteComando>(cliente);
        }

        public void ValidarCliente(Cliente cliente)
        {
            var clienteCadastrado = clienteGateway.Consultar()
                                                   .Where(x => x.Cpf == cliente.Cpf)
                                                   .FirstOrDefault();
            if (clienteCadastrado != null)
                throw new RegraNegocioException("Cpf já cadastrado");

        }
    }
}
