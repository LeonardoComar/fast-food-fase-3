using Entidades.Clientes;
using FluentNHibernate.Mapping;

namespace Infra.Clientes.Map
{
    public class ClienteMap : ClassMap<Cliente>
    {
        public ClienteMap()
        {
            Table("clientes");
            Id(c => c.Codigo).Column("codigo").GeneratedBy.Sequence("clientes_codigo_seq");
            Map(c => c.Cpf).Column("cpf").Not.Nullable();
            Map(c => c.Nome).Column("nome").Not.Nullable();
            Map(c => c.Email).Column("email").Not.Nullable();
        }
    }
}
