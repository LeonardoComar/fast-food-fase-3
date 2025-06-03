import pytest
from sqlalchemy.orm import Session
from unittest.mock import MagicMock, patch
from app.repository.order_repository import OrderRepository
from app.domain.order_model import OrderDB, OrderStatus

class TestOrderRepository:
    def setup_method(self):
        # Create a mock session for each test
        self.mock_session = MagicMock(spec=Session)
        self.repository = OrderRepository(self.mock_session)
        
        # Sample order data
        self.sample_order = OrderDB(
            id=1, 
            client_id=1,
            total_price=25.99,
            status=OrderStatus.RECEIVED,
            products=[
                {"id": 1, "name": "Product 1", "quantity": 2, "price": 10.99},
                {"id": 2, "name": "Product 2", "quantity": 1, "price": 4.01}
            ]
        )
    
    def test_get_all_orders(self):
        # Arrange
        self.mock_session.query.return_value.all.return_value = [self.sample_order]
        
        # Act
        orders = self.repository.get_all_orders()
        
        # Assert
        self.mock_session.query.assert_called_once_with(OrderDB)
        assert len(orders) == 1
        assert orders[0].id == 1
        assert orders[0].status == OrderStatus.RECEIVED
    
    def test_get_orders_by_status(self):
        # Arrange
        self.mock_session.query.return_value.filter.return_value.all.return_value = [self.sample_order]
        
        # Act
        orders = self.repository.get_orders_by_status(OrderStatus.RECEIVED)
        
        # Assert
        self.mock_session.query.assert_called_once_with(OrderDB)
        self.mock_session.query.return_value.filter.assert_called_once()
        assert len(orders) == 1
        assert orders[0].status == OrderStatus.RECEIVED
    
    def test_get_order_by_id_found(self):
        # Arrange
        self.mock_session.query.return_value.filter.return_value.first.return_value = self.sample_order
        
        # Act
        order = self.repository.get_order_by_id(1)
        
        # Assert
        self.mock_session.query.assert_called_once_with(OrderDB)
        self.mock_session.query.return_value.filter.assert_called_once()
        assert order.id == 1
        assert order.client_id == 1
    
    def test_get_order_by_id_not_found(self):
        # Arrange
        self.mock_session.query.return_value.filter.return_value.first.return_value = None
        
        # Act
        order = self.repository.get_order_by_id(999)
        
        # Assert
        assert order is None
    
    def test_create_order(self):
        # Arrange
        order_data = {
            "client_id": 2,
            "total_price": 15.50,
            "status": OrderStatus.RECEIVED,
            "products": [
                {"id": 3, "name": "Product 3", "quantity": 1, "price": 15.50}
            ]
        }
        
        # Mock add and commit methods
        self.mock_session.add = MagicMock()
        self.mock_session.commit = MagicMock()
        self.mock_session.refresh = MagicMock()
        
        # Mock the behavior of creating a new order
        with patch('app.repository.order_repository.OrderDB', return_value=OrderDB(
            id=2, **order_data
        )):
            # Act
            new_order = self.repository.create_order(order_data)
            
            # Assert
            self.mock_session.add.assert_called_once()
            self.mock_session.commit.assert_called_once()
            self.mock_session.refresh.assert_called_once()
            assert new_order.id == 2
            assert new_order.client_id == 2
            assert new_order.status == OrderStatus.RECEIVED
    
    def test_update_order_status_found(self):
        # Arrange
        order_to_update = OrderDB(
            id=1, 
            client_id=1,
            total_price=25.99,
            status=OrderStatus.RECEIVED,
            products=[
                {"id": 1, "name": "Product 1", "quantity": 2, "price": 10.99},
                {"id": 2, "name": "Product 2", "quantity": 1, "price": 4.01}
            ]
        )
        
        self.mock_session.query.return_value.filter.return_value.first.return_value = order_to_update
        
        # Act
        updated_order = self.repository.update_order_status(1, OrderStatus.PREPARING)
        
        # Assert
        self.mock_session.commit.assert_called_once()
        self.mock_session.refresh.assert_called_once()
        assert updated_order.id == 1
        assert updated_order.status == OrderStatus.PREPARING
    
    def test_update_order_status_not_found(self):
        # Arrange
        self.mock_session.query.return_value.filter.return_value.first.return_value = None
        
        # Act & Assert
        with pytest.raises(ValueError, match="Order with ID 999 not found"):
            self.repository.update_order_status(999, OrderStatus.PREPARING)
