using CasosDeUso.Clientes;
using CasosDeUso.Clientes.Interfaces;
using CasosDeUso.Pedidos;
using CasosDeUso.Pedidos.Interfaces;
using CasosDeUso.Produtos;
using CasosDeUso.Produtos.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CasosDeUso;

public static class CasosDeUsoBootstrapper
{
    public static void Register(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CasosDeUsoBootstrapper));
        services.AddTransient<IClienteCasosDeUso, ClienteCasoDeUso>();
        services.AddTransient<IProdutoCasosDeUso, ProdutoCasosDeUso>();
        services.AddTransient<IPedidoCasosDeUso, PedidoCasosDeUso>();
        services.AddTransient<IPedidoComboCasosDeUso, PedidoComboCasosDeUso>();
        services.AddTransient<IPedidoComboItemCasosDeUso, PedidoComboItemCasosDeUso>();
        services.AddTransient<IPedidoStatusCasosDeUso, PedidoStatusCasosDeUso>();
    }
}
