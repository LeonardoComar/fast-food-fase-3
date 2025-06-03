using Entidades.Pedidos;
using Entidades.Pedidos.Enums;
using FluentNHibernate.Mapping;

namespace Infra.Pedidos.Map
{
    public class PedidoStatusMap : ClassMap<PedidoStatus>
    {
        public PedidoStatusMap()
        {
            Table("pedido_status");
            Id(x => x.Codigo).Column("codigo").GeneratedBy.Sequence("pedido_status_codigo_seq");
            Map(x => x.Status).Column("status").CustomType<StatusPedido>().Not.Nullable();
            Map(x => x.DataCriacao).Column("data_criacao").Not.Nullable().Default("CURRENT_TIMESTAMP");
            References(x => x.Pedido).Column("codigo_pedido").Not.Nullable().Cascade.Delete();
        }
    }
}
