using InterfaceAdapters.Bases.Gateway;
using NHibernate;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace InterfaceAdapters.Tests.Repository.Bases
{
    public class BaseGatewayTestes
    {
        private readonly ISession _session;
        private readonly BaseGateway<Entidade> _sut;

        public BaseGatewayTestes()
        {
            _session = Substitute.For<ISession>();
            _sut = new BaseGateway<Entidade>(_session);
        }

        [Fact]
        public void Inserir_DeveChamarSaveNoSession()
        {
            var entidade = new Entidade();

            _sut.Inserir(entidade);

            _session.Received(1).Save(entidade);
        }

        [Fact]
        public void Alterar_DeveChamarUpdateNoSession()
        {
            var entidade = new Entidade();

            _sut.Alterar(entidade);

            _session.Received(1).Update(entidade);
        }

        [Fact]
        public void Excluir_DeveChamarDeleteNoSession()
        {
            var entidade = new Entidade();

            _sut.Excluir(entidade);

            _session.Received(1).Delete(entidade);
        }

        [Fact]
        public void Refresh_DeveChamarRefreshNoSession()
        {
            var entidade = new Entidade();

            _sut.Refresh(entidade);

            _session.Received(1).Refresh(entidade);
        }

        [Fact]
        public void RetornarPorCodigo_DeveChamarGetNoSession()
        {
            var entidade = new Entidade();

            var entidadeCodigo = 1;

            _session.Get<Entidade>(entidadeCodigo).Returns(entidade);

            var result = _sut.RetornarPorCodigo(entidadeCodigo);

            _session.Received(1).Get<Entidade>(entidadeCodigo);

            Assert.Equal(entidade, result);
        }

        [Fact]
        public void Consultar_DeveRetornarListaDeEntidades()
        {
            var entidades = new List<Entidade>
            {
                new Entidade(),
                new Entidade()
            };

            _session.Query<Entidade>().Returns(entidades.AsQueryable());

            var result = _sut.Consultar();

            Assert.Equal(2, result.Count);
        }
    }

    public class Entidade
    {
        public int Codigo { get; set; }
    }
}
