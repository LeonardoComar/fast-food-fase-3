using CasosDeUso.Clientes.Interfaces.Gateway;
using CasosDeUso.Pedidos.Interfaces.Gateway;
using CasosDeUso.Produtos.Interfaces.Gateway;
using InterfaceAdapters.Clientes.Controllers;
using InterfaceAdapters.Clientes.Controllers.Interfaces;
using InterfaceAdapters.Clientes.Gateway;
using InterfaceAdapters.Pedidos.Controllers;
using InterfaceAdapters.Pedidos.Controllers.Interfaces;
using InterfaceAdapters.Pedidos.Gateway;
using InterfaceAdapters.Produtos.Controllers;
using InterfaceAdapters.Produtos.Controllers.Interfaces;
using InterfaceAdapters.Produtos.Gateway;
using Microsoft.Extensions.DependencyInjection;

namespace InterfaceAdapters;

public static class InterfaceAdaptersBootstrapper
{
    public static void Register(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(InterfaceAdaptersBootstrapper));
        services.AddTransient<IClienteController, ClienteController>();
        services.AddTransient<IProdutoController, ProdutoController>();
        services.AddTransient<IPedidoController, PedidoController>();
        services.AddTransient<IClienteGateway, ClienteGateway>();
        services.AddTransient<IProdutoGateway, ProdutoGateway>();
        services.AddTransient<IPedidoGateway, PedidoGateway>();
        services.AddTransient<IPedidoStatusGateway, PedidoStatusGateway>();
        services.AddTransient<IPedidoPagamentoGateway, PedidoPagamentoGateway>();
        services.AddTransient<IPedidoComboItemGateway, PedidoComboItemGateway>();
        services.AddTransient<IPedidoComboGateway, PedidoComboGateway>();
    }
}
