import pytest
from unittest.mock import patch, MagicMock
from fastapi import status
from app.domain.order_model import OrderResponse, OrderListResponse, OrderStatus

class TestOrderRoutes:
    def setup_method(self):
        # Sample order data
        self.sample_order = {
            "id": 1,
            "client_id": 1,
            "total_price": 25.99,
            "status": OrderStatus.RECEIVED,
            "products": [
                {"id": 1, "name": "Product 1", "quantity": 2, "price": 10.99},
                {"id": 2, "name": "Product 2", "quantity": 1, "price": 4.01}
            ]
        }
        
        self.sample_order_response = OrderResponse(**self.sample_order)
        
        self.sample_order_create = {
            "client_id": 2,
            "total_price": 15.50,
            "products": [
                {"id": 3, "name": "Product 3", "quantity": 1, "price": 15.50}
            ]
        }
        
        self.sample_status_update = {
            "status": OrderStatus.PREPARING
        }
    
    @patch('app.api.order_routes.OrderService')
    def test_get_all_orders(self, mock_service_class, authenticated_client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        
        order_list = OrderListResponse(orders=[self.sample_order_response])
        mock_service.get_all_orders.return_value = order_list
        
        # Act
        response = authenticated_client.get("/api/orders/")
        
        # Assert
        assert response.status_code == status.HTTP_200_OK
        assert response.json() == {"orders": [self.sample_order]}
        mock_service.get_all_orders.assert_called_once()
    
    @patch('app.api.order_routes.OrderService')
    def test_get_orders_by_status(self, mock_service_class, authenticated_client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        
        order_list = OrderListResponse(orders=[self.sample_order_response])
        mock_service.get_orders_by_status.return_value = order_list
        
        # Act
        response = authenticated_client.get(f"/api/orders/?status={OrderStatus.RECEIVED.value}")
        
        # Assert
        assert response.status_code == status.HTTP_200_OK
        assert response.json() == {"orders": [self.sample_order]}
        mock_service.get_orders_by_status.assert_called_once()
    
    @patch('app.api.order_routes.OrderService')
    def test_get_order_by_id_found(self, mock_service_class, authenticated_client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.get_order_by_id.return_value = self.sample_order_response
        
        # Act
        response = authenticated_client.get("/api/orders/1")
        
        # Assert
        assert response.status_code == status.HTTP_200_OK
        assert response.json() == self.sample_order
        mock_service.get_order_by_id.assert_called_once_with(1)
    
    @patch('app.api.order_routes.OrderService')
    def test_get_order_by_id_not_found(self, mock_service_class, authenticated_client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.get_order_by_id.side_effect = ValueError("Order with ID 999 not found")
        
        # Act
        response = authenticated_client.get("/api/orders/999")
        
        # Assert
        assert response.status_code == status.HTTP_404_NOT_FOUND
        assert "not found" in response.json()["detail"]
    
    @patch('app.api.order_routes.OrderService')
    def test_create_order(self, mock_service_class, authenticated_client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.create_order.return_value = self.sample_order_response
        
        # Act
        response = authenticated_client.post("/api/orders/", json=self.sample_order_create)
        
        # Assert
        assert response.status_code == status.HTTP_201_CREATED
        assert response.json() == self.sample_order
        
        # Verify service was called with correct data
        mock_service.create_order.assert_called_once()
        
    @patch('app.api.order_routes.OrderService')
    def test_update_order_status(self, mock_service_class, authenticated_client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        
        updated_order = OrderResponse(
            id=1,
            client_id=1,
            total_price=25.99,
            status=OrderStatus.PREPARING,
            products=[
                {"id": 1, "name": "Product 1", "quantity": 2, "price": 10.99},
                {"id": 2, "name": "Product 2", "quantity": 1, "price": 4.01}
            ]
        )
        mock_service.update_order_status.return_value = updated_order
        
        # Act
        response = authenticated_client.patch("/api/orders/1/status", json=self.sample_status_update)
        
        # Assert
        assert response.status_code == status.HTTP_200_OK
        assert response.json()["status"] == OrderStatus.PREPARING
        mock_service.update_order_status.assert_called_once()
    
    @patch('app.api.order_routes.OrderService')
    def test_update_order_status_not_found(self, mock_service_class, authenticated_client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.update_order_status.side_effect = ValueError("Order with ID 999 not found")
        
        # Act
        response = authenticated_client.patch("/api/orders/999/status", json=self.sample_status_update)
        
        # Assert
        assert response.status_code == status.HTTP_404_NOT_FOUND
        assert "not found" in response.json()["detail"]
