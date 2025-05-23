import pytest
from pydantic import ValidationError
from app.domain.order_model import OrderCreate, OrderResponse, OrderStatus, OrderDB, OrderStatusUpdate

class TestOrderModel:
    def test_order_create_valid(self):
        # Test valid order creation
        order_data = {
            "client_id": 1,
            "total_price": 25.99,
            "products": [
                {"id": 1, "name": "Product 1", "quantity": 2, "price": 10.99},
                {"id": 2, "name": "Product 2", "quantity": 1, "price": 4.01}
            ],
            "status": OrderStatus.RECEIVED
        }
        order = OrderCreate(**order_data)
        
        assert order.client_id == 1
        assert order.total_price == 25.99
        assert len(order.products) == 2
        assert order.status == OrderStatus.RECEIVED
    
    def test_order_create_default_status(self):
        # Test order creation without specifying status (should use default)
        order_data = {
            "client_id": 1,
            "total_price": 25.99,
            "products": [
                {"id": 1, "name": "Product 1", "quantity": 2, "price": 10.99},
                {"id": 2, "name": "Product 2", "quantity": 1, "price": 4.01}
            ]
        }
        order = OrderCreate(**order_data)
        
        assert order.status == OrderStatus.RECEIVED
    
    def test_order_create_invalid_status(self):
        # Test order creation with invalid status
        order_data = {
            "client_id": 1,
            "total_price": 25.99,
            "products": [
                {"id": 1, "name": "Product 1", "quantity": 2, "price": 10.99}
            ],
            "status": "Invalid Status"
        }
        
        with pytest.raises(ValidationError):
            OrderCreate(**order_data)
    
    def test_order_response_from_db(self):
        # Test creating OrderResponse from OrderDB
        db_order = OrderDB(
            id=1,
            client_id=1,
            total_price=25.99,
            status=OrderStatus.PREPARING,
            products=[
                {"id": 1, "name": "Product 1", "quantity": 2, "price": 10.99},
                {"id": 2, "name": "Product 2", "quantity": 1, "price": 4.01}
            ]
        )
        
        # Using model_validate to create from SQLAlchemy model
        response = OrderResponse.model_validate(db_order)
        
        assert response.id == 1
        assert response.client_id == 1
        assert response.total_price == 25.99
        assert response.status == OrderStatus.PREPARING
        assert len(response.products) == 2
    
    def test_order_status_update_valid(self):
        # Test valid status update
        status_data = {
            "status": OrderStatus.PREPARING
        }
        status_update = OrderStatusUpdate(**status_data)
        
        assert status_update.status == OrderStatus.PREPARING
    
    def test_order_status_update_invalid(self):
        # Test invalid status update
        status_data = {
            "status": "Invalid Status"
        }
        
        with pytest.raises(ValidationError):
            OrderStatusUpdate(**status_data)
