from sqlalchemy.orm import Session
from typing import List, Optional, Dict, Any
from app.domain.order_model import OrderDB

class OrderRepository:
    def __init__(self, db_session: Session):
        self.db_session = db_session
    
    def get_all_orders(self) -> List[OrderDB]:
        """
        Retrieve all orders from the database
        """
        return self.db_session.query(OrderDB).all()
    
    def get_orders_by_status(self, status: str) -> List[OrderDB]:
        """
        Retrieve orders filtered by status
        """
        return self.db_session.query(OrderDB).filter(OrderDB.status == status).all()
    
    def get_order_by_id(self, order_id: int) -> Optional[OrderDB]:
        """
        Retrieve an order by its ID
        """
        return self.db_session.query(OrderDB).filter(OrderDB.id == order_id).first()
    
    def create_order(self, order_data: Dict[str, Any]) -> OrderDB:
        """
        Create a new order in the database
        """
        new_order = OrderDB(**order_data)
        self.db_session.add(new_order)
        self.db_session.commit()
        self.db_session.refresh(new_order)
        return new_order
    
    def update_order_status(self, order_id: int, new_status: str) -> OrderDB:
        """
        Update an order's status
        """
        order = self.db_session.query(OrderDB).filter(OrderDB.id == order_id).first()
        if not order:
            raise ValueError(f"Order with ID {order_id} not found")
        
        # Update status
        order.status = new_status
        
        # Commit changes
        self.db_session.commit()
        self.db_session.refresh(order)
        return order
