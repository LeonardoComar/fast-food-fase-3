from sqlalchemy import create_engine, Engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker, Session
from contextlib import contextmanager
import os
from dotenv import load_dotenv

# Load environment variables
load_dotenv()

# Base class for all models
Base = declarative_base()

def get_connection_url() -> str:
    """
    Build database connection URL from environment variables or default values.
    """
    db_user = os.getenv("DB_USER", "fastfood_user")
    db_password = os.getenv("DB_PASSWORD", "Mudar123!")
    db_host = os.getenv("DB_HOST", "db-fastfood")
    db_port = os.getenv("DB_PORT", "3306")
    db_name = os.getenv("DB_NAME", "fastfood")
    
    # Build MySQL connection URL
    return f"mysql+pymysql://{db_user}:{db_password}@{db_host}:{db_port}/{db_name}"

def get_engine() -> Engine:
    """
    Create and return a SQLAlchemy engine instance.
    """
    connection_url = get_connection_url()
    return create_engine(
        connection_url,
        echo=os.getenv("SQL_ECHO", "False").lower() == "true",
        pool_pre_ping=True,
        pool_recycle=3600,
        pool_timeout=30,
        pool_size=5,
        max_overflow=10
    )

# Create a session factory
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=get_engine())

@contextmanager
def get_db_session():
    """
    Context manager for database sessions.
    Usage:
        with get_db_session() as session:
            # use session for database operations
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
    Generator function for FastAPI dependency injection.
    Usage:
        @app.get("/items/")
        def read_items(db: Session = Depends(get_db)):
            # use db for database operations
    """
    session = SessionLocal()
    try:
        yield session
    finally:
        session.close()