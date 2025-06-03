using Entidades.Pedidos;
using Entidades.Produtos;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace Entidades.Tests.Pedidos
{
    public class PedidoComboItemTestes
    {
        [Fact]
        public void Constructor_QuandoCriado_ComProduto_DeveDefinirPrecoCorretamente()
        {
            var produto = Builder<Produto>.CreateNew()
                .With(p => p.Preco, 15.50m)
                .Build();

            var pedidoCombo = Builder<PedidoCombo>.CreateNew().Build();

            var sut = new PedidoComboItem(pedidoCombo, produto);

            sut.Preco.Should().Be(15.50m);
            sut.Produto.Should().Be(produto);
            sut.PedidoCombo.Should().Be(pedidoCombo);
        }
    }
}
