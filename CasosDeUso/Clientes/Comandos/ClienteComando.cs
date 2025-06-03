namespace CasosDeUso.Clientes.Comandos
{
    public class ClienteComando
    {
        public int Codigo { get; set; }
        public required string Cpf { get; set; }
        public required string Email { get; set; }
        public required string Nome { get; set; }
    }
}
