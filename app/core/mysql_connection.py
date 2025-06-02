from sqlalchemy import create_engine, Engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker, Session
from contextlib import contextmanager
import os
import boto3
from botocore.exceptions import ClientError
from dotenv import load_dotenv

# Carrega as variáveis de ambiente do .env (se estiver usando localmente)
load_dotenv()

# Classe base para os modelos
Base = declarative_base()

def fetch_db_credentials_from_sm() -> dict:
    """
    Busca as credenciais do banco no AWS Secrets Manager.
    Espera-se que o secret_string tenha o formato:
        "Host=<endpoint>;Database=<nome_db>;Username=<usuario>;Password=<senha>"
    Retorna um dict: {"host": ..., "database": ..., "username": ..., "password": ...}.
    """
    secret_id = os.getenv("DB_SECRET_ID")  # Ex.: arn ou nome do secret (aqui: aws-secretsmanager-secret-fastfood)
    region = os.getenv("AWS_REGION", "us-east-1")
    
    if not secret_id:
        raise RuntimeError("A variável de ambiente DB_SECRET_ID não foi definida.")

    session = boto3.session.Session()
    client = session.client(service_name="secretsmanager", region_name=region)
    try:
        resp = client.get_secret_value(SecretId=secret_id)
    except ClientError as e:
        # Você pode logar o erro ou tratar casos específicos (AccessDenied, ResourceNotFound, etc).
        raise RuntimeError(f"Não foi possível recuperar o segredo: {e}")

    if "SecretString" not in resp:
        raise RuntimeError("O segredo não contém uma string legível.")
    secret_str = resp["SecretString"]  # ex.: "Host=...;Database=...;Username=...;Password=..."
    
    # Vamos quebrar pelo ';' e depois pelo '=' para extrair chaves e valores
    parts = secret_str.split(";")
    cred_dict = {}
    for part in parts:
        if "=" in part:
            chave, valor = part.split("=", 1)
            chave = chave.strip().lower()      # exemplo: "host", "database", "username", "password"
            valor = valor.strip()
            cred_dict[chave] = valor

    # Verificações mínimas
    for campo in ("host", "database", "username", "password"):
        if campo not in cred_dict:
            raise RuntimeError(f"Campo '{campo}' não encontrado no secret_string recuperado.")

    return {
        "host": cred_dict["host"],
        "port": cred_dict.get("port", "3306"),  # caso não tenha explicitado port no secret, usa 3306
        "database": cred_dict["database"],
        "username": cred_dict["username"],
        "password": cred_dict["password"]
    }

def get_connection_url() -> str:
    """
    Monta a connection URL do MySQL usando credenciais obtidas do AWS Secrets Manager.
    """
    creds = fetch_db_credentials_from_sm()
    host = creds["host"]
    port = creds["port"]
    user = creds["username"]
    pwd = creds["password"]
    db_name = creds["database"]

    # Exemplo: "mysql+pymysql://user:pwd@host:3306/db_name"
    return f"mysql+pymysql://{user}:{pwd}@{host}:{port}/{db_name}"

def get_engine() -> Engine:
    """
    Cria e retorna a instância do SQLAlchemy Engine,
    usando a URL obtida do Secrets Manager.
    """
    connection_url = get_connection_url()
    return create_engine(
        connection_url,
        echo=os.getenv("SQL_ECHO", "False").lower() == "true",
        pool_pre_ping=True,
        pool_recycle=3600,
        pool_timeout=30,  # Timeout para conexões
        pool_size=5,      # Ajuste conforme necessidade
        max_overflow=10   # Conexões extras quando o pool estiver cheio
    )

# Cria um factory de sessões (o engine será inicializado apenas uma vez, na importação)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=get_engine())

@contextmanager
def get_db_session():
    """
    Context manager para sessao do DB.
    Uso:
        with get_db_session() as session:
            # faz operações com session
    """
    session = SessionLocal()
    try:
        yield session
        session.commit()
    except Exception as e:
        session.rollback()
        raise e
    finally:
        session.close()

def get_db():
    """
    Gerador para injeção de dependência no FastAPI.
    Uso:
        @app.get("/itens/")
        def read_items(db: Session = Depends(get_db)):
            ...
    """
    session = SessionLocal()
    try:
        yield session
    finally:
        session.close()
