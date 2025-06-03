using Entidades.Pedidos;
using Entidades.Pedidos.Enums;
using FizzWare.NBuilder;
using FluentAssertions;
using System;
using Xunit;

namespace Entidades.Tests.Pedidos
{
    public class PedidoPagamentoTestes
    {
        [Fact]
        public void Constructor_QuandoCriado_DeveDefinirMetodoPagamentoPorPadrao()
        {
            var sut = Builder<PedidoPagamento>.CreateNew().Build();

            sut.Metodo.Should().Be(MetodoPagamentoEnum.Pix);
        }

        [Fact]
        public void SetPedido_QuandoDefinido_DeveDefinirPedidoCorretamente()
        {
            var pedido = Builder<Pedido>.CreateNew().Build();
            var sut = Builder<PedidoPagamento>.CreateNew().Build();

            sut.SetPedido(pedido);

            sut.Pedido.Should().Be(pedido);
        }

        [Fact]
        public void SetValor_QuandoDefinido_DeveDefinirValorCorretamente()
        {
            var sut = Builder<PedidoPagamento>.CreateNew()
                .With(p => p.Valor, 100.00m)
                .Build();

            sut.Valor.Should().Be(100.00m);
        }

        [Fact]
        public void SetDataPagamento_QuandoDefinido_DeveDefinirDataPagamentoCorretamente()
        {
            var dataPagamento = DateTime.Now;

            var sut = Builder<PedidoPagamento>.CreateNew()
                .With(p => p.DataPagamento, dataPagamento)
                .Build();

            sut.DataPagamento.Should().Be(dataPagamento);
        }
    }
}
