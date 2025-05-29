import pytest
from unittest.mock import patch, MagicMock
from fastapi import status
from fastapi.testclient import TestClient
from app.main import app
from app.domain.product_model import ProductCreate, ProductResponse, ProductListResponse

@pytest.fixture
def client():
    return TestClient(app)

class TestProductRoutes:
    def setup_method(self):
        # Sample product data
        self.sample_product = {
            "id": 1,
            "name": "Test Product",
            "category": "Lanche",
            "price": 10.99,
            "description": "Test description"
        }
        
        self.sample_product_response = ProductResponse(**self.sample_product)
        
        self.sample_product_create = {
            "name": "New Product",
            "category": "Bebida",
            "price": 5.99,
            "description": "New product description"
        }
    
    @patch('app.api.product_routes.ProductService')
    def test_get_all_products(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        
        product_list = ProductListResponse(products=[self.sample_product_response])
        mock_service.get_all_products.return_value = product_list
        
        # Act
        response = client.get("/api/products/")
        
        # Assert
        assert response.status_code == status.HTTP_200_OK
        assert response.json() == {"products": [self.sample_product]}
        mock_service.get_all_products.assert_called_once()
    
    @patch('app.api.product_routes.ProductService')
    def test_get_product_by_id_found(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.get_product_by_id.return_value = self.sample_product_response
        
        # Act
        response = client.get("/api/products/1")
        
        # Assert
        assert response.status_code == status.HTTP_200_OK
        assert response.json() == self.sample_product
        mock_service.get_product_by_id.assert_called_once_with(1)
    
    @patch('app.api.product_routes.ProductService')
    def test_get_product_by_id_not_found(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.get_product_by_id.side_effect = ValueError("Product with ID 999 not found")
        
        # Act
        response = client.get("/api/products/999")
        
        # Assert
        assert response.status_code == status.HTTP_404_NOT_FOUND
        assert "not found" in response.json()["detail"]
    
    @patch('app.api.product_routes.ProductService')
    def test_create_product(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.create_product.return_value = self.sample_product_response
        
        # Act
        response = client.post("/api/products/", json=self.sample_product_create)
        
        # Assert
        assert response.status_code == status.HTTP_201_CREATED
        assert response.json() == self.sample_product
        
        # Verify service was called with correct data
        mock_service.create_product.assert_called_once()
        
    @patch('app.api.product_routes.ProductService')
    def test_update_product_found(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        
        updated_product = ProductResponse(
            id=1,
            name="Updated Product",
            category="Lanche",
            price=12.99,
            description="Updated description"
        )
        mock_service.update_product.return_value = updated_product
        
        # Act
        response = client.put("/api/products/1", json=self.sample_product_create)
        
        # Assert
        assert response.status_code == status.HTTP_200_OK
        assert response.json()["name"] == "Updated Product"
        mock_service.update_product.assert_called_once()
    
    @patch('app.api.product_routes.ProductService')
    def test_update_product_not_found(self, mock_service_class, client):
        # Arrange
        mock_service = MagicMock()
        mock_service_class.return_value = mock_service
        mock_service.update_product.side_effect = ValueError("Product with ID 999 not found")
        
        # Act
        response = client.put("/api/products/999", json=self.sample_product_create)
        
        # Assert
        assert response.status_code == status.HTTP_404_NOT_FOUND
        assert "not found" in response.json()["detail"]
