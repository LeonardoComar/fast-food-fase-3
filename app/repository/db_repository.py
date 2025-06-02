import os
from sqlalchemy import create_engine, text
from sqlalchemy.exc import OperationalError, ProgrammingError
import boto3
from botocore.exceptions import ClientError

# Se estiver usando localmente, continue carregando .env
from dotenv import load_dotenv
load_dotenv()

# Variável de ambiente que aponta para o Secret (pode ser o ARN completo ou apenas o nome)
# Exemplo de valor: "arn:aws:secretsmanager:us-east-1:587167200064:secret:aws-secretsmanager-secret-fastfood-6RuX0W"
DB_SECRET_ID = os.getenv("DB_SECRET_ID")
AWS_REGION = os.getenv("AWS_REGION", "us-east-1")

# Parâmetro para habilitar log de SQLAlchemy (echo)
SQL_ECHO = os.getenv("SQL_ECHO", "false").lower() == "true"


def fetch_db_credentials_from_sm() -> dict:
    """
    Busca as credenciais do banco no AWS Secrets Manager.
    Espera-se que o secret_string tenha o formato:
        "Host=<endpoint>;Database=<nome_db>;Username=<usuario>;Password=<senha>"
    Retorna um dict: {"host": ..., "port": ..., "database": ..., "username": ..., "password": ...}.
    """
    if not DB_SECRET_ID:
        raise RuntimeError("A variável de ambiente DB_SECRET_ID não foi definida.")

    session = boto3.session.Session()
    client = session.client(service_name="secretsmanager", region_name=AWS_REGION)
    try:
        resp = client.get_secret_value(SecretId=DB_SECRET_ID)
    except ClientError as e:
        raise RuntimeError(f"Não foi possível recuperar o segredo do Secrets Manager: {e}")

    if "SecretString" not in resp:
        raise RuntimeError("O segredo não contém uma string legível.")

    secret_str = resp["SecretString"]
    # Exemplo: "Host=my-rds-instance.chi8akyshzbu.us-east-1.rds.amazonaws.com;Database=db_fastfood;Username=admin;Password=MinhaSenhaSecreta"

    parts = secret_str.split(";")
    cred_dict = {}
    for part in parts:
        if "=" in part:
            chave, valor = part.split("=", 1)
            cred_dict[chave.strip().lower()] = valor.strip()

    # Verificações mínimas
    for campo in ("host", "database", "username", "password"):
        if campo not in cred_dict:
            raise RuntimeError(f"Campo '{campo}' não encontrado no secret_string recuperado.")

    return {
        "host": cred_dict["host"],
        "port": cred_dict.get("port", "3306"),  # usa 3306 se não informado
        "database": cred_dict["database"],
        "username": cred_dict["username"],
        "password": cred_dict["password"]
    }


def create_db_tables():
    """
    Cria o banco de dados (se não existir) e depois cria/verifica as tabelas dentro dele.
    """
    create_database_if_not_exists()
    create_tables()


def create_database_if_not_exists():
    """
    Conecta ao MySQL sem especificar banco, verifica se o DB existe e cria se necessário.
    """
    creds = fetch_db_credentials_from_sm()
    host = creds["host"]
    port = creds["port"]
    user = creds["username"]
    pwd = creds["password"]
    db_name = creds["database"]

    # Monta URL sem indicar banco específico
    base_url = f"mysql+pymysql://{user}:{pwd}@{host}:{port}/"

    try:
        engine = create_engine(base_url, echo=SQL_ECHO)
        with engine.connect() as conn:
            # Verifica se o DB já existe
            result = conn.execute(text(f"SHOW DATABASES LIKE '{db_name}'"))
            if not result.first():
                conn.execute(text(f"CREATE DATABASE {db_name}"))
                print(f"Banco de dados '{db_name}' criado com sucesso!")
    except OperationalError as e:
        print(f"Erro de conexão com o MySQL (ao checar/criar DB): {e}")
        raise
    finally:
        if 'engine' in locals():
            engine.dispose()


def create_tables():
    """
    Conecta ao banco específico e cria as tabelas (caso ainda não existam).
    """
    creds = fetch_db_credentials_from_sm()
    host = creds["host"]
    port = creds["port"]
    user = creds["username"]
    pwd = creds["password"]
    db_name = creds["database"]

    # Monta URL indicando o banco
    db_url = f"mysql+pymysql://{user}:{pwd}@{host}:{port}/{db_name}"

    try:
        engine = create_engine(db_url, echo=SQL_ECHO)
        with engine.connect() as conn:
            # Criação da tabela products
            conn.execute(text("""
                CREATE TABLE IF NOT EXISTS products (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    name VARCHAR(255) NOT NULL,
                    category VARCHAR(100) NOT NULL,
                    price FLOAT NOT NULL,
                    description TEXT
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
            """))

            # Criação da tabela clients
            conn.execute(text("""
                CREATE TABLE IF NOT EXISTS clients (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    name VARCHAR(255) NOT NULL,
                    cpf VARCHAR(14) NOT NULL UNIQUE
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
            """))

            # Criação da tabela orders (dependendo de clients)
            conn.execute(text("""
                CREATE TABLE IF NOT EXISTS orders (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    client_id INT NOT NULL,
                    total_price FLOAT NOT NULL,
                    status VARCHAR(50) NOT NULL,
                    products JSON NOT NULL,
                    FOREIGN KEY (client_id) REFERENCES clients(id)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
            """))

            # Adicione outras tabelas aqui, se necessário

            print("Tabelas criadas/verificadas com sucesso!")
    except ProgrammingError as e:
        print(f"Erro SQL ao criar tabelas: {e}")
        raise
    except OperationalError as e:
        print(f"Erro de conexão com o banco (na criação de tabelas): {e}")
        raise
    finally:
        if 'engine' in locals():
            engine.dispose()