using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System.Reflection;

namespace Infra
{
    internal static class ConfiguracoesNHibernate
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<ISessionFactory>(factory =>
            {
                return Fluently.Configure().Database(() =>
                {
                    return PostgreSQLConfiguration.Standard
                            .FormatSql()
                            .ShowSql()
                            .ConnectionString(configuration.GetConnectionString("DefaultConnection"));
                })
                   .Mappings(c => c.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                   .CurrentSessionContext("call")
                   .BuildSessionFactory();
            });

            services.AddScoped<ISession>(factory =>
            {
                return factory.GetService<ISessionFactory>().OpenSession();
            });

            return services;
        }
    }
}