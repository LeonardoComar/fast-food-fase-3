from sqlalchemy import Column, Integer, String, Float, ForeignKey, JSON
from pydantic import BaseModel, Field
from typing import Optional, List, Dict, Any
from enum import Enum

from app.core.mysql_connection import Base

# SQLAlchemy model for database mapping
class OrderDB(Base):
    __tablename__ = "orders"
    
    id = Column(Integer, primary_key=True, index=True)
    client_id = Column(Integer, ForeignKey("clients.id"), nullable=False)
    total_price = Column(Float, nullable=False)
    status = Column(String(50), nullable=False)
    products = Column(JSON, nullable=False)

# Enum for order status validation
class OrderStatus(str, Enum):
    RECEIVED = "Recebido"
    PREPARING = "Em preparação"
    READY = "Pronto"
    FINISHED = "Finalizado"

# Pydantic model for API requests
class OrderCreate(BaseModel):
    client_id: int
    total_price: float
    products: List[Dict[str, Any]]
    status: OrderStatus = OrderStatus.RECEIVED

# Pydantic model for status updates
class OrderStatusUpdate(BaseModel):
    status: OrderStatus

# Pydantic model for API responses
class OrderResponse(BaseModel):
    id: int
    client_id: int
    total_price: float
    status: str
    products: List[Dict[str, Any]]
    
    class Config:
        from_attributes = True

# Pydantic model for multiple orders response
class OrderListResponse(BaseModel):
    orders: List[OrderResponse]
