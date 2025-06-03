using InterfaceAdapters.Produtos.Enums;

namespace InterfaceAdapters.Produtos.Presenters.Requests
{
    public class ProdutoFiltroRequest
    {
        public string? Nome { get; set; }
        public AtivoInativoEnum? Situacao { get; set; }
        public CategoriaEnum? Categoria { get; set; }
    }
}
