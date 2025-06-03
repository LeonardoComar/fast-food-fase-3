using Entidades.Pedidos;
using Entidades.Pedidos.Enums;
using FluentNHibernate.Mapping;

namespace Infra.Pedidos.Map
{
    public class PedidoPagamentoMap : ClassMap<PedidoPagamento>
    {
        public PedidoPagamentoMap()
        {
            Table("pedido_pagamento");
            Id(x => x.Codigo).Column("codigo").GeneratedBy.Sequence("pedido_pagamento_codigo_seq");
            References(x => x.Pedido).Column("codigo_pedido").Not.Nullable().Cascade.Delete();
            Map(x => x.Valor).Column("valor").Not.Nullable();
            Map(x => x.DataPagamento).Column("data_pagamento").Nullable();
            Map(x => x.Metodo).Column("forma_pagamento").CustomType<MetodoPagamentoEnum>().Length(50).Nullable();
        }
    }
}
