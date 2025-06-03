using InterfaceAdapters.Produtos.Controllers.Interfaces;
using InterfaceAdapters.Produtos.Presenters.Requests;
using InterfaceAdapters.Produtos.Presenters.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Produtos
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoController _produtoController;

        public ProdutosController(IProdutoController produtoController)
        {
            _produtoController = produtoController;
        }


        [HttpPost]
        public ActionResult<ProdutoResponse> Inserir(ProdutoRequest produtoRequest)
        {

            return _produtoController.Inserir(produtoRequest);
        }

        [HttpPut]
        [Route("{codigo}")]
        public ActionResult<ProdutoResponse> Alterar(int codigo, ProdutoRequest produtoRequest)
        {

            return _produtoController.Alterar(codigo, produtoRequest);
        }


        [HttpGet]
        public ActionResult<List<ProdutoResponse>> Consultar([FromQuery] ProdutoFiltroRequest produtoFiltroRequest)
        {
            return _produtoController.Consultar(produtoFiltroRequest);
        }

        [HttpGet]
        [Route("{codigo}")]
        public ActionResult<ProdutoResponse> Consultar(int codigo)
        {
            return _produtoController.Consultar(codigo);
        }
    }

}
