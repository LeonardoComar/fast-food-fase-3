import os
from sqlalchemy import create_engine, text
from sqlalchemy.exc import OperationalError, ProgrammingError

# Configurações do banco de dados
DB_USER = os.getenv("DB_USER", "fastfood_user")
DB_PASSWORD = os.getenv("DB_PASSWORD", "Mudar123!")
DB_HOST = os.getenv("DB_HOST", "db-fastfood")
DB_PORT = os.getenv("DB_PORT", "3306")
DB_NAME = os.getenv("DB_NAME", "fastfood")
SQL_ECHO = os.getenv("SQL_ECHO", "false").lower() == "true"

def create_db_tables():
    # Criar banco de dados se não existir
    create_database_if_not_exists()
    
    # Criar tabelas
    create_tables()

def create_database_if_not_exists():
    # URL sem o nome do banco específico
    base_url = f"mysql+pymysql://{DB_USER}:{DB_PASSWORD}@{DB_HOST}:{DB_PORT}/"
    
    try:
        engine = create_engine(base_url, echo=SQL_ECHO)
        with engine.connect() as conn:
            # Verificar se o banco de dados existe
            result = conn.execute(text(f"SHOW DATABASES LIKE '{DB_NAME}'"))
            if not result.first():
                conn.execute(text(f"CREATE DATABASE {DB_NAME}"))
                print(f"Banco de dados '{DB_NAME}' criado com sucesso!")
    except OperationalError as e:
        print(f"Erro de conexão com o MySQL: {e}")
        raise
    finally:
        if 'engine' in locals():
            engine.dispose()

def create_tables():
    # URL com o nome do banco específico
    db_url = f"mysql+pymysql://{DB_USER}:{DB_PASSWORD}@{DB_HOST}:{DB_PORT}/{DB_NAME}"
    
    try:
        engine = create_engine(db_url, echo=SQL_ECHO)
        with engine.connect() as conn:
            # Criação das tabelas (substitua com seu schema real)
            conn.execute(text("""
                CREATE TABLE IF NOT EXISTS products (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    name VARCHAR(255) NOT NULL,
                    category VARCHAR(100) NOT NULL,
                    price FLOAT NOT NULL,
                    description TEXT
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
            """))
            
            # Criar tabela de clientes
            conn.execute(text("""
                CREATE TABLE IF NOT EXISTS clients (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    name VARCHAR(255) NOT NULL,
                    cpf VARCHAR(14) NOT NULL UNIQUE
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
            """))
            
            # Criar tabela de pedidos (depende da tabela clients)
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
            # Adicione mais tabelas conforme necessário
            print("Tabelas criadas/verificadas com sucesso!")
    except ProgrammingError as e:
        print(f"Erro SQL ao criar tabelas: {e}")
        raise
    except OperationalError as e:
        print(f"Erro de conexão com o banco: {e}")
        raise
    finally:
        if 'engine' in locals():
            engine.dispose()