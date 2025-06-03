namespace InterfaceAdapters.Clientes.Presenters.Responses
{
    public class ClienteResponse
    {
        public int Codigo { get; set; }
        public required string Cpf { get; set; }
        public required string Email { get; set; }
        public required string Nome { get; set; }
    }
}
