using Entidades.Pedidos;
using Entidades.Pedidos.Enums;
using FizzWare.NBuilder;
using FluentAssertions;
using System;
using Xunit;

namespace Entidades.Tests.Pedidos
{
    public class PedidoStatusTestes
    {
        [Fact]
        public void Constructor_QuandoCriado_DeveDefinirStatusPorPadrao()
        {
            var sut = Builder<PedidoStatus>.CreateNew()
                .With(ps => ps.Status, StatusPedido.AguardandoPagamento)
                .Build();

            sut.Status.Should().Be(StatusPedido.AguardandoPagamento);
        }

        [Fact]
        public void Constructor_QuandoCriado_DeveDefinirDataCriacaoPorPadrao()
        {
            var sut = Builder<PedidoStatus>.CreateNew()
                .With(ps => ps.DataCriacao, DateTime.Now)
                .Build();

            sut.DataCriacao.Date.Should().Be(DateTime.Now.Date);
        }

        [Fact]
        public void Constructor_QuandoRecebePedido_StatusEDataCriacaoDevemSerDefinidosCorretamente()
        {
            var pedido = Builder<Pedido>.CreateNew().Build();
            var status = StatusPedido.Pronto;
            var dataCriacao = DateTime.Now.AddHours(-1);

            var sut = new PedidoStatus(pedido, status, dataCriacao);

            sut.Pedido.Should().Be(pedido);
            sut.Status.Should().Be(status);
            sut.DataCriacao.Should().Be(dataCriacao);
        }
    }
}
