using Entidades.Produtos.Enums;

namespace Entidades.Produtos
{
    public class Produto
    {
        private int produtoCodigo;

        public virtual int Codigo { get; set; }
        public virtual string Nome { get; protected set; }
        public virtual string? Descricao { get; protected set; }
        public virtual decimal Preco { get; protected set; }
        public virtual CategoriaEnum Categoria { get; protected set; }
        public virtual AtivoInativoEnum Situacao { get; protected set; }

        protected Produto()
        {
        }

        public Produto(int produtoCodigo)
        {
            this.produtoCodigo = produtoCodigo;
        }
    }
}
