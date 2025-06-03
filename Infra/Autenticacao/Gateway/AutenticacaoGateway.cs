using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using InterfaceAdapters.Autenticacao.Gateway.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Infra.Autenticacao.Gateway
{
    public class AutenticacaoGateway(IAmazonCognitoIdentityProvider cognitoClient, IConfiguration configuration) : IAutenticacaoGateway
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoClient = cognitoClient;
        private readonly string _userPoolId = configuration["AWS:CognitoUserPoolId"];

        public async Task<string?> ObterNomePorTokenAsync(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var nameClaim = token.Claims.FirstOrDefault(c => c.Type == "cpf")?.Value;

            if (string.IsNullOrEmpty(nameClaim))
            {
                throw new Exception("JWT não contém o atributo 'name'.");
            }

            Console.WriteLine($"Nome extraído do JWT: {nameClaim}");

            var filter = $"username = \"{nameClaim}\"";

            var request = new ListUsersRequest
            {
                UserPoolId = _userPoolId,
                Filter = filter,
                Limit = 1
            };

            var response = await _cognitoClient.ListUsersAsync(request);
            var user = response.Users.FirstOrDefault();

            return user?.Attributes.FirstOrDefault(a => a.Name == "custom:jwtToken")?.Value == jwtToken ? user?.Username : null;
        }
    }
}