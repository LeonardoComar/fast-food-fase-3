using Entidades.Pedidos;
using FluentNHibernate.Mapping;

namespace Infra.Pedidos.Map
{
    public class PedidoMap : ClassMap<Pedido>
    {
        public PedidoMap()
        {
            Table("pedidos");
            Id(x => x.Codigo).Column("codigo").GeneratedBy.Sequence("pedidos_codigo_seq");
            References(x => x.Cliente).Column("codigo_cliente").Nullable().Cascade.None();
            HasMany(x => x.PedidoCombo).KeyColumn("codigo_pedido").Cascade.All().Inverse();
            HasMany(x => x.PedidoStatus).KeyColumn("codigo_pedido").Cascade.All().Inverse();
            HasOne(x => x.PedidoPagamento).Cascade.All();
        }
    }
}
