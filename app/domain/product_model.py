from sqlalchemy import Column, Integer, String, Float, Text
from pydantic import BaseModel, Field, validator
from typing import Optional, List
from enum import Enum

from app.core.mysql_connection import Base

# SQLAlchemy model for database mapping
class ProductDB(Base):
    __tablename__ = "products"
    
    id = Column(Integer, primary_key=True, index=True)
    name = Column(String(255), nullable=False)
    category = Column(String(100), nullable=False)
    price = Column(Float, nullable=False)
    description = Column(Text, nullable=True)

# Enum for category validation
class ProductCategory(str, Enum):
    SANDWICH = "Lanche"
    SIDE_DISH = "Acompanhamento"
    DRINK = "Bebida"
    DESSERT = "Sobremesa"

# Pydantic model for API requests
class ProductCreate(BaseModel):
    name: str
    category: ProductCategory
    price: float
    description: Optional[str] = None

# Pydantic model for API responses
class ProductResponse(BaseModel):
    id: int
    name: str
    category: str
    price: float
    description: Optional[str] = None
    
    class Config:
        from_attributes = True

# Pydantic model for multiple products response
class ProductListResponse(BaseModel):
    products: List[ProductResponse]
