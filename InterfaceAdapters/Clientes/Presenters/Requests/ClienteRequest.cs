namespace InterfaceAdapters.Clientes.Presenters.Requests
{
    public class ClienteRequest
    {
        public required string Cpf { get; set; }
        public required string Email { get; set; }
        public required string Nome { get; set; }
    }
}
