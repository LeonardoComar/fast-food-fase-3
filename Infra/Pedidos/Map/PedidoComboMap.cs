using Entidades.Pedidos;
using FluentNHibernate.Mapping;

namespace Infra.Pedidos.Map
{
    public class PedidoComboMap : ClassMap<PedidoCombo>
    {
        public PedidoComboMap()
        {
            Table("pedido_combos");
            Id(x => x.Codigo).Column("codigo").GeneratedBy.Sequence("pedido_combos_codigo_seq");
            References(x => x.Pedido).Column("codigo_pedido").Not.Nullable().Cascade.Delete();
            Map(x => x.Preco).Column("preco").Not.Nullable();
            HasMany(x => x.PedidoComboItem).Cascade.All().Inverse();
        }
    }
}
