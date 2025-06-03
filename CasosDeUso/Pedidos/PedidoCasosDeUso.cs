using AutoMapper;
using CasosDeUso.Clientes.Interfaces;
using CasosDeUso.Pedidos.Comandos;
using CasosDeUso.Pedidos.Interfaces;
using CasosDeUso.Pedidos.Interfaces.Gateway;
using CasosDeUso.Produtos.Interfaces;
using Entidades.Clientes;
using Entidades.Pedidos;
using Entidades.Util;
using MetodoPagamentoEnum = CasosDeUso.Pedidos.Enums.MetodoPagamentoEnum;
using StatusPedido = CasosDeUso.Pedidos.Enums.StatusPedido;

namespace CasosDeUso.Pedidos
{
    public class PedidoCasosDeUso(IMapper mapper,
                                  IPedidoGateway pedidoGateway,
                                  IPedidoComboGateway pedidoComboGateway,
                                  IPedidoComboItemGateway pedidoComboItemGateway,
                                  IPedidoPagamentoGateway pedidoPagamentoGateway,
                                  IPedidoStatusGateway pedidoStatusGateway,
                                  IClienteCasosDeUso clienteCasosDeUso,
                                  IPedidoComboCasosDeUso pedidoComboCasosDeUso,
                                  IProdutoCasosDeUso produtoCasosDeUso,
                                  IPedidoStatusCasosDeUso pedidoStatusCasosDeUso) : IPedidoCasosDeUso
    {
        public List<PedidoComando> Consultar()
        {
            var pedidos = pedidoGateway.Consultar();

            var pedidosComando = mapper.Map<List<PedidoComando>>(pedidos);

            return pedidosComando;
        }


        public PedidoComando ConsultarPorCodigo(int codigo)
        {
            var pedido = pedidoGateway.RetornarPorCodigo(codigo) ??
                throw new RegraNegocioException("Pedido não localizado");

            return mapper.Map<PedidoComando>(pedido);
        }

        public List<PedidoComando> ConsultarPedidosCozinha()
        {
            var pedidoStatus = pedidoStatusGateway.Query()
                                                  .Select(ps => new PedidoStatusComando
                                                  {
                                                      PedidoCodigo = ps.Pedido.Codigo,
                                                      DataCriacao = ps.DataCriacao,
                                                      Status = (StatusPedido)ps.Status
                                                  })
                                                  .Where(ps => ps.DataCriacao == pedidoStatusGateway
                                                  .Query()
                                                  .Where(subPs => subPs.Pedido.Codigo == ps.PedidoCodigo)
                                                  .Max(subPs => subPs.DataCriacao))
                                                  .ToList()
                                                  .Where(ps => ps != null && (ps.Status == StatusPedido.EmPreparo || ps.Status == StatusPedido.Recebido))
                                                  .OrderBy(ps => ps.Status)
                                                  .OrderBy(ps => ps.DataCriacao)
                                                  .ToList();




            if (pedidoStatus.Count > 0)
            {
                var codigosPedidos = pedidoStatus.Select(ps => ps.PedidoCodigo)
                                                 .Distinct()
                                                 .ToList();

                var pedidos = pedidoGateway.Query()
                                           .Where(p => codigosPedidos.Contains(p.Codigo))
                                           .ToList();

                return mapper.Map<List<PedidoComando>>(pedidos);
            }

            return [];
        }
        public List<PedidoStatusComando> ConsultarPedidosMonitor()
        {
            var pedidoStatusComando = pedidoStatusGateway.Query()
                                                      .Select(ps => new PedidoStatusComando
                                                      {
                                                          PedidoCodigo = ps.Pedido.Codigo,
                                                          DataCriacao = ps.DataCriacao,
                                                          Status = (StatusPedido)ps.Status
                                                      })
                                                      .Where(ps => ps.DataCriacao == pedidoStatusGateway
                                                      .Query()
                                                      .Where(subPs => subPs.Pedido.Codigo == ps.PedidoCodigo)
                                                      .Max(subPs => subPs.DataCriacao))
                                                      .ToList()
                                                      .Where(ps => ps != null && (ps.Status == StatusPedido.Pronto || ps.Status == StatusPedido.EmPreparo || ps.Status == StatusPedido.Recebido))
                                                      .OrderBy(ps => ps.Status)
                                                      .OrderBy(ps => ps.DataCriacao)
                                                      .ToList();
            if (pedidoStatusComando.Count > 0)
                return pedidoStatusComando;

            return [];
        }

        public PedidoComando CadastrarPedido(PedidoComando pedidoComando,string? token)
        {
            var clienteComando = !string.IsNullOrEmpty(token) ? _ = clienteCasosDeUso.ConsultarPorToken(token) : null;

            if (pedidoComando.PedidoCombo == null || !pedidoComando.PedidoCombo.Any())
                throw new RegraNegocioException("Pedido deve conter pelo menos um combo.");

            var cliente = mapper.Map<Cliente>(clienteComando);

            var pedido = new Pedido(cliente);

            pedidoGateway.Inserir(pedido);

            foreach (var pedidoComboComando in pedidoComando.PedidoCombo)
            {
                var pedidoCombo = new PedidoCombo(pedido);

                pedidoComboGateway.Inserir(pedidoCombo);

                foreach (var pedidoComboItemComando in pedidoComboComando.PedidoComboItem)
                {
                    var produto = produtoCasosDeUso.RetornarPorCodigo(pedidoComboItemComando.CodigoProduto);

                    var pedidoComboItem = new PedidoComboItem(pedidoCombo, produto);

                    pedidoComboItemGateway.Inserir(pedidoComboItem);
                }
            }

            var pedidoStatus = new PedidoStatus(pedido, (Entidades.Pedidos.Enums.StatusPedido)StatusPedido.AguardandoPagamento, DateTime.Now);

            pedidoStatusGateway.Inserir(pedidoStatus);

            return mapper.Map<PedidoComando>(pedido);
        }

        public PedidoStatusComando ConsultarStatus(int codigo)
        {
            var pedido = RetornarPorCodigo(codigo);

            var pedidoStatus = pedido.PedidoStatus.OrderByDescending(ps => ps.DataCriacao).FirstOrDefault() ??
                  throw new RegraNegocioException("Pedido não possui status");

            return mapper.Map<PedidoStatusComando>(pedidoStatus);
        }

        public PedidoComando AlterarStatus(int codigo, StatusPedido statusPedido)
        {
            var pedido = RetornarPorCodigo(codigo);

            var ultimoStatus = pedido.PedidoStatus.OrderByDescending(ps => ps.DataCriacao).FirstOrDefault() ??
                  throw new RegraNegocioException("Pedido não possui status");

            switch (statusPedido)
            {
                case StatusPedido.Cancelado:
                    if ((StatusPedido)ultimoStatus.Status != StatusPedido.AguardandoPagamento)
                        throw new RegraNegocioException("Pedido já foi pago e não pode ser cancelado");
                    break;
                case StatusPedido.Recebido:
                    throw new RegraNegocioException("Alteração de pedido para recebido só pode ser realizada pelo metodo de pagamento");
                case StatusPedido.Finalizado:
                    if ((StatusPedido)ultimoStatus.Status != StatusPedido.Pronto)
                        throw new RegraNegocioException("Pedido precisa estar como 'Pronto' para ser finalizado");
                    break;
                case StatusPedido.EmPreparo:
                    if ((StatusPedido)ultimoStatus.Status != StatusPedido.Recebido)
                        throw new RegraNegocioException("Pedido precisa estar 'Recebido' para ser entrar em preparo");
                    break;
                case StatusPedido.Pronto:
                    if ((StatusPedido)ultimoStatus.Status != StatusPedido.EmPreparo)
                        throw new RegraNegocioException("Pedido precisa estar 'Em Preparo' para ser finalizado");
                    break;
                default:
                    throw new RegraNegocioException("Pedido não pode ser alterado para esse status");
            }

            pedidoStatusCasosDeUso.Inserir(pedido, statusPedido);

            pedidoGateway.Refresh(pedido);

            return mapper.Map<PedidoComando>(pedido);
        }

        public PedidoComando InserirCombo(int codigo, PedidoComboComando pedidoComboComando)
        {
            var pedido = ObtemValidaPedidoAlteracao(codigo);

            var pedidoCombo = mapper.Map<PedidoCombo>(pedidoComboComando);

            pedidoCombo.SetPedido(pedido);

            pedidoComboCasosDeUso.Inserir(pedidoCombo);

            pedidoGateway.Refresh(pedido);

            return mapper.Map<PedidoComando>(pedido);
        }

        public PedidoComando RemoverCombo(int codigo, int pedidoCombo)
        {
            Pedido pedido = ObtemValidaPedidoAlteracao(codigo);

            pedidoComboCasosDeUso.Remover(pedidoCombo);

            pedidoGateway.Refresh(pedido);

            return mapper.Map<PedidoComando>(pedido);
        }

        public async Task<string> ConfirmarPedido(int codigo, PedidoPagamentoComando pedidoPagamentoComando, MetodoPagamentoEnum metodoPagamento)
        {
            var pedido = RetornarPorCodigo(codigo);

            if (pedido.PedidoStatus.Any(p => p.Status != (Entidades.Pedidos.Enums.StatusPedido)StatusPedido.EmPreparo))
                throw new RegraNegocioException("Esse pedido já possui pagamento");

            var pedidoPagamento = mapper.Map<PedidoPagamento>(pedidoPagamentoComando);

            if (pedido.CalcularTotal() != pedidoPagamento.Valor)
                throw new RegraNegocioException("Valor do pedido incorreto");


            if (pedido.PedidoStatus.Max(p => p.Status != (Entidades.Pedidos.Enums.StatusPedido)StatusPedido.AguardandoPagamento))
            {
                var pedidoStatus = new PedidoStatus(pedido, (Entidades.Pedidos.Enums.StatusPedido)StatusPedido.AguardandoPagamento, DateTime.Now);

                pedidoStatusGateway.Inserir(pedidoStatus);
            }

            var pedidoComando = mapper.Map<PedidoComando>(pedido);

            var QrCode = await pedidoPagamentoGateway.RealizarPagamento(pedidoComando, metodoPagamento);

            return QrCode;
        }

        public async Task<PedidoComando> ConfirmarPagamento(string codigoPagamento, MetodoPagamentoEnum metodoPagamento)
        {

            var codigoPedido = await pedidoPagamentoGateway.ObterPagamento(codigoPagamento, metodoPagamento);

            var pedido = RetornarPorCodigo(int.Parse(codigoPedido));

            if (pedido.PedidoPagamento != null)
                throw new RegraNegocioException("Esse pedido já possui pagamento");

            var pedidoPagamento = new PedidoPagamento(pedido, pedido.CalcularTotal(), DateTime.Now, (Entidades.Pedidos.Enums.MetodoPagamentoEnum)MetodoPagamentoEnum.Pix);

            if (pedido.CalcularTotal() != pedidoPagamento.Valor)
                throw new RegraNegocioException("Valor do pedido incorreto");

            pedidoPagamento.SetPedido(pedido);

            pedidoPagamentoGateway.Inserir(pedidoPagamento);

            var pedidoStatus = new PedidoStatus(pedido, (Entidades.Pedidos.Enums.StatusPedido)StatusPedido.EmPreparo, DateTime.Now);

            pedidoStatusGateway.Inserir(pedidoStatus);

            return mapper.Map<PedidoComando>(pedido);
        }

        private Pedido ObtemValidaPedidoAlteracao(int codigo)
        {
            var pedido = RetornarPorCodigo(codigo);

            if (pedido.PedidoStatus.Where(ps => ps.Status != (Entidades.Pedidos.Enums.StatusPedido)StatusPedido.AguardandoPagamento).Any())
                throw new RegraNegocioException("Pedido não pode ser alterado");

            return pedido;
        }

        private Pedido RetornarPorCodigo(int codigo)
        {
            var pedido = pedidoGateway.RetornarPorCodigo(codigo) ??
                throw new RegraNegocioException("Pedido não localizado");

            return pedido;
        }
    }
}
