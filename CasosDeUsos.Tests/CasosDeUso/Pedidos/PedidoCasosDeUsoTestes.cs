using AutoMapper;
using CasosDeUso.Clientes.Interfaces;
using CasosDeUso.Pedidos;
using CasosDeUso.Pedidos.Comandos;
using CasosDeUso.Pedidos.Interfaces;
using CasosDeUso.Pedidos.Interfaces.Gateway;
using CasosDeUso.Produtos.Interfaces;
using Entidades.Pedidos;
using Entidades.Pedidos.Enums;
using Entidades.Util;
using FizzWare.NBuilder;
using FluentAssertions;
using NSubstitute;

namespace CasosDeUsos.Tests.CasosDeUso.Pedidos
{
    public class PedidoCasosDeUsoTests
    {
        private readonly IPedidoCasosDeUso _sut;
        private readonly IMapper _mapper;
        private readonly IPedidoGateway _pedidoGateway;
        private readonly IPedidoComboGateway _pedidoComboGateway;
        private readonly IPedidoComboItemGateway _pedidoComboItemGateway;
        private readonly IPedidoPagamentoGateway _pedidoPagamentoGateway;
        private readonly IPedidoStatusGateway _pedidoStatusGateway;
        private readonly IClienteCasosDeUso _clienteCasosDeUso;
        private readonly IPedidoComboCasosDeUso _pedidoComboCasosDeUso;
        private readonly IProdutoCasosDeUso _produtoCasosDeUso;
        private readonly IPedidoStatusCasosDeUso _pedidoStatusCasosDeUso;

        public PedidoCasosDeUsoTests()
        {
            _mapper = Substitute.For<IMapper>();
            _pedidoGateway = Substitute.For<IPedidoGateway>();
            _pedidoComboGateway = Substitute.For<IPedidoComboGateway>();
            _pedidoComboItemGateway = Substitute.For<IPedidoComboItemGateway>();
            _pedidoPagamentoGateway = Substitute.For<IPedidoPagamentoGateway>();
            _pedidoStatusGateway = Substitute.For<IPedidoStatusGateway>();
            _clienteCasosDeUso = Substitute.For<IClienteCasosDeUso>();
            _pedidoComboCasosDeUso = Substitute.For<IPedidoComboCasosDeUso>();
            _produtoCasosDeUso = Substitute.For<IProdutoCasosDeUso>();
            _pedidoStatusCasosDeUso = Substitute.For<IPedidoStatusCasosDeUso>();


            _sut = new PedidoCasosDeUso(
                _mapper,
                _pedidoGateway,
                _pedidoComboGateway,
                _pedidoComboItemGateway,
                _pedidoPagamentoGateway,
                _pedidoStatusGateway,
                _clienteCasosDeUso,
                _pedidoComboCasosDeUso,
                _produtoCasosDeUso,
                _pedidoStatusCasosDeUso);
        }
        
        [Fact]
        public void Consultar_ShouldReturnPedidos()
        {
            var pedidos = Builder<Pedido>.CreateListOfSize(5).Build();
            var pedidosComando = Builder<PedidoComando>.CreateListOfSize(5).Build().ToList();

            _pedidoGateway.Consultar().Returns(pedidos);
            _mapper.Map<List<PedidoComando>>(pedidos).Returns(pedidosComando);

            var result = _sut.Consultar();

            _pedidoGateway.Received(1).Consultar();
            _mapper.Received(1).Map<List<PedidoComando>>(pedidos);
        }

        [Fact]
        public void AlterarStatus_ShouldChangeStatus_WhenValid()
        {
            var pedido = Builder<Pedido>.CreateNew()
                .With(p => p.PedidoStatus, new List<PedidoStatus>
                {
                    new PedidoStatus(null, StatusPedido.AguardandoPagamento, DateTime.Now)
                })
                .Build();

            _pedidoGateway.RetornarPorCodigo(1).Returns(pedido);

            var result = _sut.AlterarStatus(1, (global::CasosDeUso.Pedidos.Enums.StatusPedido)StatusPedido.Cancelado);

            _pedidoStatusCasosDeUso.Received(1).Inserir(pedido, (global::CasosDeUso.Pedidos.Enums.StatusPedido)StatusPedido.Cancelado);
        }

        [Fact]
        public void AlterarStatus_ShouldThrowRegraNegocioException_WhenInvalidStatusTransition()
        {
            var pedido = Builder<Pedido>.CreateNew()
                .With(p => p.PedidoStatus, new List<PedidoStatus>
                {
                    new PedidoStatus(null, StatusPedido.Pronto, DateTime.Now)
                })
                .Build();

            _pedidoGateway.RetornarPorCodigo(1).Returns(pedido);

            _sut.Invoking(x => x.AlterarStatus(1, (global::CasosDeUso.Pedidos.Enums.StatusPedido)StatusPedido.Cancelado))
                .Should().Throw<RegraNegocioException>().WithMessage("Pedido já foi pago e não pode ser cancelado");
        }
    }
}
