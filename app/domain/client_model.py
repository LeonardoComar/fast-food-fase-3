from sqlalchemy import Column, Integer, String
from pydantic import BaseModel, Field
from typing import List, Optional

from app.core.mysql_connection import Base

# SQLAlchemy model for database mapping
class ClientDB(Base):
    __tablename__ = "clients"
    
    id = Column(Integer, primary_key=True, index=True)
    name = Column(String(255), nullable=False)
    cpf = Column(String(14), nullable=False, unique=True)

# Pydantic model for API requests
class ClientCreate(BaseModel):
    name: str
    cpf: str

# Pydantic model for API responses
class ClientResponse(BaseModel):
    id: int
    name: str
    cpf: str
    
    class Config:
        from_attributes = True

# Pydantic model for multiple clients response
class ClientListResponse(BaseModel):
    clients: List[ClientResponse]

# Pydantic model for client name updates
class ClientUpdate(BaseModel):
    name: str
