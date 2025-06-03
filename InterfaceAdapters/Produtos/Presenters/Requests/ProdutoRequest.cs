using InterfaceAdapters.Produtos.Enums;

namespace InterfaceAdapters.Produtos.Presenters.Requests
{
    public class ProdutoRequest
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public decimal Preco { get; set; }
        public CategoriaEnum Categoria { get; set; }
        public AtivoInativoEnum Situacao { get; set; }
    }
}