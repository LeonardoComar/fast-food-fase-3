using AutoMapper;
using CasosDeUso.Pedidos.Interfaces;
using CasosDeUso.Produtos;
using CasosDeUso.Produtos.Comandos;
using CasosDeUso.Produtos.Interfaces;
using CasosDeUso.Produtos.Interfaces.Gateway;
using Entidades.Produtos;
using Entidades.Util;
using FizzWare.NBuilder;
using FluentAssertions;
using NSubstitute;

namespace CasosDeUsos.Tests.CasosDeUso.Produtos
{
    public class ProdutoCasosDeUsoTests
    {
        private readonly IProdutoCasosDeUso _sut;
        private readonly IMapper _mapper;
        private readonly IProdutoGateway _produtoGateway;
        private readonly IPedidoComboItemCasosDeUso _pedidoComboItemCasosDeUso;

        public ProdutoCasosDeUsoTests()
        {
            _mapper = Substitute.For<IMapper>();
            _produtoGateway = Substitute.For<IProdutoGateway>();
            _pedidoComboItemCasosDeUso = Substitute.For<IPedidoComboItemCasosDeUso>();

            _sut = new ProdutoCasosDeUso(_mapper, _produtoGateway, _pedidoComboItemCasosDeUso);
        }

        [Fact]
        public void AlterarProdutos_ShouldReturnAlteredProdutoComando()
        {
            var produtoComando = Builder<ProdutoComando>.CreateNew().Build();
            var produto = Builder<Produto>.CreateNew().Build();


            _produtoGateway.RetornarPorCodigo(produtoComando.Codigo).Returns(produto);
            _mapper.Map<Produto>(produtoComando).Returns(produto);
            _mapper.Map<ProdutoComando>(produto).Returns(produtoComando);

            var result = _sut.AlterarProdutos(produtoComando);

            _produtoGateway.Received(1).Alterar(produto);
            _mapper.Received(1).Map<ProdutoComando>(produto);
            result.Should().BeEquivalentTo(produtoComando);
        }

        [Fact]
        public void Excluir_ShouldThrowRegraNegocioException_WhenProdutoHasDependencias()
        {
            var produto = Builder<Produto>.CreateNew().Build();
            _produtoGateway.RetornarPorCodigo(1).Returns(produto);
            _pedidoComboItemCasosDeUso.ExisteComboItemComProduto(1).Returns(true);

            _sut.Invoking(x => x.Excluir(1)).Should().Throw<RegraNegocioException>();
        }

        [Fact]
        public void Excluir_ShouldRemoveProduto_WhenProdutoHasNoDependencias()
        {
            var produto = Builder<Produto>.CreateNew().Build();
            _produtoGateway.RetornarPorCodigo(1).Returns(produto);
            _pedidoComboItemCasosDeUso.ExisteComboItemComProduto(1).Returns(false);

            _sut.Excluir(1);

            _produtoGateway.Received(1).Excluir(produto);
        }

        [Fact]
        public void CadastraProduto_ShouldReturnProdutoComando_WhenProdutoIsCadastrado()
        {
            var produtoComando = Builder<ProdutoComando>.CreateNew().Build();
            var produto = Builder<Produto>.CreateNew().Build();

            _mapper.Map<Produto>(produtoComando).Returns(produto);
            _mapper.Map<ProdutoComando>(produto).Returns(produtoComando);

            var result = _sut.CadastraProduto(produtoComando);

            _produtoGateway.Received(1).Inserir(produto);
            _mapper.Received(1).Map<ProdutoComando>(produto);
            result.Should().BeEquivalentTo(produtoComando);
        }

        [Fact]
        public void ConsultarPorCodigo_ShouldThrowRegraNegocioException_WhenProdutoNotFound()
        {
            _produtoGateway.RetornarPorCodigo(1).Returns((Produto)null);

            _sut.Invoking(x => x.ConsultarPorCodigo(1)).Should().Throw<RegraNegocioException>();
        }

        [Fact]
        public void ConsultarPorCodigo_ShouldReturnProdutoComando_WhenProdutoFound()
        {
            var produto = Builder<Produto>.CreateNew().Build();
            var produtoComando = Builder<ProdutoComando>.CreateNew().Build();

            _produtoGateway.RetornarPorCodigo(1).Returns(produto);
            _mapper.Map<ProdutoComando>(produto).Returns(produtoComando);

            var result = _sut.ConsultarPorCodigo(1);

            _produtoGateway.Received(1).RetornarPorCodigo(1);
            _mapper.Received(1).Map<ProdutoComando>(produto);
            result.Should().BeEquivalentTo(produtoComando);
        }
    }
}
