using AutoMapper;
using CasosDeUso.Clientes;
using CasosDeUso.Clientes.Comandos;
using CasosDeUso.Clientes.Interfaces.Gateway;
using Entidades.Clientes;
using Entidades.Util;
using FluentAssertions;
using NSubstitute;

namespace CasosDeUsos.Tests.CasosDeUso.Clientes
{
    public class ClienteCasoDeUsoTests
    {
        private readonly IMapper _mapper;
        private readonly IClienteGateway _clienteGateway;
        private readonly ClienteCasoDeUso _sut;

        public ClienteCasoDeUsoTests()
        {
            _mapper = Substitute.For<IMapper>();
            _clienteGateway = Substitute.For<IClienteGateway>();
            _sut = new ClienteCasoDeUso(_clienteGateway, _mapper);
        }

        [Fact]
        public void CadastrarCliente_DeveLancarRegraNegocioException_QuandoCpfJaEstiverCadastrado()
        {
            var clienteComando = new ClienteComando
            {
                Cpf = "12345678909",
                Nome = "Teste",
                Email = "teste@email.com"
            };

            var clienteExistente = new Cliente("12345678909", "existente@email.com", "Existente");

            _clienteGateway.Consultar().Returns(new List<Cliente> { clienteExistente });

            var acao = () => _sut.CadastrarCliente(clienteComando);

            acao.Should().Throw<RegraNegocioException>()
                .WithMessage("Cpf já cadastrado");
            _clienteGateway.DidNotReceive().Inserir(Arg.Any<Cliente>());
        }

        [Fact]
        public void ConsultarPorCodigo_DeveLancarRegraNegocioException_QuandoClienteNaoExistir()
        {
            _clienteGateway.RetornarPorCodigo(Arg.Any<int>()).Returns((Cliente?)null);

            var acao = () => _sut.ConsultarPorCodigo(1);

            acao.Should().Throw<RegraNegocioException>()
                .WithMessage("Cliente não encontrado");
        }

        [Fact]
        public void ConsultarPorCodigo_DeveRetornarClienteComando_QuandoClienteExistir()
        {
            var cliente = new Cliente("12345678909", "email@email.com", "Cliente");
            var clienteComando = new ClienteComando { Cpf = "12345678909", Email = "email@email.com", Nome = "Cliente" };

            _clienteGateway.RetornarPorCodigo(1).Returns(cliente);
            _mapper.Map<ClienteComando>(cliente).Returns(clienteComando);

            var resultado = _sut.ConsultarPorCodigo(1);

            resultado.Should().BeEquivalentTo(clienteComando);
        }

        [Fact]
        public void FiltrarClientePorCpf_DeveRetornarNull_QuandoCpfNaoExistir()
        {
            _clienteGateway.Consultar().Returns(new List<Cliente>());

            var resultado = _sut.FiltrarClientePorCpf("12345678909");

            resultado.Should().BeNull();
        }

        [Fact]
        public void FiltrarClientePorCpf_DeveRetornarClienteComando_QuandoCpfExistir()
        {
            var cliente = new Cliente("12345678909", "email@email.com", "Cliente");
            var clienteComando = new ClienteComando { Cpf = "12345678909", Email = "email@email.com", Nome = "Cliente" };

            _clienteGateway.Consultar().Returns(new List<Cliente> { cliente });
            _mapper.Map<ClienteComando>(cliente).Returns(clienteComando);

            var resultado = _sut.FiltrarClientePorCpf("12345678909");

            resultado.Should().BeEquivalentTo(clienteComando);
        }
    }
}
