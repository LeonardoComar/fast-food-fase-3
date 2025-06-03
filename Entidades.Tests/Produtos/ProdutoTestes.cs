using Entidades.Produtos;
using Entidades.Produtos.Enums;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace Entidades.Tests.Produtos
{
    public class ProdutoTestes
    {
        [Fact]
        public void Produto_CriarProduto_ComCodigo_EstaCorreto()
        {
            var sut = Builder<Produto>.CreateNew()
                .With(p => p.Nome, "Produto Teste")
                .With(p => p.Preco, 100.00m)
                .With(p => p.Categoria, CategoriaEnum.Lanche)
                .With(p => p.Situacao, AtivoInativoEnum.Ativo)
                .Build();

            sut.Codigo.Should().BeGreaterThan(0);
            sut.Nome.Should().Be("Produto Teste");
            sut.Preco.Should().Be(100.00m);
            sut.Categoria.Should().Be(CategoriaEnum.Lanche);
            sut.Situacao.Should().Be(AtivoInativoEnum.Ativo);
        }

        [Fact]
        public void Produto_CriarProduto_Descricao_Nula()
        {
            var sut = Builder<Produto>.CreateNew()
                .With(p => p.Nome, "Produto Teste")
                .With(p => p.Descricao, (string?)null)
                .Build();

            sut.Descricao.Should().BeNull();
        }

        [Fact]
        public void Produto_CriarProduto_Descricao_Valida()
        {
            var sut = Builder<Produto>.CreateNew()
                .With(p => p.Nome, "Produto Teste")
                .With(p => p.Descricao, "Descrição do Produto")
                .Build();

            sut.Descricao.Should().Be("Descrição do Produto");
        }

        [Fact]
        public void Produto_CriarProduto_ComPreco_Valido()
        {
            var sut = Builder<Produto>.CreateNew()
                .With(p => p.Preco, 150.00m)
                .Build();

            sut.Preco.Should().Be(150.00m);
        }
    }
}
