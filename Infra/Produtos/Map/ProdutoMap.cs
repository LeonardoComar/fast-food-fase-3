using Entidades.Produtos;
using Entidades.Produtos.Enums;
using FluentNHibernate.Mapping;

namespace Infra.Produtos.Map
{
    public class ProdutoMap : ClassMap<Produto>
    {
        public ProdutoMap()
        {
            Table("produtos");
            Id(c => c.Codigo).Column("codigo").GeneratedBy.Sequence("produtos_codigo_seq");
            Map(c => c.Nome).Column("nome").Not.Nullable();
            Map(c => c.Descricao).Column("descricao").Not.Nullable();
            Map(c => c.Preco).Column("preco").Not.Nullable();
            Map(c => c.Situacao).Column("situacao").Not.Nullable().CustomType(typeof(AtivoInativoEnum));
            Map(c => c.Categoria).Column("categoria").Not.Nullable().CustomType(typeof(CategoriaEnum));
        }

    }

}
