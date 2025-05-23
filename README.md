# fast-food-fase-3

# Pendências:
- Criar a imagem da minha aplicação no docker hub
- criar recursos no AWS Academy usando o Terraform
    - RDS
    - EKS
    - API Gateway
    - Lambda 
    - Cognito

- Adaptar o código para conexão com o banco de dados gerenciavel
- Fazer a conexão com Cognito para receber o token de acesso e configurar uma labda simples
- Atualizar Postman para rodar as requisições do AWS
- Criar CI/CD da aplicação

# Comandos:
## Docker:
docker-compose build
docker-compose up -d
docker-compose down -v

## Testes
pytest --cov=app app/tests/