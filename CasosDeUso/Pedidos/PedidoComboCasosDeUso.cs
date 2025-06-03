using CasosDeUso.Pedidos.Interfaces;
using CasosDeUso.Pedidos.Interfaces.Gateway;
using CasosDeUso.Produtos.Interfaces;
using Entidades.Pedidos;
using Entidades.Util;

namespace CasosDeUso.Pedidos
{
    public class PedidoComboCasosDeUso(IPedidoComboGateway pedidoComboGateway, IPedidoComboItemCasosDeUso pedidoComboItemCasosDeUso, IProdutoCasosDeUso produtoCasosDeUso) : IPedidoComboCasosDeUso
    {
        public PedidoCombo Inserir(PedidoCombo pedidoCombo)
        {

            pedidoComboGateway.Inserir(pedidoCombo);

            foreach (var pedidoComboItem in pedidoCombo.PedidoComboItem)
            {
                var produto = produtoCasosDeUso.RetornarPorCodigo(pedidoComboItem.Produto.Codigo) ??
                    throw new RegraNegocioException("Produto não econtrado");

                var pedidoItem = new PedidoComboItem(pedidoCombo, produto);

                pedidoComboItemCasosDeUso.Inserir(pedidoItem);

            }

            return pedidoCombo;
        }


        public PedidoCombo Inserir(int codigo, PedidoCombo pedidoCombo)
        {
            if (!pedidoCombo.IsValid())
                throw new RegraNegocioException("O combo deve conter pelo menos um item.");

            Inserir(pedidoCombo);

            return pedidoCombo;
        }


        public void Remover(int codigoCombo)
        {
            var pedidoCombo = pedidoComboGateway.RetornarPorCodigo(codigoCombo) ??
                throw new RegraNegocioException("Combo do pedido não localizado");

            pedidoComboGateway.Excluir(pedidoCombo);
        }
    }
}
