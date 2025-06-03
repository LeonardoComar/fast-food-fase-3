using Entidades.Clientes;
using Entidades.Util;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace Entidades.Tests.Clientes
{
    public class ClienteTestes
    {
        [Fact]
        public void CriarCliente_Valido()
        {
            var sut = Builder<Cliente>.CreateNew()
                .With(c => c.Nome, "João da Silva")
                .With(c => c.Email, "joao@exemplo.com")
                .With(c => c.Cpf, "12345678909")
                .Build();

            sut.Should().NotBeNull();
            sut.Nome.Should().Be("João da Silva");
            sut.Email.Should().Be("joao@exemplo.com");
            sut.Cpf.Should().Be("12345678909");
        }

        [Fact]
        public void SetNome_Valido()
        {
            var sut = Builder<Cliente>.CreateNew().Build();
            sut.SetNome("Maria Silva");

            sut.Nome.Should().Be("Maria Silva");
        }

        [Fact]
        public void SetNome_Invalido_ThrowRegraNegocioException()
        {
            var sut = Builder<Cliente>.CreateNew().Build();
            string nomeInvalido = string.Empty;

            sut.Invoking(x => x.SetNome(nomeInvalido)).Should().Throw<RegraNegocioException>()
                .WithMessage("Tamanho do Nome inválido");
        }

        [Fact]
        public void SetEmail_Valido()
        {
            var sut = Builder<Cliente>.CreateNew().Build();
            sut.SetEmail("maria@exemplo.com");

            sut.Email.Should().Be("maria@exemplo.com");
        }

        [Fact]
        public void SetEmail_Invalido_ThrowRegraNegocioException()
        {
            var sut = Builder<Cliente>.CreateNew().Build();
            string emailInvalido = "invalido-email";

            sut.Invoking(x => x.SetEmail(emailInvalido)).Should().Throw<RegraNegocioException>()
                .WithMessage("E-mail inválido");
        }

        [Fact]
        public void SetCpf_Valido()
        {
            var sut = Builder<Cliente>.CreateNew().Build();
            sut.SetCpf("12345678909");

            sut.Cpf.Should().Be("12345678909");
        }

        [Fact]
        public void SetCpf_Invalido_ThrowRegraNegocioException()
        {
            var sut = Builder<Cliente>.CreateNew().Build();
            string cpfInvalido = "invalido-cpf";

            sut.Invoking(x => x.SetCpf(cpfInvalido)).Should().Throw<RegraNegocioException>()
                .WithMessage("Cpf inválido");
        }

        [Fact]
        public void SetEmail_ExcedeLimite_ThrowRegraNegocioException()
        {
            var sut = Builder<Cliente>.CreateNew().Build();
            string emailInvalido = new string('a', 201) + "@exemplo.com";

            sut.Invoking(x => x.SetEmail(emailInvalido)).Should().Throw<RegraNegocioException>()
                .WithMessage("E-mail inválido");
        }

        [Fact]
        public void SetNome_ExcedeLimite_ThrowRegraNegocioException()
        {
            var sut = Builder<Cliente>.CreateNew().Build();
            string nomeInvalido = new string('a', 151);

            sut.Invoking(x => x.SetNome(nomeInvalido)).Should().Throw<RegraNegocioException>()
                .WithMessage("Tamanho do Nome inválido");
        }
    }
}
