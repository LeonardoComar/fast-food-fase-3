using CasosDeUso.Pedidos.Interfaces;
using Infra.Autenticacao.Gateway;
using Infra.Pagamento;
using Infra.Transactions;
using InterfaceAdapters.Autenticacao.Gateway.Interfaces;
using InterfaceAdapters.Transactions.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class InfraBootstrapper
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IUnitOfWorks, UnitOfWorks>();
        services.AddTransient<IAutenticacaoGateway, AutenticacaoGateway>();
        services.AddTransient<IPagamentoService, PagamentoSimplificadoService>();

        services.AddNHibernate(configuration);
    }
}
