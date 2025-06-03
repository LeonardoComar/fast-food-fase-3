namespace InterfaceAdapters.Autenticacao.Gateway.Interfaces
{
    public interface IAutenticacaoGateway
    {
        Task<string?> ObterNomePorTokenAsync(string jwtToken);
    }
}
