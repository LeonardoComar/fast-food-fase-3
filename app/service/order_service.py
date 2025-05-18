from sqlalchemy.orm import Session
from typing import Optional
from app.repository.order_repository import OrderRepository
from app.domain.order_model import OrderResponse, OrderListResponse, OrderCreate, OrderStatusUpdate

class OrderService:
    def __init__(self, db_session: Session):
        self.repository = OrderRepository(db_session)
    
    def get_all_orders(self) -> OrderListResponse:
        """
        Get all orders and return them as a response model
        """
        orders = self.repository.get_all_orders()
        return OrderListResponse(
            orders=[OrderResponse.model_validate(order) for order in orders]
        )
    
    def get_orders_by_status(self, status: str) -> OrderListResponse:
        """
        Get orders filtered by status
        """
        orders = self.repository.get_orders_by_status(status)
        return OrderListResponse(
            orders=[OrderResponse.model_validate(order) for order in orders]
        )
    
    def get_order_by_id(self, order_id: int) -> OrderResponse:
        """
        Get an order by its ID
        """
        order = self.repository.get_order_by_id(order_id)
        if not order:
            raise ValueError(f"Order with ID {order_id} not found")
        return OrderResponse.model_validate(order)
    
    def create_order(self, order_data: OrderCreate) -> OrderResponse:
        """
        Create a new order
        """
        # Convert Pydantic model to dict for the repository
        order_dict = order_data.model_dump()
        order = self.repository.create_order(order_dict)
        return OrderResponse.model_validate(order)
    
    def update_order_status(self, order_id: int, status_data: OrderStatusUpdate) -> OrderResponse:
        """
        Update an order's status
        """
        # Check if the order exists
        existing_order = self.repository.get_order_by_id(order_id)
        if not existing_order:
            raise ValueError(f"Order with ID {order_id} not found")
        
        # Update the order's status
        updated_order = self.repository.update_order_status(order_id, status_data.status)
        return OrderResponse.model_validate(updated_order)
