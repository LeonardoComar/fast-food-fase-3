using CasosDeUso.Produtos.Enums;

namespace CasosDeUso.Produtos.Comandos
{
    public class ProdutoComando
    {
        public int Codigo { get; set; }
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public decimal Preco { get; set; }
        public CategoriaEnum? Categoria { get; set; }
        public AtivoInativoEnum? Situacao { get; set; }
    }
}
