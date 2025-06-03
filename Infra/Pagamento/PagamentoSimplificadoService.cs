using CasosDeUso.Pedidos.Comandos;
using CasosDeUso.Pedidos.Enums;
using CasosDeUso.Pedidos.Interfaces;
using System.Text;

namespace Infra.Pagamento
{
    public class PagamentoSimplificadoService : IPagamentoService
    {
        public async Task<string> GerarCodigoPagamento(PedidoComando pedidoComando, MetodoPagamentoEnum metodoPagamento)
        {
            // Lógica simplificada para gerar um código de pagamento
            string codigoBase = $"PAG-{metodoPagamento}-{pedidoComando.Codigo}-{DateTime.Now:yyyyMMddHHmmss}";
            return await Task.FromResult(Convert.ToBase64String(Encoding.UTF8.GetBytes(codigoBase)));
        }

        public async Task<string> ValidarPagamento(string codigoPagamento, MetodoPagamentoEnum metodoPagamento)
        {
            try
            {
                // Decodificar o código e extrair o código do pedido
                string decodedCode = Encoding.UTF8.GetString(Convert.FromBase64String(codigoPagamento));
                string[] parts = decodedCode.Split('-');
                if (parts.Length >= 3)
                {
                    return await Task.FromResult(parts[2]);
                }
                return await Task.FromResult("0");
            }
            catch
            {
                // Em caso de erro, assumimos que é o próprio código do pedido
                return await Task.FromResult(codigoPagamento);
            }
        }
    }
}
