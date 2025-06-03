using Entidades.Clientes;
using Entidades.Pedidos;
using FizzWare.NBuilder;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Entidades.Tests.Pedidos
{
    public class PedidoTestes
    {
        [Fact]
        public void CriarPedido_Valido()
        {
            var cliente = Builder<Cliente>.CreateNew()
                .With(c => c.Nome, "João da Silva")
                .With(c => c.Email, "joao@exemplo.com")
                .With(c => c.Cpf, "12345678909")
                .Build();

            var sut = Builder<Pedido>.CreateNew()
                .With(p => p.Cliente, cliente)
                .Build();

            sut.Should().NotBeNull();
            sut.Cliente.Should().Be(cliente);
        }

        [Fact]
        public void CalcularTotal_SemPedidoCombo_RetornaZero()
        {
            var sut = Builder<Pedido>.CreateNew()
                .With(p => p.PedidoCombo, new List<PedidoCombo>())
                .Build();

            var total = sut.CalcularTotal();

            total.Should().Be(0);
        }

        [Fact]
        public void CriarPedido_ComCliente_NaoNulo()
        {
            var cliente = Builder<Cliente>.CreateNew()
                .With(c => c.Nome, "Maria Silva")
                .Build();

            var sut = new Pedido(cliente);

            sut.Cliente.Should().Be(cliente);
        }
    }
}
