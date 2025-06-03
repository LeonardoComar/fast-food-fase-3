import pytest
from sqlalchemy.orm import Session
from unittest.mock import MagicMock, patch
from app.repository.product_repository import ProductRepository
from app.domain.product_model import ProductDB

class TestProductRepository:
    def setup_method(self):
        # Create a mock session for each test
        self.mock_session = MagicMock(spec=Session)
        self.repository = ProductRepository(self.mock_session)
        
        # Sample product data
        self.sample_product = ProductDB(
            id=1, 
            name="Test Product", 
            category="Lanche", 
            price=10.99, 
            description="Test description"
        )
    
    def test_get_all_products(self):
        # Arrange
        self.mock_session.query.return_value.all.return_value = [self.sample_product]
        
        # Act
        products = self.repository.get_all_products()
        
        # Assert
        self.mock_session.query.assert_called_once_with(ProductDB)
        assert len(products) == 1
        assert products[0].id == 1
        assert products[0].name == "Test Product"
    
    def test_get_product_by_id_found(self):
        # Arrange
        self.mock_session.query.return_value.filter.return_value.first.return_value = self.sample_product
        
        # Act
        product = self.repository.get_product_by_id(1)
        
        # Assert
        self.mock_session.query.assert_called_once_with(ProductDB)
        self.mock_session.query.return_value.filter.assert_called_once()
        assert product.id == 1
        assert product.name == "Test Product"
    
    def test_get_product_by_id_not_found(self):
        # Arrange
        self.mock_session.query.return_value.filter.return_value.first.return_value = None
        
        # Act
        product = self.repository.get_product_by_id(999)
        
        # Assert
        assert product is None
    
    def test_create_product(self):
        # Arrange
        product_data = {
            "name": "New Product",
            "category": "Bebida",
            "price": 5.99,
            "description": "New product description"
        }
        
        # Mock add and commit methods
        self.mock_session.add = MagicMock()
        self.mock_session.commit = MagicMock()
        self.mock_session.refresh = MagicMock()
        
        # Mock the behavior of creating a new product
        with patch('app.repository.product_repository.ProductDB', return_value=ProductDB(
            id=2, **product_data
        )):
            # Act
            new_product = self.repository.create_product(product_data)
            
            # Assert
            self.mock_session.add.assert_called_once()
            self.mock_session.commit.assert_called_once()
            self.mock_session.refresh.assert_called_once()
            assert new_product.id == 2
            assert new_product.name == "New Product"
            assert new_product.category == "Bebida"
    
    def test_update_product_found(self):
        # Arrange
        product_to_update = ProductDB(
            id=1, 
            name="Original Product", 
            category="Lanche", 
            price=10.99, 
            description="Original description"
        )
        
        self.mock_session.query.return_value.filter.return_value.first.return_value = product_to_update
        
        update_data = {
            "name": "Updated Product",
            "price": 12.99
        }
        
        # Act
        updated_product = self.repository.update_product(1, update_data)
        
        # Assert
        self.mock_session.commit.assert_called_once()
        self.mock_session.refresh.assert_called_once()
        assert updated_product.id == 1
        assert updated_product.name == "Updated Product"
        assert updated_product.price == 12.99
        # Category should remain unchanged
        assert updated_product.category == "Lanche"
    
    def test_update_product_not_found(self):
        # Arrange
        self.mock_session.query.return_value.filter.return_value.first.return_value = None
        
        update_data = {
            "name": "Updated Product",
            "price": 12.99
        }
        
        # Act & Assert
        with pytest.raises(ValueError, match="Product with ID 999 not found"):
            self.repository.update_product(999, update_data)
