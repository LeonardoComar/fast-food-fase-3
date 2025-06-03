using Entidades.Pedidos;
using FizzWare.NBuilder;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Entidades.Tests.Pedidos
{
    public class PedidoComboTestes
    {
        [Fact]
        public void SetPedido_ClienteValido_ClienteDeveSerDefinido()
        {
            var pedido = Builder<Pedido>.CreateNew().Build();
            var sut = Builder<PedidoCombo>.CreateNew().Build();

            sut.SetPedido(pedido);

            sut.Pedido.Should().Be(pedido);
        }

        [Fact]
        public void CalcularPreco_PedidoComboItemValido_DeveRetornarPrecoCorreto()
        {
            var pedidoComboItem1 = Builder<PedidoComboItem>.CreateNew().With(c => c.Preco, 10.00m).Build();
            var pedidoComboItem2 = Builder<PedidoComboItem>.CreateNew().With(c => c.Preco, 20.00m).Build();

            var sut = Builder<PedidoCombo>.CreateNew()
                .With(c => c.PedidoComboItem, new List<PedidoComboItem> { pedidoComboItem1, pedidoComboItem2 })
                .Build();

            var total = sut.CalcularPreco();

            total.Should().Be(30.00m);
        }

        [Fact]
        public void IsValid_PedidoComboItemComMenosDeDoisItems_DeveRetornarFalso()
        {
            var sut = Builder<PedidoCombo>.CreateNew()
                .With(c => c.PedidoComboItem, new List<PedidoComboItem> {
                                                                           new PedidoComboItem()
                                                                        })
                .Build();

            sut.IsValid().Should().BeFalse();
        }

        [Fact]
        public void IsValid_PedidoComboItemComDoisItems_DeveRetornarVerdadeiro()
        {
            var sut = Builder<PedidoCombo>.CreateNew()
                .With(c => c.PedidoComboItem, new List<PedidoComboItem> {
                                                                            new PedidoComboItem(),
                                                                            new PedidoComboItem()
                                                                        })
                .Build();

            sut.IsValid().Should().BeTrue();
        }
    }
}
