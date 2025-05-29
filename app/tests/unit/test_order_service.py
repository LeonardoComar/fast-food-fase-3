import pytest
from unittest.mock import MagicMock, patch
from app.service.order_service import OrderService
from app.domain.order_model import OrderCreate, OrderResponse, OrderListResponse, OrderDB, OrderStatus, OrderStatusUpdate

class TestOrderService:
    def setup_method(self):
        # Create a mock repository for each test
        self.mock_repository = MagicMock()
        
        # Create the service with the mock repository
        with patch('app.service.order_service.OrderRepository', return_value=self.mock_repository):
            self.mock_db_session = MagicMock()
            self.service = OrderService(self.mock_db_session)
        
        # Sample order data
        self.sample_order_db = OrderDB(
            id=1, 
            client_id=1,
            total_price=25.99,
            status=OrderStatus.RECEIVED,
            products=[
                {"id": 1, "name": "Product 1", "quantity": 2, "price": 10.99},
                {"id": 2, "name": "Product 2", "quantity": 1, "price": 4.01}
            ]
        )
        
        self.sample_order_create = OrderCreate(
            client_id=2,
            total_price=15.50,
            products=[
                {"id": 3, "name": "Product 3", "quantity": 1, "price": 15.50}
            ]
        )
        
        self.sample_status_update = OrderStatusUpdate(
            status=OrderStatus.PREPARING
        )
    
    def test_get_all_orders(self):
        # Arrange
        self.mock_repository.get_all_orders.return_value = [self.sample_order_db]
        
        # Act
        result = self.service.get_all_orders()
        
        # Assert
        self.mock_repository.get_all_orders.assert_called_once()
        assert isinstance(result, OrderListResponse)
        assert len(result.orders) == 1
        assert result.orders[0].id == 1
        assert result.orders[0].client_id == 1
    
    def test_get_orders_by_status(self):
        # Arrange
        self.mock_repository.get_orders_by_status.return_value = [self.sample_order_db]
        
        # Act
        result = self.service.get_orders_by_status(OrderStatus.RECEIVED)
        
        # Assert
        self.mock_repository.get_orders_by_status.assert_called_once_with(OrderStatus.RECEIVED)
        assert isinstance(result, OrderListResponse)
        assert len(result.orders) == 1
        assert result.orders[0].status == OrderStatus.RECEIVED
    
    def test_get_order_by_id_found(self):
        # Arrange
        self.mock_repository.get_order_by_id.return_value = self.sample_order_db
        
        # Act
        result = self.service.get_order_by_id(1)
        
        # Assert
        self.mock_repository.get_order_by_id.assert_called_once_with(1)
        assert isinstance(result, OrderResponse)
        assert result.id == 1
        assert result.client_id == 1
    
    def test_get_order_by_id_not_found(self):
        # Arrange
        self.mock_repository.get_order_by_id.return_value = None
        
        # Act & Assert
        with pytest.raises(ValueError, match="Order with ID 999 not found"):
            self.service.get_order_by_id(999)
    
    def test_create_order(self):
        # Arrange
        created_order = OrderDB(
            id=2, 
            client_id=self.sample_order_create.client_id,
            total_price=self.sample_order_create.total_price,
            status=OrderStatus.RECEIVED,
            products=self.sample_order_create.products
        )
        self.mock_repository.create_order.return_value = created_order
        
        # Act
        result = self.service.create_order(self.sample_order_create)
        
        # Assert
        self.mock_repository.create_order.assert_called_once()
        # Verify the dict passed to repository has correct data
        order_dict = self.mock_repository.create_order.call_args[0][0]
        assert order_dict["client_id"] == 2
        assert order_dict["total_price"] == 15.50
        assert len(order_dict["products"]) == 1
        
        assert isinstance(result, OrderResponse)
        assert result.id == 2
        assert result.client_id == 2
    
    def test_update_order_status_found(self):
        # Arrange
        self.mock_repository.get_order_by_id.return_value = self.sample_order_db
        
        updated_order = OrderDB(
            id=1, 
            client_id=1,
            total_price=25.99,
            status=OrderStatus.PREPARING,
            products=[
                {"id": 1, "name": "Product 1", "quantity": 2, "price": 10.99},
                {"id": 2, "name": "Product 2", "quantity": 1, "price": 4.01}
            ]
        )
        self.mock_repository.update_order_status.return_value = updated_order
        
        # Act
        result = self.service.update_order_status(1, self.sample_status_update)
        
        # Assert
        self.mock_repository.get_order_by_id.assert_called_once_with(1)
        self.mock_repository.update_order_status.assert_called_once_with(1, OrderStatus.PREPARING)
        assert isinstance(result, OrderResponse)
        assert result.id == 1
        assert result.status == OrderStatus.PREPARING
    
    def test_update_order_status_not_found(self):
        # Arrange
        self.mock_repository.get_order_by_id.return_value = None
        
        # Act & Assert
        with pytest.raises(ValueError, match="Order with ID 999 not found"):
            self.service.update_order_status(999, self.sample_status_update)
